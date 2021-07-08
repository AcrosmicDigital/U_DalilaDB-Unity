using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using System.Runtime.Serialization;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections.Concurrent;

namespace U.DalilaDB
{

    [Serializable]
    public abstract class DalilaDBCollection<TCollection> where TCollection : DalilaDBCollection<TCollection>, new()
    {

        #region Basics


        protected static TCollection instance_ = null;  // A instance of the child class to have the overwrited properties
        public static TCollection _instance
        {
            get
            {
                if (instance_ == null)
                    instance_ = new TCollection();

                return instance_;

            }
        }


        private static DalilaFS fileSystem_ = null;  // The fileSystem to perform files operations
        private static DalilaFS _fileSystem
        {
            get
            {
                if (fileSystem_ == null)
                    fileSystem_ = new DalilaFS(_instance.rootPath_);

                return fileSystem_;
            }
        }


        private static TaskFactory taskQueue_;
        private static TaskFactory _taskQueue
        {
            get
            {
                if (taskQueue_ == null)
                {
                    taskQueue_ = new TaskFactory(new LimitedConcurrencyScheduler(1));
                }

                return taskQueue_;
            }
        }

        private static TaskFactory secondTaskQueue_;
        private static TaskFactory _secondTaskQueue
        {
            get
            {
                if (secondTaskQueue_ == null)
                {
                    secondTaskQueue_ = new TaskFactory(new LimitedConcurrencyScheduler(1));
                }

                return secondTaskQueue_;
            }
        }


        protected abstract string rootPath_ { get; }  // This can be overritd to change the path

        public static string RootPath => _fileSystem._root;  // Directory where DalilaDB is stored
        public static string Location => "/DalilaDB/DalilaDBCollections/" + _instance.GetType().Name + "/";
        public static string LocationPath => RootPath.TrimEnd('/') + Location;  // Get the path where the resources are stores
        public static string ResourceLocation(SID _id) => Location + _id + ".xml";  // Get the name of a resource referenced from the Root directory
        public static string ResourceLocation(TCollection document) => Location + document._id + ".xml";  // Get the name of a resource referenced from the Root directory
        public static string ResourcePath(SID _id) => RootPath.TrimEnd('/') + ResourceLocation(_id); // Get the full path of a specific resource
        public static string ResourcePath(TCollection document) => RootPath.TrimEnd('/') + ResourceLocation(document);  // Get the full path of a specific resource


        #endregion Basics

        #region SerializableItems


        // The id of the document
        [DataMember()]
        private SID id_;
        public SID _id {
            get
            {
                if (id_ == null)
                    id_ = new SID();

                return id_;
            }
            set
            {
                id_ = value;
            }
        }

        #endregion SerializableItems

        #region HotCache


        #region StoreAndCount


        protected virtual int cacheSize_ => 20; // This are the hot items to use, is function tocan be virtual
        public static int CacheSize   // hotElementsMax_ getter
        {
            get
            {
                return _instance.cacheSize_.MinInt(0);
            }
        }


        private static ConcurrentDictionary<string, TCollection> cacheStore = new ConcurrentDictionary<string, TCollection>();  // Concurent dictionary for cache


        public static int CacheCount()
        {
            return cacheStore.Count();
        }

        public static string[] CacheGet()
        {
            return cacheStore.Select(e => e.Key.ToString()).Select(k => DalilaFS.GetOnlyResourceName(k, ".xml")).Where(k => k != "").OrderBy(c => c).ToArray();
        }


        #endregion StoreAndCount

        #region Functions


        public static void CacheClear()  // Clear the cache memory
        {
            cacheStore.Clear();
        }

        private static void CacheAdd(string key, TCollection document)
        {
            // Try remove if exist
            CacheRemove(key);

            if (document == null || key == null)
                return;

            // If is full remove first element
            if (cacheStore.Count >= CacheSize)
            {
                try
                {
                    var first = cacheStore.First();
                    var remove = cacheStore.TryRemove(first.Key, out var s);
                    if (!remove)
                        return;
                }
                catch (Exception)
                {
                    return;
                }
            }

            var cloneOpp = DalilaFS.CloneResource(document);

            if (!cloneOpp)
                return;

            cacheStore.TryAdd(key, cloneOpp.Data);

        }

