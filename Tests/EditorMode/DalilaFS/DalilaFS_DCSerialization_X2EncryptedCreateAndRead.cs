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
    public class DalilaFS_DCSerialization_X2EncryptedCreateAndRead
    {
        // The filesystem in a path
        string rootPath = Application.persistentDataPath + "/DalilaFS_DCSerialization_X2EncryptedCreateAndRead";
        DalilaFS fs;


        [SetUp]
        public void SetUp()
        {
            // Reset the directory to be empty
            if (Directory.Exists(rootPath))
                Directory.Delete(rootPath, true);
            Directory.CreateDirectory(rootPath);
            fs = new DalilaFS(rootPath);

            // Enable the emcryption
            fs._aesEncryption = true;

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
        public void DalilaFS_DCSerialization_X2ChangeKeyLenght()
        {

            // Create a new instance of a dc serializable class and store the name in a variable
            var file = new SampleDCSerializableClass
            {
                id = 2233,
                name = "Andrew",
                age = 23,
            };
            var fileName = "/Sample.xml";

            // Change key lenght
            fs._aesKeySize = DalilaFS.aesValidKeySizes.aes256;

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
        public void DalilaFS_DCSerialization_X3SaveSimplePasses()
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
        public void DalilaFS_DCSerialization_X4ChangeFixedKeySimplePasses()
        {

            // Create a new instance of a dc serializable class and store the name in a variable
            var file = new SampleDCSerializableClass
            {
                id = 2233,
                name = "Andrew",
                age = 23,
            };
            var fileName = "/Sample.xml";

            // Change the fixed key
            fs._aesFixedKey = "New key to serialize";

            // Save the class
            var saveOpp = fs.CreateDCResource(fileName, file);
            Debug.Log(saveOpp);
            Assert.IsTrue(saveOpp);

            // Read the class saved
            var readOpp = fs.ReadDCResource<SampleDCSerializableClass>(fileName);
            Debug.Log(readOpp);
            Assert.IsTrue(readOpp);

            // Compare the values of the two classes
            Assert.AreEqual(file.id, readOpp.Data.id);
            Assert.AreEqual(file.name, readOpp.Data.name);
            Assert.AreEqual(file.age, readOpp.Data.age);

            // Create a new filesystem, with the same fixed key
            fs = new DalilaFS(rootPath);
            fs._aesEncryption = true;
            fs._aesFixedKey = "New key to serialize";

            // If you change again the key, the files cant be readed more
            var readOpp2 = fs.ReadDCResource<SampleDCSerializableClass>(fileName);
            Assert.IsTrue(readOpp2);

            // Compare the values of the two classes
            Assert.AreEqual(file.id, readOpp2.Data.id);
            Assert.AreEqual(file.name, readOpp2.Data.name);
            Assert.AreEqual(file.age, readOpp2.Data.age);

        }

        [Test]
        public void DalilaFS_DCSerialization_X5ChangeFixedKeyAtRuntine_WillCauseFails()
        {

            // Create a new instance of a dc serializable class and store the name in a variable
            var file = new SampleDCSerializableClass
            {
                id = 2233,
                name = "Andrew",
                age = 23,
            };
            var fileName = "/Sample.xml";

            // Change the fixed key
            fs._aesFixedKey = "New key to serialize";

            // Save the class
            var saveOpp = fs.CreateDCResource(fileName, file);
            Debug.Log(saveOpp);
            Assert.IsTrue(saveOpp);

            // Read the class saved
            var readOpp = fs.ReadDCResource<SampleDCSerializableClass>(fileName);
            Debug.Log(readOpp);
            Assert.IsTrue(readOpp);

            // Compare the values of the two classes
            Assert.AreEqual(file.id, readOpp.Data.id);
            Assert.AreEqual(file.name, readOpp.Data.name);
            Assert.AreEqual(file.age, readOpp.Data.age);

            // If you change again the key, the files cant be readed more
            fs._aesFixedKey = "Changed key";
            var readOpp2 = fs.ReadDCResource<SampleDCSerializableClass>(fileName);
            Assert.IsFalse(readOpp2);

            // Compare the values of the two classes
            Assert.AreEqual(null, readOpp2.Data);

        }



        [Test]
        public void DalilaFS_DCSerialization_X6RandomKeySiplePasses()
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
            Assert.IsTrue(readOpp);

            // Compare the values of the two classes
            Assert.AreEqual(file.id, readOpp.Data.id);
            Assert.AreEqual(file.name, readOpp.Data.name);
            Assert.AreEqual(file.age, readOpp.Data.age);

            // Create a new filesystem, random key will be readed from memory
            fs = new DalilaFS(rootPath);
            fs._aesEncryption = true;

            // A new key will be created, and files encrypted with the old key cant be readed more
            var readOpp2 = fs.ReadDCResource<SampleDCSerializableClass>(fileName);
            Debug.Log(readOpp2);
            Assert.IsTrue(readOpp2);

            // Compare the values of the two classes
            Assert.AreEqual(file.id, readOpp2.Data.id);
            Assert.AreEqual(file.name, readOpp2.Data.name);
            Assert.AreEqual(file.age, readOpp2.Data.age);

        }


        [Test]
        public void DalilaFS_DCSerialization_X7IfEraseRandomKey_FilesCantBeReadedAgain()
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
            Assert.IsTrue(readOpp);

            // Compare the values of the two classes
            Assert.AreEqual(file.id, readOpp.Data.id);
            Assert.AreEqual(file.name, readOpp.Data.name);
            Assert.AreEqual(file.age, readOpp.Data.age);


            // Delete the file with the key
            fs.DeleteResource(fs._aesRandomKeyResourceName);

            // You need to create a new filesystem, because random key is stored in memory once readed
            fs = new DalilaFS(rootPath);
            fs._aesEncryption = true;

            // A new key will be created, and files encrypted with the old key cant be readed more
            var readOpp2 = fs.ReadDCResource<SampleDCSerializableClass>(fileName);
            Debug.Log(readOpp2);
            Assert.IsFalse(readOpp2);

        }



        [Test]
        public void DalilaFS_DCSerialization_X8TwoFileSistemsInSamelocation_WillUseSameRandomKeyAndCanUseDiferentFixedKey()
        {

            // Create a new instance of a dc serializable class and store the name in a variable
            var file = new SampleDCSerializableClass
            {
                id = 2233,
                name = "Andrew",
                age = 23,
            };
            var fileName = "/Sample.xml";
            var file2 = new SampleDCSerializableClass
            {
                id = 3444,
                name = "Andrea",
                age = 21,
            };
            var fileName2 = "/Sample2.xml";

            // Create a new filesystem in same rootPath, with diferent fixed key
            var fs2 = new DalilaFS(rootPath);
            fs2._aesEncryption = true;
            fs2._aesFixedKey = "New key to serialize";

            // Save the first class
            var saveOpp = fs.CreateDCResource(fileName, file);
            Debug.Log(saveOpp);
            Assert.IsTrue(saveOpp);

            // Save the second class
            var saveOpp2 = fs2.CreateDCResource(fileName2, file2);
            Debug.Log(saveOpp2);
            Assert.IsTrue(saveOpp2);

            // Compare the two random keys, must be the same
            Assert.AreEqual(fs._aesRandomKeyResourceName, fs2._aesRandomKeyResourceName);
            Assert.AreEqual(fs._aesRandomKey, fs2._aesRandomKey);

            // Compare full keys, must be diferent because fixed keys are diferent
            Assert.AreNotEqual(fs._aesKey, fs2._aesKey);

            // Read the first class saved
            var readOpp = fs.ReadDCResource<SampleDCSerializableClass>(fileName);
            Debug.Log(readOpp);
            Assert.IsTrue(readOpp);

            // Compare the values of the two classes
            Assert.AreEqual(file.id, readOpp.Data.id);
            Assert.AreEqual(file.name, readOpp.Data.name);
            Assert.AreEqual(file.age, readOpp.Data.age);

            // Read the second class saved
            var readOpp2 = fs2.ReadDCResource<SampleDCSerializableClass>(fileName2);
            Debug.Log(readOpp2);
            Assert.IsTrue(readOpp2);

            // Compare the values of the two classes
            Assert.AreEqual(file2.id, readOpp2.Data.id);
            Assert.AreEqual(file2.name, readOpp2.Data.name);
            Assert.AreEqual(file2.age, readOpp2.Data.age);


            // First filesystem cant read second filesystems files because are diferent, but can overrride them, so be carefully
            var readOpp3 = fs2.ReadDCResource<SampleDCSerializableClass>(fileName);
            Debug.Log(readOpp3);
            Assert.IsFalse(readOpp3);

            var readOpp4 = fs.ReadDCResource<SampleDCSerializableClass>(fileName2);
            Debug.Log(readOpp4);
            Assert.IsFalse(readOpp4);

        }


        [Test]
        public void DalilaFS_DCSerialization_X9TwoFileSistemsInSamelocation_CanUseDiferentRandomKey()
        {

            // Create a new instance of a dc serializable class and store the name in a variable
            var file = new SampleDCSerializableClass
            {
                id = 2233,
                name = "Andrew",
                age = 23,
            };
            var fileName = "/Sample.xml";
            var file2 = new SampleDCSerializableClass
            {
                id = 3444,
                name = "Andrea",
                age = 21,
            };
            var fileName2 = "/Sample2.xml";

            fs._aesRandomKeyResourceName = "/RandomOne.key";

            // Create a new filesystem in same rootPath, with diferent fixed key
            var fs2 = new DalilaFS(rootPath);
            fs2._aesEncryption = true;
            fs2._aesFixedKey = "New key to serialize";
            fs2._aesRandomKeyResourceName = "/RandomTwo.key";

            // Save the first class
            var saveOpp = fs.CreateDCResource(fileName, file);
            Debug.Log(saveOpp);
            Assert.IsTrue(saveOpp);

            // Save the second class
            var saveOpp2 = fs2.CreateDCResource(fileName2, file2);
            Debug.Log(saveOpp2);
            Assert.IsTrue(saveOpp2);

            // Compare the two random keys, must be the same
            Assert.AreNotEqual(fs._aesRandomKeyResourceName, fs2._aesRandomKeyResourceName);
            Assert.AreNotEqual(fs._aesRandomKey, fs2._aesRandomKey);

            // Compare full keys, must be diferent because fixed keys are diferent
            Assert.AreNotEqual(fs._aesKey, fs2._aesKey);

            // Read the first class saved
            var readOpp = fs.ReadDCResource<SampleDCSerializableClass>(fileName);
            Debug.Log(readOpp);
            Assert.IsTrue(readOpp);

            // Compare the values of the two classes
            Assert.AreEqual(file.id, readOpp.Data.id);
            Assert.AreEqual(file.name, readOpp.Data.name);
            Assert.AreEqual(file.age, readOpp.Data.age);

            // Read the second class saved
            var readOpp2 = fs2.ReadDCResource<SampleDCSerializableClass>(fileName2);
            Debug.Log(readOpp2);
            Assert.IsTrue(readOpp2);

            // Compare the values of the two classes
            Assert.AreEqual(file2.id, readOpp2.Data.id);
            Assert.AreEqual(file2.name, readOpp2.Data.name);
            Assert.AreEqual(file2.age, readOpp2.Data.age);


            // First filesystem cant read second filesystems files because are diferent, but can overrride them, so be carefully
            var readOpp3 = fs2.ReadDCResource<SampleDCSerializableClass>(fileName);
            Debug.Log(readOpp3);
            Assert.IsFalse(readOpp3);

            var readOpp4 = fs.ReadDCResource<SampleDCSerializableClass>(fileName2);
            Debug.Log(readOpp4);
            Assert.IsFalse(readOpp4);

        }


    }
}
