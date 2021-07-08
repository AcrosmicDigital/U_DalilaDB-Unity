using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;

namespace U.DalilaDB
{
    [DataContract(Name = "SID", Namespace = "http://www.sacrum.com")]
    public sealed class SID
    {
        [DataMember()]
        private string id;


        public static char[] validCharacters = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', };

        public SID()
        {

            byte[] buffer = Guid.NewGuid().ToByteArray();

            var sid = BitConverter.ToUInt64(buffer, 0).ToString();

            if (sid.Length < 19)
            {
                for (int i = 0; i < 19 - sid.Length; i++)
                {
                    sid = sid + "0";
                }
            }

            if (sid.Length > 19)
            {
                for (int i = 0; i < sid.Length - 19; i++)
                {
                    sid = sid.TrimEnd(sid[sid.Length - 1]);
                }
            }

            id = sid;

        }

        public SID(string sid)
        {
            
            if (string.IsNullOrEmpty(sid))
                throw new ArgumentNullException("SID cant be created from null or empty string");

            if (sid.Length != 19 || sid.TrimEnd(validCharacters) != "")
                throw new FormatException("SID must be 19 numeric chars long");

            id = sid;

        }

        public static implicit operator string(SID sid)
        {
            return sid.id.ToString();
        }

        public static implicit operator SID(string sid)
        {
            return new SID(sid);
        }

        public override string ToString()
        {
            return id;
        }
         
        public override bool Equals(object obj)
        {
            if((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                SID sid = (SID)obj;
                return id == sid.id;
            }
        }

        public static bool operator==(SID obj1, SID obj2)
        {
            var equal = true;  // Are equal, because two are undefined
            try
            {
                equal = obj1.Equals(obj2);
            }
            catch (Exception)
            {
                try
                {
                    equal = obj2.Equals(obj1);
                }
                catch (Exception)
                {

                }
            }

            return equal;

        }

        public static bool operator !=(SID obj1, SID obj2)
        {
            return !(obj1 == obj2);
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

    }

}