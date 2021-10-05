using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DalilaFsTests
{

    [DataContract(Name = "ExampleSerializableClass", Namespace = "http://www.sacrum.com")]
    public class SampleDCSerializableClass
    {

        [DataMember()]
        public int id;
        [DataMember()]
        public string name { get; set; } 
        [DataMember()]
        public float time;
        [DataMember()]
        public int age;
        [DataMember()]
        public int[] numbers;
        [DataMember()]
        public List<string> adresses;
        [DataMember()]
        public Vector3 position;
        [DataMember()]
        public Quaternion rotation;

    }


}
