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
    public abstract class DalilaDBVolatileDocument<TDocument> where TDocument : DalilaDBVolatileDocument<TDocument>, new()
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

        //...

        #endregion Basics

        #region HotCache


        #region StoreAndCount


        private bool cacheSize_ => true; // This are the hot items to use
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
            if (cacheStore != null) return true;
            else return false;
        }


        #endregion StoreAndCount

        #region Functions


        public static DataOperation CacheClear()  // Clear the cache memory
        {
            cacheStore = null;
            return new DataOperation().Successful("1");
        }

        private static DataOperation CacheAdd(TDocument document)
        {
            // Try remove if exist
            var rmvOpp = CacheRemove();
            if (!rmvOpp)
                return rmvOpp;

            if (document == null)
                return new DataOperation().Fails(new ArgumentNullException()); ;


            var cloneOpp = DalilaFS.CloneResource(document);

            if (!cloneOpp)
                return cloneOpp;

            // Add the object
            cacheStore = cloneOpp.Data;

            return new DataOperation().Successful("1");

        }

        private static DataOperation CacheRemove()
        {
            cacheStore = null;
            return new DataOperation().Successful("1");
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
            
            return _taskQueue.StartNew(() =>
            {
                return CacheExist();
            });
        }

        public Task<DataOperation> SaveAsync()
        {
            
            return _taskQueue.StartNew(() =>
            {
                return CacheAdd((TDocument)this);
            });
        }

        public static Task<DataOperation<TDocument>> FindAsync() => DoFindOrDefaultAsync(false, null);

        public static Task<DataOperation<TDocument>> FindOrDefaultAsync(TDocument defaultValue) => DoFindOrDefaultAsync(true, defaultValue);

        private static Task<DataOperation<TDocument>> DoFindOrDefaultAsync(bool useDefault, TDocument defaultValue) 
        {
            return _taskQueue.StartNew(() =>
            {
                // Read
                var opp = CacheRead();

                if (opp)
                {
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
            return _taskQueue.StartNew(() =>
            {
                if(Exist())
                    return CacheAdd((TDocument)this);

                return new DataOperation().Fails(new FileNotFoundException());

            });
        }

        public static Task<DataOperation> UpdateAsync(Func<TDocument, TDocument> updating)
        {
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
            
            return _taskQueue.StartNew(() =>
            {
                return CacheRemove();
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

            Debug.Log(" - InCache: " + Exist());

            Debug.Log("<---- ");
        }

        #endregion

    }
}
