using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using System.Runtime.Serialization;
using System.IO;
using System.Data;

namespace U.DalilaDB
{
    [Serializable]
    public abstract class DalilaDBDocument<TDocument> where TDocument : DalilaDBDocument<TDocument>, new()
    {

        #region Basics


        private static TDocument instance_ = null;  // A instance of the child class to have the overwrited properties
        private static TDocument _instance
        {
            get
            {
                if (instance_ == null)
                {
                    instance_ = new TDocument();
                }

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


        protected abstract string rootPath_ { get; }  // This can be overritd to change the path

        public static string RootPath => _fileSystem._root;  // Directory where DalilaDB is stored
        public static string Location => "/DalilaDB/DalilaDocuments/" + _instance.GetType().Name + "/"; // Relative to root
        public static string LocationPath => RootPath.TrimEnd('/') + Location;  // Get the full path of the resource
        public static string ResourceLocation => Location + _instance.GetType().Name + ".xml";  // Get the name of a resource referenced from the Root directory
        public static string ResourcePath => RootPath.TrimEnd('/') + ResourceLocation;  // Get the full path of s specific resource


        #endregion Basics

        #region SerializableItems

        //...

        #endregion Basics

        #region HotCache


        #region StoreAndCount


        protected virtual bool cacheSize_ => true; // This are the hot items to use
        public static bool CacheSize   // hotElementsMax_ getter
        {
            get
            {
                return _instance.cacheSize_;
            }
        }


        private volatile static TDocument cacheStore = null;  // Volatile Instance for cache


        public static bool CacheExist()
        {
            return cacheStore == null;
        }


        #endregion StoreAndCount

        #region Functions


        public static void CacheClear()  // Clear the cache memory
        {
            cacheStore = null;
        }

        private static void CacheAdd(TDocument document)
        {
            // Try remove if exist
            CacheRemove();

            if (document == null)
                return;

            if (!CacheSize)
                return;

            var cloneOpp = DalilaFS.CloneResource(document);

            if (!cloneOpp)
                return;

            cacheStore = cloneOpp.Data;

        }

        private static void CacheRemove()
        {
            CacheClear();
        }

        private static DataOperation<TDocument> CacheRead()
        {
            if (cacheStore != null)
                return DalilaFS.CloneResource(cacheStore);
            else
                return new DataOperation<TDocument>().Fails(null, new FileNotFoundException());
        }


        #endregion Functions


        #endregion HotCache




        #region ASYNC


        public static Task<bool> ExistAsync()  // True if the document exist (is saved)
        {
            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {
                return _fileSystem.ExistResource(ResourceLocation).IsSuccessful;
            });
        }

        public Task<DataOperation> SaveAsync()
        {
            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {
                
                var opp = _fileSystem.CreateResource(ResourceLocation, (TDocument)this);

                // Try to save the hotDocument
                if(opp) CacheAdd((TDocument)this);

                return opp;
            });
        }

        public static Task<DataOperation<TDocument>> FindAsync() => DoFindOrDefaultAsync(false, null);

        public static Task<DataOperation<TDocument>> FindOrDefaultAsync(TDocument defaultValue) => DoFindOrDefaultAsync(true, defaultValue);

        private static Task<DataOperation<TDocument>> DoFindOrDefaultAsync(bool useDefault, TDocument defaultValue) 
        {
            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {
                // Read from Hot document
                var hotOpp = CacheRead();
                if (hotOpp)
                    return hotOpp;

                // Read from file
                var opp = _fileSystem.ReadOrDeleteResource<TDocument>(ResourceLocation);

                if (opp)
                {
                    CacheAdd(opp.Data);
                    return opp;
                }
                else
                {
                    if (useDefault) return new DataOperation<TDocument>().Successful(defaultValue, "0");
                    else return opp;
                }
                    
            });
        }

        private Task<DataOperation> ReplaceAsync()
        {
            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {

                var opp = _fileSystem.ReplaceResource(ResourceLocation, (TDocument)this);

                // Try to save the hotDocument
                if (opp) CacheAdd((TDocument)this);

                return opp;
            });
        }

        public static Task<DataOperation> UpdateAsync(Func<TDocument, TDocument> updating)
        {
            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            if (updating == null)
                throw new ArgumentNullException("updatingFunc or predicate cant be null");

            return _taskQueue.StartNew(() =>
            {
                var readOpp = Find();

                // If cant read return
                if (!readOpp) return readOpp;

                var value = readOpp.Data;

                var updateFails = false;
                try
                {
                    value = updating(value);
                }
                catch (Exception)
                {
                    updateFails = true;
                }
                if (!updateFails)
                {
                    return value.ReplaceAsync().Result;
                }

                return new DataOperation().Fails(new EvaluateException("Error updating Function"));
            });
        }

        public static Task<DataOperation> DeleteAsync()
        {
            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {
                CacheRemove();

                return _fileSystem.DeleteResource(ResourceLocation);
            });
        }


        #endregion ASYNC

        #region SYNC


        public static bool Exist() => ExistAsync().Result;


        public DataOperation Save() => SaveAsync().Result;
        public static DataOperation<TDocument> Find() => FindAsync().Result;
        public static DataOperation<TDocument> FindOrDefault(TDocument defaultDoc) => FindOrDefaultAsync(defaultDoc).Result;
        public static DataOperation Update(Func<TDocument, TDocument> updating) => UpdateAsync(updating).Result;
        public static DataOperation Delete() => DeleteAsync().Result;


        #endregion

        #region DEBUG

        public static void Define()
        {
            Debug.Log("----> ");

            Debug.Log(" - Name: " + _instance.GetType().Name);
            Debug.Log(" - Type: DalilaDBDocument");

            Debug.Log(" - Stored: " + Exist());

            Debug.Log(" - InCache: " + Exist());

            Debug.Log("<---- ");
        }

        #endregion

    }
}
