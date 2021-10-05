using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Xml;
using NUnit.Framework;
using U.DalilaDB;
using UnityEngine;
using UnityEngine.TestTools;

namespace DalilaFsTests
{
    public class DalilaFS_DCSerialization_X1CreateAndRead
    {
        // The filesystem in a path
        string rootPath = Application.persistentDataPath + "/DalilaFS_DCSerialization_X1CreateAndRead";
        DalilaFS fs;


        [SetUp]
        public void SetUp()
        {
            // Reset the directory to be empty
            if (Directory.Exists(rootPath))
                Directory.Delete(rootPath, true);
            Directory.CreateDirectory(rootPath);
            fs = new DalilaFS(rootPath);
        }



        [Test]
        public void DalilaFS_DCSerialization_X1CreateAndReadSimplePasses()
        {

            // Create a new instance of a dc serializable class and store the name in a variable
            var file = new SampleDCSerializableClass
            {
                id = 2233,
                name = "Andrew",
                age = 23,
            };
            var fileName = "/Sample.xml";

            // Save the class
            var saveOpp = fs.CreateDCResource(fileName, file);
            Debug.Log(saveOpp);
            Assert.IsTrue(saveOpp);

            // Read the class saved
            var readOpp = fs.ReadDCResource<SampleDCSerializableClass>(fileName);
            Debug.Log(readOpp);

            // Compare the values of the two classes
            Assert.AreEqual(file.id, readOpp.Data.id);
            Assert.AreEqual(file.name, readOpp.Data.name);
            Assert.AreEqual(file.age, readOpp.Data.age);

        }


        [Test]
        public void DalilaFS_DCSerialization_X2SaveSimplePasses()
        {

            // Create a new instance of a dc serializable class and store the name in a variable
            var file = new SampleDCSerializableClass
            {
                id = 2233,
                name = "Andrew",
                age = 23,
            };
            var fileName = "/Sample.xml";

            // Create a new instance of a dc serializable class
            var file2 = new SampleDCSerializableClass
            {
                id = 344,
                name = "Andrea",
                age = 24,
            };

            // Save the class
            var saveOpp = fs.SaveDCResource(fileName, file);
            Debug.Log(saveOpp);
            Assert.IsTrue(saveOpp);

            // Try to save other class with the same name
            var saveOpp2 = fs.SaveDCResource(fileName, file2);
            Debug.Log(saveOpp2);
            Assert.IsFalse(saveOpp2);

            // Read the class saved
            var readOpp = fs.ReadDCResource<SampleDCSerializableClass>(fileName);
            Debug.Log(readOpp);

            // Compare the values of the two classes, must be the first save
            Assert.AreEqual(file.id, readOpp.Data.id);
            Assert.AreEqual(file.name, readOpp.Data.name);
            Assert.AreEqual(file.age, readOpp.Data.age);

        }




        [Test]
        public void DalilaFS_DCSerialization_Y1CreateEncryAndReadSimplePasses()
        {

            // Create a new instance of a dc serializable class and store the name in a variable
            var file = new SampleDCSerializableClass
            {
                id = 2233,
                name = "Andrew",
                age = 23,
            };
            var fileName = "/Sample.xml";

            byte[] savedKey = { 0x16, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x15 };

            using (Aes myAes = Aes.Create())
            {

                // Save the class
                var saveOpp = fs.CreateEncryDCResource(fileName, file, savedKey);
                Debug.Log(saveOpp);
                Assert.IsTrue(saveOpp);
                //Assert.IsTrue(fs.ExistResource("/Sample.xml"));

                //// Read the class saved
                //var readOpp = fs.ReadEncryDCResource<SampleDCSerializableClass>(fileName, myAes.Key, myAes.IV);
                //Debug.Log(readOpp);

                ////Display the original data and the decrypted data.
                //Debug.Log("Original  : " + file.id + " " + file.name + " " + file.age);
                //Debug.Log("Readed    : " + readOpp.Data.id + " " + readOpp.Data.name + " " + readOpp.Data.age);

                //// Compare the values of the two classes
                //Assert.AreEqual(file.id, readOpp.Data.id);
                //Assert.AreEqual(file.name, readOpp.Data.name);
                //Assert.AreEqual(file.age, readOpp.Data.age);
                ////Assert.IsTrue(fs.ExistResource("/Sample.xml"));

            }

            using (Aes myAes = Aes.Create())
            {

               
                // Read the class saved
                var readOpp = fs.ReadEncryDCResource<SampleDCSerializableClass>(fileName, savedKey);
                Debug.Log(readOpp);

                //Display the original data and the decrypted data.
                Debug.Log("Original  : " + file.id + " " + file.name + " " + file.age);
                Debug.Log("Readed    : " + readOpp.Data.id + " " + readOpp.Data.name + " " + readOpp.Data.age);

                // Compare the values of the two classes
                Assert.AreEqual(file.id, readOpp.Data.id);
                Assert.AreEqual(file.name, readOpp.Data.name);
                Assert.AreEqual(file.age, readOpp.Data.age);
                //Assert.IsTrue(fs.ExistResource("/Sample.xml"));

            }




        }





        [Test]
        public void DalilaFS_DCSerialization_Z1CloneSimplePasses()
        {

            // Create a new instance of a dc serializable class
            var file = new SampleDCSerializableClass
            {
                id = 2233,
                name = "Andrew",
                age = 23,
            };

            // Clone the class without save
            var cloneOpp = DalilaFS.CloneDCResource(file);
            Debug.Log(cloneOpp);

            // Compare the values of the two classes
            Assert.AreEqual(file.id, cloneOpp.Data.id);
            Assert.AreEqual(file.name, cloneOpp.Data.name);
            Assert.AreEqual(file.age, cloneOpp.Data.age);

            // Objects must be diferent instances of the same class
            System.Object.ReferenceEquals(file, cloneOpp.Data);

        }

    }
}
