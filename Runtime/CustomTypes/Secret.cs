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
    [DataContract(Name = "Secret")]
    public struct Secret
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

        #region Comparator operators

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            Secret s = (Secret)obj;

            return hashValue.SequenceEqual(s.hashValue);

        }

        public static bool operator ==(Secret obj1, Secret obj2)
        {
            var equals = true;

            try
            {
                equals = obj1.Equals(obj2);
            }
            catch (System.Exception)
            {
                try
                {
                    equals = obj2.Equals(obj1);
                }
                catch (System.Exception)
                {

                }
            }

            return equals;
        }

        public static bool operator !=(Secret obj1, Secret obj2)
        {
            return !(obj1 == obj2);
        }

        #endregion Comparator operators



        #region Cast operators

        public static implicit operator string(Secret s)
        {
            if (s == null) return "";

            return Encoding.UTF8.GetString(s.hashValue);
        }

        public override string ToString()
        {
            return this + "";
        }

        public static implicit operator Secret(string plainText)
        {
            return new Secret(plainText);
        }

        #endregion Cast operators


        public override int GetHashCode()
        {
            return hashValue.GetHashCode();
        }


    }
}