        private static void CacheRemove(string key)
        {
            if (key == null)
                return;

            cacheStore.TryRemove(key, out var ff); // Delete the hotKey if exist
        }

        private static DataOperation<TCollection> CacheReadByLocation(string key)
        {
            if (!cacheStore.TryGetValue(key, out var file))
                return new DataOperation<TCollection>().Fails(null, new FileNotFoundException());

            return DalilaFS.CloneResource(file);
        }

        private static DataOperation<TCollection> CacheReadOne(Func<TCollection, bool> predicate)
        {
            // Try to get the value from the hot dictionary
            var hotElement = cacheStore.Where(d =>
            {
                var pred = true;

                try
                {
                    if (predicate != null)
                        pred = predicate(d.Value);
                }
                catch (Exception)
                {
                    pred = false;
                }

                return pred;

            }).Select(d => d.Value).FirstOrDefault();

            if (hotElement != null)
                return DalilaFS.CloneResource(hotElement);

            return new DataOperation<TCollection>().Fails(null, new FileNotFoundException());

        }


        #endregion Functions


        #endregion HotCache




        #region ASYNC


        #region EXIST


        public static Task<bool> ExistAsync(SID _id)  // True if a document with the key exist
        {
            // Verificar que la key sea valida, nada de caracteres raros
            if (_id == null)
                throw new ArgumentNullException("_id cant be null");

            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {
                return _fileSystem.ExistResource(ResourceLocation(_id)).IsSuccessful;
            });
        }

