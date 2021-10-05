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

        protected string aesRandomKeyResourceName = "/DalilaFSAes.key";
        protected string aesDefaultFixedKey_ = "KeyIsNoKey";
        protected byte[] aesDefaultRandomKey_ = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

        public enum aesValidKeySizes
        {
           aes128 = 16, // 16byte x 8bit = 128
           aes256 = 32, // 32byte x 8bit = 256
        }

        public aesValidKeySizes aesKeySize { get; set; } = aesValidKeySizes.aes128;


        protected string aesFixedKey_ = null;
        public string _aesFixedKey
        {
            get
            {

                if (_aesFixedKey == null)
                    aesFixedKey_ = aesDefaultFixedKey_;

                return aesFixedKey_;
            }
            set
            {
                if (!String.IsNullOrEmpty(value) && !String.IsNullOrWhiteSpace(value))
                    aesFixedKey_ = value;
            }
        }


        protected byte[] aesRandomKey_ = null;
        protected byte[] _aesRandomKey
        {
            get
            {
                if (aesRandomKey_ == null)
                {
                    // Try to create the path of the key file
                    string path = null; // full path
                    try
                    {
                        path = ResourceToSystemPath(aesRandomKeyResourceName);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("DalilaFS: Error in path of random aesKey file: " + e);
                        aesRandomKey_ = aesDefaultRandomKey_;
                    }

                    // Check if a key is stored
                    if (!ExistResource(aesRandomKeyResourceName))
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
                    if (ExistResource(aesRandomKeyResourceName))
                    {
                        try
                        {
                            aesRandomKey_ = File.ReadAllBytes(ResourceToSystemPath(aesRandomKeyResourceName));
                            return aesRandomKey_;
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("DalilaFS: Error while reading random aesKey: " + e);
                        }
                    }

                    // If the file cant be created or readed
                    Debug.LogError("DalilaFS: Error creating or reading random aesKey, using default key");
                    aesRandomKey_ = aesDefaultRandomKey_;

                }

                return aesRandomKey_;

            }
        }


        protected byte[] aesKey_ = null;
        protected byte[] _aesKey 
        {
            get
            {

                if(aesKey_ == null)
                {
                    try
                    {
                        byte[] fullKey = new byte[(int)aesKeySize];

                        using (SHA256 mySHA256 = SHA256.Create())
                        {

                            byte[] hashValue = mySHA256.ComputeHash(Encoding.ASCII.GetBytes("FK" + aesFixedKey_ + "RK").Concat<byte>(_aesRandomKey).ToArray());

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
