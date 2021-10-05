using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using U.DalilaDB;
using System.IO;
using System;

namespace DalilaFsTests
{
    public class DalilaFS_Operations_X3IsEmptyLocation
    {
        // The filesystem in a path
        string rootPath = Application.persistentDataPath + "/233e23e23e32ed23e23";
        DalilaFS fs;


        [SetUp]
        public void SetUp()
        {
            // Create a new auxiliar path and filesystem
            if (Directory.Exists(rootPath))
                Directory.Delete(rootPath, true);
            Directory.CreateDirectory(rootPath);
            fs = new DalilaFS(rootPath);
        }



        [Test]
        public void A1_IsEmptyLocation_WhenLocationExistAnsIsEmpty_WillReturnTrue()
        {

            var validLocations = new string[]
            {
                "/cat/",
                "/dog/",
                "/lion/66/",
                "/bull/",
                "/DotNet/Dog/"
            };



            // Create
            DataSource_SacrumFileSystem.CreateLocations(validLocations, fs);

            foreach (var location in validLocations)
            {
                Debug.Log("Empty: " + location);

                var operation = fs.IsEmptyLocation(location);
                Assert.IsTrue(operation);
                Assert.Null(operation.Error);

            }

            // Create
            DataSource_SacrumFileSystem.DeleteLocations(validLocations, fs);
        }

        [Test]
        public void A2_IsEmptyLocation_WhenExistAndIsNotEmpty_WillReturnFalse()
        {
            var validResources = new string[]
            {
                "/cat/bull",
                "/dog/moth",
                "/lion/66",
                "/bull/99/77",
            };


            // Create
            DataSource_SacrumFileSystem.CreateResources(validResources, fs);

            foreach (var resource in validResources)
            {
                Debug.Log("Empty: " + DalilaFS.GetResourceLocation(resource));

                var operation = fs.IsEmptyLocation(DalilaFS.GetResourceLocation(resource));
                Assert.IsFalse(operation);
                Assert.Throws<FileNotFoundException>(() => throw operation.Error);

            }

            // Create
            DataSource_SacrumFileSystem.DeleteResources(validResources, fs);
        }

        [Test]
        public void A3_IsEmptyLocation_WhenLocationNoExist_WillReturnFalse()
        {
            foreach (var location in DataSource_SacrumFileSystem.validLocations)
            {
                if (location == "/") continue;  // Root is not empty

                Debug.Log("Empty: " + location);

                var operation = fs.IsEmptyLocation(location);
                Assert.IsFalse(operation);
                Assert.Throws<DirectoryNotFoundException>(() => throw operation.Error);

            }
        }

        [Test]
        public void A4_IsEmptyLocation_WhenInvalidLocationPasses_WillReturnFalse()
        {
            foreach (var location in DataSource_SacrumFileSystem.validLocations)
            {
                Debug.Log("Empty: " + location + "%%invalid))");

                var operation = fs.IsEmptyLocation(location + "%%invalid))");
                Assert.IsFalse(operation);
                Assert.Throws<FormatException>(() => throw operation.Error);

            }
        }

        [Test]
        public void A5_IsEmptyLocation_WhenLocationIsAResource_WillReturnFalse()
        {
            var validResources = new string[]
            {
                "/cat",
                "/dog",
                "/lion/66",
            };

            // Create the resources
            DataSource_SacrumFileSystem.CreateResources(validResources, fs);


            var validResourcesAsLocations = new string[]
            {
                "/cat/",
                "/dog/",
                "/lion/66/",
            };

            // Create the locations
            foreach (var location in validResourcesAsLocations)
            {
                Debug.Log("Empty: " + location);
                var operation = fs.IsEmptyLocation(location);
                Assert.IsFalse(operation);
                Assert.Throws<DirectoryNotFoundException>(() => throw operation.Error);
            }


            // Create the resources
            DataSource_SacrumFileSystem.DeleteResources(validResources, fs);
        }

    }
}
