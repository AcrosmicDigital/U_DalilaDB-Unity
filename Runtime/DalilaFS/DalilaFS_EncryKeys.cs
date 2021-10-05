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

        public enum aesValidKeySizes
        {
           aes128 = 16, // 16byte x 8bit = 128
           aes256 = 32, // 32byte x 8bit = 256
        }

        public bool _aesEncryption { get; set; } = false;
        public aesValidKeySizes _aesKeySize { get; set; } = aesDefaultKeySize;


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
                {
                    // If fixed key is null, full key must be null too
                    aesKey_ = null;
                    aesFixedKey_ = aesDefaultFixedKey;
                }

                //Debug.Log("Readfixed: " + aesFixedKey_);

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
                        Debug.LogError("DalilaFS: Error in path of random aesKey file: " + e);
                        aesRandomKey_ = aesDefaultRandomKey;
                    }

                    // Check if a key is stored
                    if (!ExistResource(_aesRandomKeyResourceName))
                    {
                        try
                        {
                            using (Aes aesAlg = Aes.Create())
                            {
                                File.WriteAllBytes(path, aesAlg.Key);
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("DalilaFS: Error while creating random aesKey: " + e);
                        }
                    }

                    // Try to read the key
                    if (ExistResource(_aesRandomKeyResourceName))
                    {
                        try
                        {
                            aesRandomKey_ = File.ReadAllBytes(ResourceToSystemPath(_aesRandomKeyResourceName));
                            return aesRandomKey_;
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("DalilaFS: Error while reading random aesKey: " + e);
                        }
                    }

                    // If the file cant be created or readed
                    Debug.LogError("DalilaFS: Error creating or reading random aesKey, using default key");
                    aesRandomKey_ = aesDefaultRandomKey;

                }

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
                        Debug.LogError("DalilaFS: Error while computing aesKey: " + e);
                        aesKey_ = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
                    }
                }

                return aesKey_;

            } 
        }
    }
}
