using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using U.DalilaDB;
using System.IO;

namespace DalilaFsTests
{
    public class DalilaFS_Operations_X2CreateLocation
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
        public void A1_CreateLocation_WhenLocationDoesntExist_WillCreateAndReturnTrueAndOne()
        {
            foreach(var location in DataSource_SacrumFileSystem.validLocations)
            {
                if (location == "/") continue;  // Root will exists

                var operation = fs.CreateLocation(location);
                Debug.Log("Creating: " + location + " Operation: " + operation);

                Assert.IsTrue(operation);
                Assert.IsTrue(operation == 1);
                Assert.Null(operation.Error);

                Assert.IsTrue(Directory.Exists(fs.LocationToSystemPath(location)));
            }

            // Delete
            DataSource_SacrumFileSystem.DeleteLocations(DataSource_SacrumFileSystem.validLocations, fs);
        }

        [Test]
        public void A2_CreateLocation_WhenLocationAlreadyExist_WillReturnTrueAndCero()
        {
            // Create
            DataSource_SacrumFileSystem.CreateLocations(DataSource_SacrumFileSystem.validLocations, fs);


            foreach (var location in DataSource_SacrumFileSystem.validLocations)
            {
                var operation = fs.CreateLocation(location);
                Debug.Log("Creating: " + location + " Operation: " + operation);

                Assert.IsTrue(operation);
                Assert.IsTrue(operation == 0);
                Assert.Null(operation.Error);

                Assert.IsTrue(Directory.Exists(fs.LocationToSystemPath(location)));
            }


            // Delete
            DataSource_SacrumFileSystem.DeleteLocations(DataSource_SacrumFileSystem.validLocations, fs);
        }

        [Test]
        public void A3_CreateLocation_WhenInvalidLocationPasses_WillReturnFalseAndcero()
        {
            foreach (var location in DataSource_SacrumFileSystem.validLocations)
            {
                var operation = fs.CreateLocation(location + "%%invalid))");
                Debug.Log("Creating: " + location + "%%invalid))" + " Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<System.FormatException>(() => throw operation.Error);

            }
        }

        [Test]
        public void A4_CreateLocation_WhenAResourceExistWithTheSameName_WillReturnFalseAndDontCreate()
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

            // Create the locations
            foreach (var location in validLocations)
            {
                var operation = fs.CreateLocation(location);
                Debug.Log("Creating: " + location + " Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<System.Data.DuplicateNameException>(() => throw operation.Error);
            }

            // Delete the resources
            DataSource_SacrumFileSystem.DeleteResources(validResources, fs);

        }

    }
}
