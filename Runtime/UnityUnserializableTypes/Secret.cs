using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace U.DalilaDB
{
    [DataContract()]
    public class Secret
    {

        #region Properties - This values will be serialized

        [DataMember()]
        private byte[] hashValue;

        #endregion Properties


        public Secret(string plainText)
        {
            using (SHA256 mySHA256 = SHA256.Create())
            {
                hashValue = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(plainText));
            }
        }

        public bool Compare(string plainText)
        {

            try
            {
                byte[] secondHashValue;

                using (SHA256 mySHA256 = SHA256.Create())
                {
                    secondHashValue = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(plainText));
                }

                return hashValue.SequenceEqual(secondHashValue);
            }
            catch (Exception e)
            {
                Debug.Log("Secret: Error while comparing secrets, " + e);
                return false;
            }

            

        }

    }
}
