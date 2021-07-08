using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Concurrent;
using System.IO;
using System.Data;

namespace U.DalilaDB
{
    public abstract class DalilaDBVolatileElements<TElements> where TElements : DalilaDBVolatileElements<TElements>, new()
    {

        #region Basics


        private static TElements instance_ = null;  // A instance of the child class to have the overwrited properties
        private static TElements _instance  // The instance getter
        {
            get
            {
                if (instance_ == null)
                    instance_ = new TElements();

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


        // Serializable container
        [DataContract()]
        private class Container<TValue>  // The container to store values
        {

            [DataMember()]
            public TValue value { get; private set; }

            public Container(TValue value)
            {
                this.value = value;
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


        private static ConcurrentDictionary<string, object> cacheStore = new ConcurrentDictionary<string, object>();  // Concurent dictionary for cache


        public static int CacheCount()
        {
            return cacheStore.Count();
        }

        public static bool CacheExist(string key)
        {
            return cacheStore.ContainsKey(key);

        }

        public static string[] CacheGet()
        {
            return cacheStore.Select(e => e.Key).ToArray();
        }


        #endregion StoreAndCount

        #region Functions


        public static DataOperation CacheClear()  // Clear the cache memory
        {
            var opp = new DataOperation().Successful(cacheStore.Count() + "");
            cacheStore.Clear();
            return opp;
        }

        private static DataOperation CacheAdd<TResource>(string key, TResource file) where TResource : class
        {
            // Try remove if exist
            var rmvOpp = CacheRemove(key);
            if (!rmvOpp)
                return rmvOpp;

            // If is null
            if (file == null || key == null)
                return new DataOperation().Fails(new ArgumentNullException());

            // If is full dont save
            if (cacheStore.Count >= CacheSize)
                return new DataOperation().Fails(new InvalidOperationException("Store is full with: " + CacheSize + " elements"));


            // Clone the object
            var cloneOpp = DalilaFS.CloneResource(file);

            if (!cloneOpp)
                return cloneOpp;

            // Add the object
            var added = cacheStore.TryAdd(key, cloneOpp.Data);

            if (added)
                return new DataOperation().Successful("1");
            else
                return new DataOperation().Fails(new InvalidDataException("Cant add save data"));

        }

        private static DataOperation CacheRemove(string key)
        {
            //Debug.Log("<Removing");
            if (key == null)
                return new DataOperation().Fails(new ArgumentNullException("Key cant be null"));

            if (!cacheStore.ContainsKey(key)) return new DataOperation().Successful("1");

            var rmv = cacheStore.TryRemove(key, out var ff); // Delete the hotKey if exist

            if (rmv)
                return new DataOperation().Successful("1");
            else
                return new DataOperation().Fails(new InvalidDataException("Cant remove data"));

        }

        private static DataOperation<TResource> CacheRead<TResource>(string key) where TResource : class
        {
            // If key is null
            if (key == null)
                return new DataOperation<TResource>().Fails(null, new FileNotFoundException());

            // Try get the file
            if (!cacheStore.TryGetValue(key, out var file))
                return new DataOperation<TResource>().Fails(null, new FileNotFoundException());

            //Debug.Log("Finded");
            try
            {
                var resource = (TResource)file;
                //Debug.Log("Casted");
                var cloneOpp = DalilaFS.CloneResource(resource);
                //Debug.Log("Cloned: " + cloneOpp);
                if (!cloneOpp)
                    throw cloneOpp.Error;

                //Debug.Log("Cloned");
                return cloneOpp;
            }
            catch (Exception e)
            {
                return new DataOperation<TResource>().Fails(null, e);
            }

            

        }


        #endregion Functions


        #endregion HotCache




        #region ASYNC


        public static Task<bool> ExistAsync(object key)  // True if a document with the keyStrexist
        {
            if (key == null)
                throw new ArgumentNullException("Key cant be null or empty");

            var keyStr = key.ToString();

            // Verificar que la keyStrsea valida, nada de caracteres raros
            if (String.IsNullOrEmpty(keyStr))
                throw new ArgumentNullException("Key cant be null or empty");
            if (keyStr.TrimEnd(DalilaFS.validCharacters).Length > 0)
                throw new FormatException("Invalid key, Only can contain numbers, letters and '_'");

            return _taskQueue.StartNew(() =>
            {
                return CacheExist(keyStr);
            });
        }

        public static Task<string[]> ExistAsync()  // True if a document with the keyStrexist
        {
            
            return _taskQueue.StartNew(() =>
            {
                return CacheGet();
            });
        }

        public static Task<int> CountAsync()  // Return the number of keys stored
        {
            
            return _taskQueue.StartNew(() =>
            {
                return CacheCount();
            });
        }




        public static Task<DataOperation> SaveAsync<TValue>(object key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("Key cant be null or empty");

            var keyStr = key.ToString();

            // Verificar que el valor no sea null
            if (value == null)
                throw new ArgumentNullException("Value cant be null");

            // Verificar que la key sea valida, nada de caracteres raros
            if (String.IsNullOrEmpty(keyStr))
                throw new ArgumentNullException("Key cant be null or empty");
            if (keyStr.TrimEnd(DalilaFS.validCharacters).Length > 0)
                throw new FormatException("Invalid key, Only can contain numbers, letters and '_'");

            return _taskQueue.StartNew(() =>
            {

                var opp = CacheAdd(keyStr, new Container<TValue>(value));

                return opp;
            });
        }

        public static Task<DataOperation<TValue>> FindAsync<TValue>(object key) => DoFindOrDefaultAsync<TValue>(key, useDefault: false, defaultValue: default(TValue));

        public static Task<DataOperation<TValue>> FindOrDefaultAsync<TValue>(object key, TValue defaultValue) => DoFindOrDefaultAsync<TValue>(key, useDefault: true, defaultValue);

        private static Task<DataOperation<TValue>> DoFindOrDefaultAsync<TValue>(object key, bool useDefault, TValue defaultValue)
        {
            if (key == null)
                throw new ArgumentNullException("Key cant be null or empty");

            var keyStr = key.ToString();

            // Verificar que la key sea valida, nada de caracteres raros
            if (String.IsNullOrEmpty(keyStr))
                throw new ArgumentNullException("Key cant be null or empty");
            if (keyStr.TrimEnd(DalilaFS.validCharacters).Length > 0)
                throw new FormatException("Invalid key, Only can contain numbers, letters and '_'");

            return _taskQueue.StartNew(() =>
            {
                // Try to get the value from the hot dictionary
                var opp = CacheRead<Container<TValue>>(keyStr);

                if (opp)
                    return new DataOperation<TValue>().Successful(opp.Data.value, "1");
                else
                {
                    if (useDefault) return new DataOperation<TValue>().Successful(defaultValue, "0");
                    else return new DataOperation<TValue>().Fails(defaultValue, opp.Error);
                }


            });
        }


        private static Task<DataOperation> ReplaceAsync<TValue>(object key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("Key cant be null or empty");

            var keyStr = key.ToString();

            // Verificar que el valor no sea null
            if (value == null)
                throw new ArgumentNullException("Value cant be null");

            // Verificar que la key sea valida, nada de caracteres raros
            if (String.IsNullOrEmpty(keyStr))
                throw new ArgumentNullException("Key cant be null or empty");
            if (keyStr.TrimEnd(DalilaFS.validCharacters).Length > 0)
                throw new FormatException("Invalid key, Only can contain numbers, letters and '_'");

            return _taskQueue.StartNew(() =>
            {
                if(CacheExist(keyStr))
                    return CacheAdd(keyStr, new Container<TValue>(value));

                return new DataOperation().Fails(new FileNotFoundException());
            });
        }

        public static Task<DataOperation> UpdateAsync<TValue>(object key, Func<TValue, TValue> updating)
        {
            if (key == null)
                throw new ArgumentNullException("Key cant be null or empty");

            var keyStr = key.ToString();

            if (updating == null)
                throw new ArgumentNullException("updatingFunc or predicate cant be null");

            // Verificar que la key sea valida, nada de caracteres raros
            if (String.IsNullOrEmpty(keyStr))
                throw new ArgumentNullException("Key cant be null or empty");
            if (keyStr.TrimEnd(DalilaFS.validCharacters).Length > 0)
                throw new FormatException("Invalid key, Only can contain numbers, letters and '_'");

            return _taskQueue.StartNew(() =>
            {
                var readOpp = Find<TValue>(key);

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
                    return ReplaceAsync(key, value).Result;
                }

                return new DataOperation().Fails(new EvaluateException("Error updating"));
            });
        }


        public static Task<DataOperation> DeleteAsync(object key)
        {
            if (key == null)
                throw new ArgumentNullException("Key cant be null or empty");

            var keyStr = key.ToString();

            // Verificar que la key sea valida, nada de caracteres raros
            if (String.IsNullOrEmpty(keyStr))
                throw new ArgumentNullException("Key cant be null or empty");
            if (keyStr.TrimEnd(DalilaFS.validCharacters).Length > 0)
                throw new FormatException("Invalid key, Only can contain numbers, letters and '_'");

            return _taskQueue.StartNew(() =>
            {
                
                return CacheRemove(keyStr);
            });
        }

        public static Task<DataOperation> DeleteAllAsync()
        {
            
            return _taskQueue.StartNew(() =>
            {
                return CacheClear();
            });

        }


        #endregion ASYNC

        #region SYNC


        public static bool Exist(object key) => ExistAsync(key).Result;
        public static string[] Exist() => ExistAsync().Result;
        public static int Count() => CountAsync().Result;


        public static DataOperation Save<TValue>(object key, TValue value) => SaveAsync<TValue>(key, value).Result;

        public static DataOperation<TValue> Find<TValue>(object key) => FindAsync<TValue>(key).Result;
        public static DataOperation<TValue> FindOrDefault<TValue>(object key, TValue defaultValue) => FindOrDefaultAsync<TValue>(key, defaultValue).Result;

        public static DataOperation Update<TValue>(object key, Func<TValue, TValue> updating) => UpdateAsync(key, updating).Result;

        public static DataOperation Delete(object key) => DeleteAsync(key).Result;
        public static DataOperation DeleteAll() => DeleteAllAsync().Result;


        #endregion SYNC

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
