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
    public class DalilaFS_Operations_X1ExistLocationOrResources
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
        public void A1_ExistLocation_WhenLocationExist_WillReturnTrue()
        {

            // Create the locations
            DataSource_SacrumFileSystem.CreateLocations(DataSource_SacrumFileSystem.validLocations, fs);

            // Check if exist
            foreach (var location in DataSource_SacrumFileSystem.validLocations)
            {
                Debug.Log("Checking location: " + location);
                var operation = fs.ExistLocation(location);
                Assert.IsTrue(operation);
                Assert.Null(operation.Error);
            }

        }

        [Test]
        public void A2_ExistLocation_WhenLocationDoesntExist_WillReturnFalse()
        {

            // Check if exist
            foreach (var location in DataSource_SacrumFileSystem.validLocations)
            {
                if (location == "/")  // Root must exist
                    continue;

                Debug.Log("Checking location: " + location);
                var operation = fs.ExistLocation(location);
                Assert.IsFalse(operation);
                Assert.Throws<DirectoryNotFoundException>(() => throw operation.Error);
            }

        }

        [Test]
        public void A3_ExistLocation_WhenLocationIsInvalid_WillReturnFalse()
        {

            // Check if exist
            foreach (var location in DataSource_SacrumFileSystem.validLocations)
            {
                Debug.Log("Checking location: " + location + "#invalid%%");
                var operation = fs.ExistLocation(location + "#invalid%%");
                Assert.IsFalse(operation);
                Assert.Throws<FormatException>(() => throw operation.Error);
            }

        }

        [Test]
        public void A4_ExistLocation_WhenAResourceIsPassed_WillReturnFalse()
        {

            // Check if exist
            foreach (var location in DataSource_SacrumFileSystem.validResources)
            {
                Debug.Log("Checking location: " + location);
                var operation = fs.ExistLocation(location);
                Assert.IsFalse(operation);
                Assert.Throws<FormatException>(() => throw operation.Error);
            }

        }

        [Test]
        public void A5_ExistLocation_WhenExistAResourceWithSameName_WillReturnFalse()
        {

            var validResources = new string[]
            {
                "/cat",
                "/dog",
                "/lion/66",
            };

            var validLocations = new string[]
            {
                "/cat/",
                "/dog/",
                "/lion/66/",
            };

            // Create the resources
            DataSource_SacrumFileSystem.CreateResources(validResources, fs);

            // Check if exist
            foreach (var location in validLocations)
            {
                Debug.Log("Checking location: " + location);
                var operation = fs.ExistLocation(location);
                Assert.IsFalse(operation);
                Assert.Throws<DirectoryNotFoundException>(() => throw operation.Error);
            }

            // Delete the resources
            DataSource_SacrumFileSystem.DeleteResources(validResources, fs);

        }





        [Test]
        public void B1_ExistResource_WhenResourceExist_WillReturnTrue()
        {
            // Create the resources
            DataSource_SacrumFileSystem.CreateResources(DataSource_SacrumFileSystem.validResources, fs);

            // Check if exist
            foreach (var resource in DataSource_SacrumFileSystem.validResources)
            {
                Debug.Log("Checking resource: " + resource);
                var operation = fs.ExistResource(resource);
                Assert.IsTrue(operation);
                Assert.Null(operation.Error);
            }
        }

        [Test]
        public void B2_ExistResource_WhenResourceDoesntExist_WillReturnFalse()
        {
            // Check if exist
            foreach (var resource in DataSource_SacrumFileSystem.validResources)
            {
                
                Debug.Log("Checking resource: " + resource);
                var operation = fs.ExistResource(resource);
                Assert.IsFalse(operation);
                Assert.Throws<FileNotFoundException>(() => throw operation.Error);
            }
        }

        [Test]
        public void B3_ExistResource_WhenResourceIsInvalid_WillReturnFalse()
        {
            // Check if exist
            foreach (var resource in DataSource_SacrumFileSystem.validResources)
            {
                Debug.Log("Checking resource: " + resource + "#invalid%%");
                var operation = fs.ExistResource(resource + "#invalid%%");
                Assert.IsFalse(operation);
                Assert.Throws<FormatException>(() => throw operation.Error);
            }

        }

        [Test]
        public void B4_ExistResource_WhenALocationIsPassed_WillReturnFalse()
        {

            // Check if exist
            foreach (var resource in DataSource_SacrumFileSystem.validLocations)
            {
                Debug.Log("Checking location: " + resource);
                var operation = fs.ExistResource(resource);
                Assert.IsFalse(operation);
                Assert.Throws<FormatException>(() => throw operation.Error);
            }

        }

        [Test]
        public void B5_ExistResource_WhenExistALocationWithSameName_WillReturnFalse()
        {

            var validResources = new string[]
            {
                "/cat",
                "/dog",
                "/lion/66",
            };

            var validLocations = new string[]
            {
                "/cat/",
                "/dog/",
                "/lion/66/",
            };

            // Create the locations
            DataSource_SacrumFileSystem.CreateLocations(validLocations, fs);

            // Check if exist
            foreach (var resource in validResources)
            {
                Debug.Log("Checking resource: " + resource);
                var operation = fs.ExistResource(resource);
                Assert.IsFalse(operation);
                Assert.Throws<FileNotFoundException>(() => throw operation.Error);
            }

            // Delete the locations
            DataSource_SacrumFileSystem.DeleteLocations(validLocations, fs);

        }

    }
}
