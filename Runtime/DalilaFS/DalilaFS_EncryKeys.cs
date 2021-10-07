using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace U.DalilaDB
{
    public partial class DalilaFS
    {

        protected static string aesDefaultRandomKeyResourceName = "/DalilaFSAes.key";
        protected static string aesDefaultFixedKey = "KeyIsNoKey";
        protected static aesValidKeySizes aesDefaultKeySize = aesValidKeySizes.aes128;
        protected static byte[] aesDefaultRandomKey = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        protected static byte[] aes128DefaultFullKey = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        protected static byte[] aes256DefaultFullKey = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

        public enum aesValidKeySizes
        {
           aes128 = 16, // 16byte x 8bit = 128
           aes256 = 32, // 32byte x 8bit = 256
        }

        public bool _aesEncryption { get; set; } = false;


        public aesValidKeySizes aesKeySize_ = aesDefaultKeySize;
        public aesValidKeySizes _aesKeySize { 
            get 
            {
                return aesKeySize_;
            } 
            set 
            {
                // If aesKeySize, full key must be null
                aesKey_ = null;

                aesKeySize_ = value;
            } 
        }


        protected string aesRandomKeyResourceName_ = null;
        public string _aesRandomKeyResourceName
        {
            get
            {
                if (aesRandomKeyResourceName_ == null)
                    aesRandomKeyResourceName_ = aesDefaultRandomKeyResourceName;

                return aesRandomKeyResourceName_;
            }
            set
            {
                if (IsValidResource(value))
                {

                    // If path change, full key must be null and random jey too
                    aesRandomKey_ = null;
                    aesKey_ = null;

                    aesRandomKeyResourceName_ = value;
                }
                else
                    Debug.LogError("DalilaFS: Invalid Resource, Only can contain numbers, letters and '_', Must start with '/', Can contain '.', but cant end with it or with '/', Cant have more than one '.' or '/' together ");
            }
        }


        protected string aesFixedKey_ = null;
        public string _aesFixedKey
        {
            get
            {
                if (aesFixedKey_ == null)
                    aesFixedKey_ = aesDefaultFixedKey;


                return aesFixedKey_;
            }
            set
            {
                
                if (!String.IsNullOrEmpty(value) && !String.IsNullOrWhiteSpace(value))
                {
                    // If fixed key change, full key must be deleted, so can be recalculated
                    aesKey_ = null;

                    aesFixedKey_ = value;
                }
                else
                    Debug.LogError("DalilaFS: Invalid Fixed key");
            }
        }


        protected byte[] aesRandomKey_ = null;
        public byte[] _aesRandomKey
        {
            get
            {
                if (aesRandomKey_ == null)
                {

                    // If random key is null, full key must be null too
                    aesKey_ = null;

                    // Try to create the path of the key file
                    string path = null; // full path
                    try
                    {
                        path = ResourceToSystemPath(_aesRandomKeyResourceName);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("DalilaFS: Error in path of random aesKey file, using default key: " + e);
                        return aesDefaultRandomKey;
                    }

                    // Check if a key is stored or store a new one
                    if (!ExistResource(_aesRandomKeyResourceName))
                    {
                        try
                        {
                            using (Aes aesAlg = Aes.Create())
                            {
                                // Create path if dont exist
                                CreateLocation(GetResourceLocation(_aesRandomKeyResourceName));

                                // Write the file
                                File.WriteAllBytes(path, aesAlg.Key);
                                Debug.Log("DalilaFS: Saving random key in : " + path);
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("DalilaFS: Error while creating random aesKey, using default key: " + e);
                            return aesDefaultRandomKey;
                        }
                    }

                    // Try to read the key
                    try
                    {
                        var readedKey = File.ReadAllBytes(ResourceToSystemPath(_aesRandomKeyResourceName));
                        //Debug.Log("DalilaFS: Reading random key from : " + path);
                        aesRandomKey_ = readedKey;
                        return aesRandomKey_;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("DalilaFS: Error while reading random aesKey: " + e);
                        return aesDefaultRandomKey;
                    }

                }

                // If is not null just return
                return aesRandomKey_;

            }
        }


        protected byte[] aesKey_ = null;
        public byte[] _aesKey 
        {
            get
            {

                if(aesKey_ == null)
                {
                    try
                    {
                        byte[] fullKey = new byte[(int)_aesKeySize];

                        using (SHA256 mySHA256 = SHA256.Create())
                        {

                            byte[] hashValue = mySHA256.ComputeHash(Encoding.ASCII.GetBytes("FK" + _aesFixedKey + "RK").Concat<byte>(_aesRandomKey).ToArray());

                            for (int i = 0; i < fullKey.Length; i++)
                            {
                                fullKey[i] = hashValue[i];
                            }

                        }

                        aesKey_ = fullKey;

                    }
                    catch (Exception e)
                    {
                        Debug.LogError("DalilaFS: Error while computing aesFullKey, using default key: " + e);

                        if (_aesKeySize == aesValidKeySizes.aes128)
                            return aes128DefaultFullKey;
                        if (_aesKeySize == aesValidKeySizes.aes256)
                            return aes256DefaultFullKey;
                    }
                }

                // If is already set, dont create a new one
                return aesKey_;

            } 
        }

    }
}
