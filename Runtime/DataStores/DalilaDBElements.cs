
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
    public abstract class DalilaDBElements<TElements> where TElements : DalilaDBElements<TElements>, new()
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


        private static DalilaFS fileSystem_;  // The fileSystem to perform files operations
        private static DalilaFS _fileSystem   // The FileSystem getter
        {
            get
            {
                if (fileSystem_ == null)
                {
                    fileSystem_ = new DalilaFS(_instance.rootPath_);

                    // Set the encryption if enabled
                    if (_instance._aesEncryption)
                    {
                        fileSystem_._aesEncryption = _instance._aesEncryption;
                        fileSystem_._aesFixedKey = _instance._aesFixedKey;
                        fileSystem_._aesKeySize = _instance._aesKeySize;
                        fileSystem_._aesRandomKeyResourceName = _instance._aesRandomKeyResourceName;
                    }
                }

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


        protected abstract string rootPath_ { get; }  // This can be changed to change the path

        public static string RootPath => _fileSystem._root;  // Directory where DalilaD is stored
        public static string Location => "/DalilaDB/DalilaElements/" + _instance.GetType().Name + "/"; // Relative to root
        public static string LocationPath => RootPath.TrimEnd('/') + Location;  // Get the path where the resources are st
        public static string ResourceLocation(string key) => Location + key + ".xml";   // Dont check if exist Get the name of a resource referenced from the Root directory
        public static string ResourcePath(string key) => RootPath.TrimEnd('/') + ResourceLocation(key);  // Get the full path of s specific resource


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


        protected virtual int cacheSize_ => 20; // This are the hot items to use, is function tocan be virtual
        public static int CacheSize   // hotElementsMax_ getter
        {
            get
            {
                return _instance.cacheSize_.MinInt(0);
            }
        }


        private static ConcurrentDictionary<string, object> cacheStore = new ConcurrentDictionary<string, object>();  // Concurent dictionary for cache


        public static int CacheCount()
        {
            return cacheStore.Count();
        }

        public static string[] CacheGet()
        {
            return cacheStore.Select(e => e.Key.ToString()).Select(k => DalilaFS.GetOnlyResourceName(k, ".xml")).Where(k => k != "").ToArray();
        }


        #endregion StoreAndCount

        #region Functions


        public static void CacheClear()  // Clear the cache memory
        {
            cacheStore.Clear();
        }

        private static void CacheAdd<TResource>(string key, TResource file) where TResource : class
        {
            // Try remove if exist
            CacheRemove(key);

            if (file == null || key == null)
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

            var cloneOpp = DalilaFS.CloneDCResource(file);

            if (!cloneOpp)
                return;

            cacheStore.TryAdd(key, cloneOpp.Data);
        }

        private static void CacheRemove(string key)
        {
            //Debug.Log("<Removing");
            if (key == null)
                return;

            cacheStore.TryRemove(key, out var ff); // Delete the hotKey if exist
            //Debug.Log(">Removing");
        }

        private static DataOperation<TResource> CacheRead<TResource>(string key) where TResource : class
        {
            //Debug.Log("<Reading");
            if (key == null)
                return new DataOperation<TResource>().Fails(null, new FileNotFoundException());

            if (!cacheStore.TryGetValue(key, out var file))
                return new DataOperation<TResource>().Fails(null, new FileNotFoundException());

            //Debug.Log("Finded");
            try
            {
                var resource = (TResource)file;
                //Debug.Log("Casted");
                var cloneOpp = DalilaFS.CloneDCResource(resource);
                //Debug.Log("Cloned: " + cloneOpp);
                if (!cloneOpp)
                    throw cloneOpp.Error;

                //Debug.Log("Cloned");
                return cloneOpp;
            }
            catch (Exception)
            {
                
            }

            return new DataOperation<TResource>().Fails(null, new FileNotFoundException());

        }


        #endregion Functions


        #endregion HotCache

        #region Encryption


        protected virtual bool _aesEncryption => false; // If encryption will be enabled or disabled
        protected virtual DalilaFS.aesValidKeySizes _aesKeySize => DalilaFS.aesValidKeySizes.aes128; // Key size
        private string _aesRandomKeyResourceName => "/DalilaDB/DalilaElements/Keys/" + _instance.GetType().Name + "Aes.key"; // Name of the key
        protected virtual string _aesFixedKey => _instance.GetType().Name + "_KeyIsNoKey"; // Fixed key

        public static void ResetFileSystem()
        {
            fileSystem_ = null;
        }


        #endregion AesEncryption




        #region ASYNC


        public static Task<bool> ExistAsync(object key)  // True if a document with the keyStrexist
        {
            if(key == null)
                throw new ArgumentNullException("Key cant be null or empty");

            var keyStr= key.ToString();

            // Verificar que la keyStrsea valida, nada de caracteres raros
            if (String.IsNullOrEmpty(keyStr))
                throw new ArgumentNullException("Key cant be null or empty");
            if (keyStr.TrimEnd(DalilaFS.validCharacters).Length > 0)
                throw new FormatException("Invalid key, Only can contain numbers, letters and '_'");

            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {
                return _fileSystem.ExistResource(ResourceLocation(keyStr)).IsSuccessful;
            });
        }

        public static Task<string[]> ExistAsync()  // True if a document with the keyStrexist
        {
            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            return _taskQueue.StartNew(() =>
            {
                return _fileSystem.GetResources(Location).Select(k => DalilaFS.GetOnlyResourceName(k, ".xml")).Where(k => k != "").ToArray();
            });
        }

        public static Task<int> CountAsync()  // Return the number of keys stored
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




        public static Task<DataOperation> SaveAsync<TValue>(object key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("Key cant be null or empty");

            var keyStr = key.ToString();

            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

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
                
                var opp = _fileSystem.CreateDCResource(ResourceLocation(keyStr), new Container<TValue>(value));

                // Add to the hot dictionary
                if(opp) CacheAdd(ResourceLocation(keyStr), new Container<TValue>(value));
                else CacheRemove(ResourceLocation(keyStr));

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

            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            // Verificar que la key sea valida, nada de caracteres raros
            if (String.IsNullOrEmpty(keyStr))
                throw new ArgumentNullException("Key cant be null or empty");
            if (keyStr.TrimEnd(DalilaFS.validCharacters).Length > 0)
                throw new FormatException("Invalid key, Only can contain numbers, letters and '_'");

            return _taskQueue.StartNew(() =>
            {
                // Try to get the value from the hot dictionary
                var opp = CacheRead<Container<TValue>>(ResourceLocation(keyStr));

                //if (opp) Debug.Log("Readed from cache ");
                //else Debug.Log("Readed from file ");

                // Read the file
                if (!opp)
                    opp = _fileSystem.ReadDCResource<Container<TValue>>(ResourceLocation(keyStr));

                //Debug.Log("Readed from file: " + opp);

                // Add to the hot dictionary
                if (opp) CacheAdd(ResourceLocation(keyStr), opp.Data);

                if (opp)
                    return new DataOperation<TValue>().Successful(opp.Data.value, "1");
                else
                {
                    if(useDefault) return new DataOperation<TValue>().Successful(defaultValue, "0");
                    else return new DataOperation<TValue>().Fails(defaultValue, opp.Error);
                }
                    

            });
        }


        private static Task<DataOperation> ReplaceAsync<TValue>(object key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("Key cant be null or empty");

            var keyStr = key.ToString();

            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

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

                var opp = _fileSystem.ReplaceDCResource(ResourceLocation(keyStr), new Container<TValue>(value));

                // Add to the hot dictionary
                if (opp) CacheAdd(ResourceLocation(keyStr), new Container<TValue>(value));
                else CacheRemove(ResourceLocation(keyStr));

                return opp;
            });
        }

        public static Task<DataOperation> UpdateAsync<TValue>(object key, Func<TValue, TValue> updating)
        {
            if (key == null)
                throw new ArgumentNullException("Key cant be null or empty");

            var keyStr = key.ToString();

            if (updating == null)
                throw new ArgumentNullException("updatingFunc or predicate cant be null");

            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            // Verificar que la key sea valida, nada de caracteres raros
            if (String.IsNullOrEmpty(keyStr))
                throw new ArgumentNullException("Key cant be null or empty");
            if (keyStr.TrimEnd(DalilaFS.validCharacters).Length > 0)
                throw new FormatException("Invalid key, Only can contain numbers, letters and '_'");

            return _taskQueue.StartNew(() =>
            {
                var readOpp = Find<TValue>(key);

                // If cant read return
                if(!readOpp) return readOpp;

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

            if (_fileSystem == null)
                throw new MissingMemberException("FileSystem is null!!");

            // Verificar que la key sea valida, nada de caracteres raros
            if (String.IsNullOrEmpty(keyStr))
                throw new ArgumentNullException("Key cant be null or empty");
            if (keyStr.TrimEnd(DalilaFS.validCharacters).Length > 0)
                throw new FormatException("Invalid key, Only can contain numbers, letters and '_'");

            return _taskQueue.StartNew(() =>
            {
                // Delete the hotKey if exist
                CacheRemove(ResourceLocation(keyStr));

                return _fileSystem.DeleteResource(ResourceLocation(keyStr));
            });
        }

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