        public static Task<string[]> ExistAsync()  // True if a document with the key exist
        {

            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {
                return _fileSystem.GetResources(Location).Select(k => DalilaFS.GetOnlyResourceName(k, ".xml")).Where(k => k != "").ToArray();
            });
        }

        public static Task<int> CountAsync()  // Return the number of documents in the collection
        {
            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {
                var files = _fileSystem.GetResources(Location);

                if (files != null)
                    return files.Length;
                else
                    return 0;
            });
        }


        #endregion EXIST

        #region SAVE


        public Task<DataOperation> SaveAsync()
        {
            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {

                var operation = new DataOperation();

                var opp = _fileSystem.CreateResource(ResourceLocation(_id), (TCollection)this);

                if (opp) CacheAdd(ResourceLocation(_id), (TCollection)this);  // Add to the hot dictionary
                else CacheRemove(ResourceLocation(_id));

                return opp;
            });
        }

        public Task<DataOperation> SaveNewAsync()
        {
            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            _id = new SID();

            return SaveAsync();

        }


        #endregion SAVENEW

        #region FIND


        public static Task<DataOperation<TCollection>> FindByIdAsync(SID id) => DoFindByIdOrDefaultAsync(id, false, null);

        public static Task<DataOperation<TCollection>> FindByIdOrDefaultAsync(SID id, TCollection defaultValue) => DoFindByIdOrDefaultAsync(id, true, defaultValue);

        private static Task<DataOperation<TCollection>> DoFindByIdOrDefaultAsync(SID id, bool useDefault, TCollection defaultValue)
        {
            // Verificar que la key sea valida, nada de caracteres raros
            if (id == null)
                throw new ArgumentNullException("_id cant be null");

            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {

                var opp = _fileSystem.ReadOrDeleteResource<TCollection>(ResourceLocation(id));

                if (opp)
                {
                    CacheAdd(ResourceLocation(id), opp.Data);  // Add to the hot dictionary
                    return opp;
                }
                else
                {
                    if (useDefault) return new DataOperation<TCollection>().Successful(defaultValue, "0");
                    else return opp;
                }


            });
        }



        public static Task<DataOperation<TCollection[]>> FindAllAsync() => FindAllAsync(-1);  // Negative is infinite

        public static Task<DataOperation<TCollection[]>> FindAllAsync(int limit) => FindManyAsync(d => true, limit);



        public static Task<DataOperation<TCollection[]>> FindManyAsync(Func<TCollection, bool> predicate) => FindManyAsync(predicate, -1);

        public static Task<DataOperation<TCollection[]>> FindManyAsync(Func<TCollection, bool> predicate, int limit)
        {
            if (predicate == null)
                throw new ArgumentNullException("Predicate in Delete() cant be null");

            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {
                var readed = 0;

                // Files opened
                var resources = _fileSystem.GetResources(Location);
                var files = new List<TCollection>();

                foreach (var resource in resources)
                {
                    // Limit the reads
                    readed++;
                    if (limit > 0 && readed > limit)
                        break;

                    var resourceName = resource;
                    var readFileTask = _secondTaskQueue.StartNew(() =>  // Task run id because can be runed in any thread to paralelize, but task will wait untill all readed
                    {

                        DataOperation<TCollection> readOperation;

                        // Try to get the value from the hot dictionary
                        readOperation = CacheReadByLocation(resource);

                        // If cant read from cache, read from a file
                        if (!readOperation)
                            readOperation = _fileSystem.ReadOrDeleteResource<TCollection>(resourceName);

                        if (readOperation)
                        {

                            CacheAdd(resourceName, readOperation.Data);  // Add to the hot dictionary

                            var pred = true;

                            try
                            {
                                if (predicate != null)
                                    pred = predicate(readOperation.Data);
                            }
                            catch (Exception)
                            {
                                pred = false;
                            }

                            if (pred)
                            {
                                return readOperation.Data;
                            }

                        }

                        return null;
                    });

                    readFileTask.Wait();  // Readed one by one in a aditional threat
                    if (readFileTask.Result != null)
                    {
                        files.Add(readFileTask.Result);
                        continue;
                    }

                    // If file not valid
                    readed--;

                }

                // Return the files
                var filesArray = files.ToArray();
                if (filesArray.Length > 0)
                    return new DataOperation<TCollection[]>().Successful(filesArray, files.Count() + "");
                else
                    return new DataOperation<TCollection[]>().Fails(filesArray, new FileNotFoundException());

            });

        }



        public static Task<DataOperation<TCollection>> FindOneAsync(Func<TCollection, bool> predicate) => DoFindOneOrDefaultAsync(predicate, false, null);

        public static Task<DataOperation<TCollection>> FindOneOrDefaultAsync(Func<TCollection, bool> predicate, TCollection defaultDoc) => DoFindOneOrDefaultAsync(predicate, true, defaultDoc);

        private static Task<DataOperation<TCollection>> DoFindOneOrDefaultAsync(Func<TCollection, bool> predicate, bool useDefault, TCollection defaultDoc)
        {
            if (predicate == null)
                throw new ArgumentNullException("Predicate in Delete() cant be null");

            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {

                // Try to get the value from the hot dictionary
                var hotSearch = CacheReadOne(predicate);
                if (hotSearch)
                    return hotSearch;


                // Search the value from the files
                var resources = _fileSystem.GetResources(Location);
                int filesOpen = 0;
                var tasks = new List<Task<TCollection>>();

                foreach (var resource in resources)
                {
                    var resourceName = resource;
                    filesOpen++;
                    tasks.Add(_secondTaskQueue.StartNew(() =>
                    {

                        var readOperation = _fileSystem.ReadOrDeleteResource<TCollection>(resourceName);

                        if (readOperation)
                        {

                            var pred = true;

                            try
                            {
                                if (predicate != null)
                                    pred = predicate(readOperation.Data);
                            }
                            catch (Exception)
                            {
                                pred = false;
                            }

                            if (pred)
                                return readOperation.Data;

                        }

                        return null;
                    }));


                    if (filesOpen >= 10)
                    {
                        // Await for the files readed
                        Task.WhenAll(tasks).Wait();
                        var ss = tasks.Where(t => t.Result != null).Select(t => t.Result).FirstOrDefault();

                        // Si ya lo encontro
                        if (ss != null)
                            break;

                        // Si no lo encontro
                        filesOpen = 0;
                        tasks.Clear();

                    }

                }

                // Await for the files readed
                Task.WhenAll(tasks).Wait();
                var ff = tasks.Where(t => t.Result != null).Select(t => t.Result).FirstOrDefault();

                // Return the file
                if (ff != null)
                {
                    CacheAdd(ResourceLocation(ff), ff);  // Add to the hot dictionary

                    return new DataOperation<TCollection>().Successful(ff, "1");
                }
                else
                {
                    if (useDefault) return new DataOperation<TCollection>().Successful(defaultDoc, "0");
                    else return new DataOperation<TCollection>().Fails(null, new FileNotFoundException());
                }




            });
        }


        #endregion FINDONE

        #region UPDATE


        private Task<DataOperation> ReplaceAsync()
        {
            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {
                var operation = new DataOperation();

                if (_id == null)
                    return operation.Fails(new ArgumentNullException("_id cant be null for Update"));


                var opp = _fileSystem.ReplaceResource(ResourceLocation(_id), this);

                if (opp) CacheAdd(ResourceLocation(_id), (TCollection)this);  // Add to the hot dictionary
                else CacheRemove(ResourceLocation(_id));

                return opp;
            });

        }

        public static Task<DataOperation> UpdateAllAsync(Func<TCollection, TCollection> updating) => UpdateManyAsync(updating, d => true);

        public static Task<DataOperation> UpdateManyAsync(Func<TCollection, TCollection> updating, Func<TCollection, bool> predicate)
        {
            if (updating == null || predicate == null)
                throw new ArgumentNullException("updatingFunc or predicate cant be null");

            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {

                // Find the files
                int updated = 0;
                var files = FindMany(predicate).Data;
                foreach (var file in files)
                {
                    var updateFails = false;
                    TCollection n = null;
                    try
                    {
                        n = updating(file);
                    }
                    catch (Exception)
                    {
                        updateFails = true;
                    }
                    if (!updateFails && n != null)
                    {
                        if (n.ReplaceAsync().Result)
                            updated++;
                    }
                }

                // Return the result
                return new DataOperation().Successful(updated + "");

            });
        }

        public static Task<DataOperation> UpdateOneAsync(Func<TCollection, TCollection> updating, Func<TCollection, bool> predicate)
        {
            if (updating == null || predicate == null)
                throw new ArgumentNullException("updatingFunc or predicate cant be null");

            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {
                // Find the files
                var findOperation = FindOne(predicate);

                if (findOperation)
                {
                    var updateFails = false;
                    TCollection n = null;
                    try
                    {
                        n = updating(findOperation.Data);
                    }
                    catch (Exception)
                    {
                        updateFails = true;
                    }

                    if (!updateFails && n != null)
                        return n.ReplaceAsync().Result;

                    return new DataOperation().Fails(new EvaluateException("Error updating Function"));

                }

                return findOperation;

            });
        }

        public static Task<DataOperation> UpdateByIdAsync(Func<TCollection, TCollection> updating, SID id)
        {
            // Verificar que la key sea valida, nada de caracteres raros
            if (id == null || updating == null)
                throw new ArgumentNullException("_id and updatingFunc cant be null");

            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {
                // Find the files
                var findOperation = FindById(id);

                if (findOperation)
                {
                    var updateFails = false;
                    TCollection n = null;
                    try
                    {
                        n = updating(findOperation.Data);
                    }
                    catch (Exception)
                    {
                        updateFails = true;
                    }

                    if (!updateFails && n != null)
                        return n.ReplaceAsync().Result;

                    return new DataOperation().Fails(new EvaluateException("Error evalueting updating Function"));
                }

                return findOperation;
            });
        }


        #endregion

        #region DELETE


        public static Task<DataOperation> DeleteAllAsync()
        {
            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {
                CacheClear();
                return _fileSystem.DeleteLocation(Location);
            });

        } 

        public static Task<DataOperation> DeleteManyAsync(Func<TCollection, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("Predicate in Delete() cant be null");

            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {
                // Find the files
                int deleted = 0;
                TCollection[] files = FindManyAsync(predicate).Result.Data;
                foreach (var file in files)
                {
                    CacheRemove(ResourceLocation(file));

                    var opp = _fileSystem.DeleteResource(ResourceLocation(file));
                    if (opp)
                        deleted++;
                }

                // Return the result
                return new DataOperation().Successful(deleted + "");
            });
        }

        public static Task<DataOperation> DeleteOneAsync(Func<TCollection, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("Predicate in Delete() cant be null");

            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {
                // Find the files
                var findoperation = FindOneAsync(predicate).Result;

                if (!findoperation)
                    return new DataOperation().Successful("0");

                CacheRemove(ResourceLocation(findoperation.Data));
                return _fileSystem.DeleteResource(ResourceLocation(findoperation.Data));

            });
        }

        public static Task<DataOperation> DeleteByIdAsync(SID id)
        {
            // Verificar que la key sea valida, nada de caracteres raros
            if (id == null)
                throw new ArgumentNullException("_id cant be null");

            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {

                CacheRemove(ResourceLocation(id));
                return _fileSystem.DeleteResource(ResourceLocation(id));

            });
        }


        #endregion


        #endregion ASYNC

        #region SYNC


        public static bool Exist(SID _id) => ExistAsync(_id).Result;
        public static string[] Exist() => ExistAsync().Result;
        public static int Count() => CountAsync().Result;


        public DataOperation Save() => SaveAsync().Result;
        public DataOperation SaveNew() => SaveNewAsync().Result;


        public static DataOperation<TCollection> FindById(SID id) => FindByIdAsync(id).Result;
        public static DataOperation<TCollection> FindByIdOrDefault(SID id, TCollection defaultDoc) => FindByIdOrDefaultAsync(id, defaultDoc).Result;
        public static DataOperation<TCollection[]> FindAll() => FindAllAsync().Result;
        public static DataOperation<TCollection[]> FindAll(int limit) => FindAllAsync(limit).Result;
        public static DataOperation<TCollection[]> FindMany(Func<TCollection, bool> predicate) => FindManyAsync(predicate).Result;
        public static DataOperation<TCollection[]> FindMany(Func<TCollection, bool> predicate, int limit) => FindManyAsync(predicate, limit).Result;
        public static DataOperation<TCollection> FindOne(Func<TCollection, bool> predicate) => FindOneAsync(predicate).Result;
        public static DataOperation<TCollection> FindOneOrDefault(Func<TCollection, bool> predicate, TCollection defaultDoc) => FindOneOrDefaultAsync(predicate, defaultDoc).Result;


        public static DataOperation UpdateById(Func<TCollection, TCollection> updating, SID id) => UpdateByIdAsync(updating, id).Result;
        public static DataOperation UpdateAll(Func<TCollection, TCollection> updating) => UpdateAllAsync(updating).Result;
        public static DataOperation UpdateOne(Func<TCollection, TCollection> updating, Func<TCollection, bool> predicate) => UpdateOneAsync(updating, predicate).Result;
        public static DataOperation UpdateMany(Func<TCollection, TCollection> updating, Func<TCollection, bool> predicate) => UpdateManyAsync(updating, predicate).Result;


        public static DataOperation DeleteById(SID id) => DeleteByIdAsync(id).Result;
        public static DataOperation DeleteAll() => DeleteAllAsync().Result;
        public static DataOperation DeleteOne(Func<TCollection, bool> predicate) => DeleteOneAsync(predicate).Result;
        public static DataOperation DeleteMany(Func<TCollection, bool> predicate) => DeleteManyAsync(predicate).Result;


        #endregion

        #region DEBUG

        public static void Define()
        {
            Debug.Log("----> ");

            Debug.Log(" - Name: " + _instance.GetType().Name);
            Debug.Log(" - Type: DalilaDBCollection");

            Debug.Log(" - Stored: " + Count());
            foreach (var id in Exist())
            {
                Debug.Log("   id: " + id);
            }

            Debug.Log(" - InCache: " + CacheCount() + " of " + CacheSize);
            foreach (var id in CacheGet())
            {
                Debug.Log("   id: " + id);
            }

            Debug.Log("<---- ");
        }

        #endregion

    }
}
