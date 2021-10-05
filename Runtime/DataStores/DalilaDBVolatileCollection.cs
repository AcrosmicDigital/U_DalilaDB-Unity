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
    public abstract class DalilaDBVolatileCollection<TCollection> where TCollection : DalilaDBVolatileCollection<TCollection>, new()
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


        protected virtual int cacheSize_ => 100; // This are the hot items to use, is function tocan be virtual
        public static int CacheSize   // hotElementsMax_ getter
        {
            get
            {
                return _instance.cacheSize_.MinInt(1);
            }
        }


        private static ConcurrentDictionary<SID, TCollection> cacheStore = new ConcurrentDictionary<SID, TCollection>();  // Concurent dictionary for cache


        public static int CacheCount()
        {
            return cacheStore.Count();
        }

        public static bool CacheExist(SID id)
        {
            return cacheStore.ContainsKey(id);
        }

        public static string[] CacheGet()
        {
            return cacheStore.Select(e => e.Key.ToString()).ToArray();
        }


        #endregion StoreAndCount

        #region Functions


        public static DataOperation CacheClear()  // Clear the cache memory and all the files
        {
            var opp = new DataOperation().Successful(cacheStore.Count() + "");
            cacheStore.Clear();
            return opp;
        }

        private static DataOperation CacheAdd(SID id, TCollection document)
        {
            // Try remove if exist
            var rmvOpp = CacheRemove(id);
            if (!rmvOpp)
                return rmvOpp;

            // If is null
            if (document == null || id == null)
                return new DataOperation().Fails(new ArgumentNullException());

            // If is full dont save
            if (cacheStore.Count >= CacheSize)
                return new DataOperation().Fails(new InvalidOperationException("Store is full with: " + CacheSize + " elements"));


            // Clone the object
            var cloneOpp = DalilaFS.CloneDCResource(document);

            if (!cloneOpp)
                return cloneOpp;

            // Add the object
            var added = cacheStore.TryAdd(id, cloneOpp.Data);

            if (added)
                return new DataOperation().Successful("1");
            else
                return new DataOperation().Fails(new InvalidDataException("Cant add save data"));
        }

        private static DataOperation CacheRemove(SID id)
        {
            if (id == null)
                return new DataOperation().Fails(new ArgumentNullException("Key cant be null"));

            if (!cacheStore.ContainsKey(id)) return new DataOperation().Successful("1");

            var rmv = cacheStore.TryRemove(id, out var ff); // Delete the hotKey if exist

            if (rmv)
                return new DataOperation().Successful("1");
            else
                return new DataOperation().Fails(new InvalidDataException("Cant remove data"));

        }

        private static DataOperation<TCollection> CacheReadById(SID id)
        {

            // Try to get the value from the hot dictionary
            var hotElement = cacheStore.Where(p => 
            {
                //Debug.Log("Itemid: " + p.Key + " searched: " + id + " equal: " + (p.Key.Equals(id)));
                return id == p.Key;
                //return id.Equals(p.Key);
            }).Select(p => p.Value).FirstOrDefault();

            if (hotElement != null)
                return DalilaFS.CloneDCResource(hotElement);

            return new DataOperation<TCollection>().Fails(null, new FileNotFoundException());

        }

        private static DataOperation<TCollection[]> CacheReadMany(Func<TCollection, bool> predicate, int limit)
        {
            // Try to get the value from the hot dictionary
            var hotElementsHelper = cacheStore
                .Select(p => p.Value)
                .Where(c =>
                {
                    var r = false;
                    try
                    {
                        r = predicate(c);
                    }
                    catch (Exception)
                    {

                    }

                    return r;

                })
                .Select(c => DalilaFS.CloneDCResource(c))
                .Where(o => o)
                .Select(o => o.Data);

            TCollection[] filesArray;
            if (limit > 0)
                filesArray = hotElementsHelper.Take(limit).ToArray();
            else
                filesArray = hotElementsHelper.ToArray();

            if (filesArray.Length > 0)
                return new DataOperation<TCollection[]>().Successful(filesArray, filesArray.Count() + "");
            else
                return new DataOperation<TCollection[]>().Fails(filesArray, new FileNotFoundException());

        }

        private static DataOperation<TCollection> CacheReadOne(Func<TCollection, bool> predicate)
        {
            // Try to get the value from the hot dictionary
            var hotElement = cacheStore
                .Select(p => p.Value)
                .Where(c =>
                {
                    var r = false;
                    try
                    {
                        r = predicate(c);
                    }
                    catch (Exception)
                    {

                    }

                    return r;

                })
                .FirstOrDefault();

            if (hotElement != null)
                return DalilaFS.CloneDCResource(hotElement);
            else
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

            return _taskQueue.StartNew(() =>
            {
                return CacheExist(_id);
            });
        }

        public static Task<string[]> ExistAsync()  // True if a document with the key exist
        {
            return _taskQueue.StartNew(() =>
            {
                return CacheGet();
            });
        }

        public static Task<int> CountAsync()  // Return the number of documents in the collection
        {
            return _taskQueue.StartNew(() =>
            {
                return CacheCount();
            });
        }


        #endregion EXIST

        #region SAVE


        public Task<DataOperation> SaveAsync()
        {
            return _taskQueue.StartNew(() =>
            {
                return CacheAdd(_id, (TCollection)this);  // Add to the hot dictionary
            });
        }

        public Task<DataOperation> SaveNewAsync()
        {
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

            return _taskQueue.StartNew(() =>
            {
                var opp = CacheReadById(id);

                if (opp)
                {
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
                throw new ArgumentNullException("Predicate cant be null");

            return _taskQueue.StartNew(() =>
            {

                return CacheReadMany(predicate, limit);

            });

        }



        public static Task<DataOperation<TCollection>> FindOneAsync(Func<TCollection, bool> predicate) => DoFindOneOrDefaultAsync(predicate, false, null);

        public static Task<DataOperation<TCollection>> FindOneOrDefaultAsync(Func<TCollection, bool> predicate, TCollection defaultDoc) => DoFindOneOrDefaultAsync(predicate, true, defaultDoc);

        private static Task<DataOperation<TCollection>> DoFindOneOrDefaultAsync(Func<TCollection, bool> predicate, bool useDefault, TCollection defaultDoc)
        {
            if (predicate == null)
                throw new ArgumentNullException("Predicate in Delete() cant be null");

            return _taskQueue.StartNew(() =>
            {
                var opp = CacheReadOne(predicate);

                if (opp)
                {
                    return opp;
                }
                else
                {
                    if (useDefault) return new DataOperation<TCollection>().Successful(defaultDoc, "0");
                    else return opp;
                }

            });
        }


        #endregion FINDONE

        #region UPDATE


        private Task<DataOperation> ReplaceAsync()
        {
            return _taskQueue.StartNew(() =>
            {
                if (CacheExist(_id))
                    return CacheAdd(_id, (TCollection)this);

                return new DataOperation().Fails(new FileNotFoundException());

            });
        }


        public static Task<DataOperation> UpdateAllAsync(Func<TCollection, TCollection> updating) => UpdateManyAsync(updating, d => true);

        public static Task<DataOperation> UpdateManyAsync(Func<TCollection, TCollection> updating, Func<TCollection, bool> predicate)
        {
            if (updating == null || predicate == null)
                throw new ArgumentNullException("updatingFunc or predicate cant be null");

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

            return _taskQueue.StartNew(() =>
            {
                // Find the files
                var findOperation = FindOne(predicate);
                var findOperation2 = FindOne(predicate);
                Debug.Log("FindOne: " + object.ReferenceEquals(findOperation.Data, findOperation2.Data));

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

                    if(!updateFails && n != null)
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

            return _taskQueue.StartNew(() =>
            {
                // Find the files
                var findOperation = FindById(id);
                TCollection n = null;

                if (findOperation)
                {
                    var updateFails = false;
                    try
                    {
                        n = updating(findOperation.Data);
                    }
                    catch (Exception)
                    {
                        updateFails = true;
                    }

                    if (!updateFails)
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
            return _taskQueue.StartNew(() =>
            {
                return CacheClear();
            });
        }

        public static Task<DataOperation> DeleteManyAsync(Func<TCollection, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("Predicate in Delete() cant be null");

            return _taskQueue.StartNew(() =>
            {

                // Find the files
                int deleted = 0;
                TCollection[] files = FindManyAsync(predicate).Result.Data;
                foreach (var file in files)
                {
                    var opp = CacheRemove(file._id);
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

            return _taskQueue.StartNew(() =>
            {
                // Find the files
                var findoperation = FindOneAsync(predicate).Result;

                if (!findoperation)
                    return new DataOperation().Successful("0");

                var opp = CacheRemove(findoperation.Data._id);
                return opp;

            });
        }

        public static Task<DataOperation> DeleteByIdAsync(SID id)
        {
            // Verificar que la key sea valida, nada de caracteres raros
            if (id == null)
                throw new ArgumentNullException("_id cant be null");

            return _taskQueue.StartNew(() =>
            {
                var opp = CacheRemove(id);

                return opp;
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
