using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using U.DalilaDB;
using System.IO;
using System;
using System.Data;

namespace DalilaFsTests
{
    public class DalilaFS_Operations_X4DeleteLocationOrResources
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
        public void A1_DeleteLocation_WhenLocationExistAndIsEmpty_WillDeleteAndReturnTrueAndOneExeptForRoot()
        {

            // Create
            DataSource_SacrumFileSystem.CreateLocations(DataSource_SacrumFileSystem.validLocations, fs);

            foreach (var location in DataSource_SacrumFileSystem.validLocations)
            {
                if (location == "/" || location.StartsWith("/apple") || location.StartsWith("/banana") || location.StartsWith("/blackberry") || location.StartsWith("/tomato")) continue;  // Cant delete root

                var operation = fs.DeleteLocation(location);
                Debug.Log("Delete: " + location + " Operation: " + operation);

                Assert.IsTrue(operation);
                Assert.IsTrue(operation == 1);
                Assert.Null(operation.Error);

                Assert.IsTrue(!Directory.Exists(fs.LocationToSystemPath(location)));
            }


            // Delete
            DataSource_SacrumFileSystem.DeleteLocations(DataSource_SacrumFileSystem.validLocations, fs);

        }

        [Test]
        public void A2_DeleteLocation_WhenLocationAlreadyExistAndIsNotEmpty_WillDeleteAndReturnTrue()
        {
            // Create
            DataSource_SacrumFileSystem.CreateResources(DataSource_SacrumFileSystem.validResources, fs);

            foreach (var location in DataSource_SacrumFileSystem.validResourcesLocation)
            {
                if (location == "/" || location.StartsWith("/beaver") || location.StartsWith("/goose") || location.StartsWith("/cats") || location.StartsWith("/bull") || location.StartsWith("/birds") || location.StartsWith("/camel")) continue;  // Cant delete root

                var operation = fs.DeleteLocation(location);
                Debug.Log("Delete: " + location + " Operation: " + operation);

                Assert.IsTrue(operation);
                Assert.IsTrue(operation == 1);
                Assert.Null(operation.Error);

                Assert.IsTrue(!Directory.Exists(fs.LocationToSystemPath(location)));
            }


            // Delete
            DataSource_SacrumFileSystem.DeleteResources(DataSource_SacrumFileSystem.validResources, fs);
        }

        [Test]
        public void A3_DeleteLocation_WhenLocationNoExist_WillReturnTrueAndCero()
        {
            foreach (var location in DataSource_SacrumFileSystem.validLocations)
            {
                if (location == "/") continue;  // Cant delete root

                var operation = fs.DeleteLocation(location);
                Debug.Log("Delete: " + location + " Operation: " + operation);

                Assert.IsTrue(operation);
                Assert.IsTrue(operation == 0);
                Assert.Null(operation.Error);

                Assert.IsTrue(!Directory.Exists(fs.LocationToSystemPath(location)));
            }

        }

        [Test]
        public void A4_DeleteLocation_WhenInvalidLocationPasses_WillReturnFalse()
        {
            foreach (var location in DataSource_SacrumFileSystem.validLocations)
            {
                var operation = fs.DeleteLocation(location + "##invalid##");
                Debug.Log("Delete: " + location + "##invalid## Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<FormatException>(() => throw operation.Error);

            }
        }

        [Test]
        public void A5_DeleteLocation_WhenLocationIsAResource_WillReturnFalse()
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

            // Delete the locations
            foreach (var location in validResourcesAsLocations)
            {
                var operation = fs.DeleteLocation(location);
                Debug.Log("Delete: " + location + " Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<DuplicateNameException>(() => throw operation.Error);
            }


            // Delete the resources
            DataSource_SacrumFileSystem.DeleteResources(validResources, fs);
        }




        [Test]
        public void C1_DeleteEmpty_WhenLocationAlreadyExistAndIsEmpty_WillDeleteAndReturnTrueAndCeroExeptForRoot()
        {

            // Create
            DataSource_SacrumFileSystem.CreateLocations(DataSource_SacrumFileSystem.validLocations, fs);

            foreach (var location in DataSource_SacrumFileSystem.validLocations)
            {
                if (location == "/" ||
                    location.StartsWith("/apple/") ||
                    location.StartsWith("/banana/") ||
                    location.StartsWith("/blackberry/") ||
                    location.StartsWith("/tomato/") ||
                    location.StartsWith("") ||
                    location.StartsWith("") ||
                    location.StartsWith("") 
                    ) continue;  // Cant delete root

                var operation = fs.DeleteEmptyLocation(location);
                Debug.Log("Delete: " + location + " Operation: " + operation);

                Assert.IsTrue(operation);
                Assert.IsTrue(operation == 1);
                Assert.Null(operation.Error);

                Assert.IsTrue(!Directory.Exists(fs.LocationToSystemPath(location)));
            }


            // Delete
            DataSource_SacrumFileSystem.DeleteLocations(DataSource_SacrumFileSystem.validLocations, fs);

        }

        [Test]
        public void C2_DeleteEmpty_WhenLocationAlreadyExistAndIsNotEmpty_WillNotDeleteAndReturnFalse()
        {
            // Create
            DataSource_SacrumFileSystem.CreateResources(DataSource_SacrumFileSystem.validResources, fs);

            foreach (var location in DataSource_SacrumFileSystem.validResourcesLocation)
            {
                if (location == "/" || location.StartsWith("/beaver") || location.StartsWith("/goose/") || location.StartsWith("/cats/") || location.StartsWith("/bull/") || location.StartsWith("/birds/") || location.StartsWith("/camel/")) continue;  // Cant delete root

                var operation = fs.DeleteEmptyLocation(location);
                Debug.Log("Delete: " + location + " Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<InvalidOperationException>(() => throw operation.Error);

                Assert.IsTrue(Directory.Exists(fs.LocationToSystemPath(location)));
            }


            // Delete
            DataSource_SacrumFileSystem.DeleteResources(DataSource_SacrumFileSystem.validResources, fs);
        }

        [Test]
        public void C3_DeleteEmpty_WhenLocationNoExist_WillReturnTrueAndCero()
        {
            foreach (var location in DataSource_SacrumFileSystem.validLocations)
            {
                if (location == "/") continue;  // Cant delete root

                var operation = fs.DeleteEmptyLocation(location);
                Debug.Log("Delete: " + location + " Operation: " + operation);

                Assert.IsTrue(operation);
                Assert.IsTrue(operation == 0);
                Assert.Null(operation.Error);

                Assert.IsTrue(!Directory.Exists(fs.LocationToSystemPath(location)));
            }

        }

        [Test]
        public void C4_DeleteEmpty_WhenInvalidLocationPasses_WillReturnFalse()
        {
            foreach (var location in DataSource_SacrumFileSystem.validLocations)
            {
                var operation = fs.DeleteEmptyLocation(location + "##invalid##");
                Debug.Log("Delete: " + location + "##invalid## Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<FormatException>(() => throw operation.Error);

            }
        }

        [Test]
        public void C5_DeleteEmpty_WhenLocationIsAResource_WillReturnFalse()
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

            // Delete the locations
            foreach (var location in validResourcesAsLocations)
            {
                var operation = fs.DeleteEmptyLocation(location);
                Debug.Log("Delete: " + location + " Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<DuplicateNameException>(() => throw operation.Error);
            }


            // Delete the resources
            DataSource_SacrumFileSystem.DeleteResources(validResources, fs);
        }




        [Test]
        public void B1_DeleteResource_WhenResourceExist_WillDeleteAndReturnTrueAndOne()
        {

            // Create
            DataSource_SacrumFileSystem.CreateResources(DataSource_SacrumFileSystem.validResources, fs);

            foreach (var resource in DataSource_SacrumFileSystem.validResources)
            {
                
                var operation = fs.DeleteResource(resource);
                Debug.Log("Delete: " + resource + " Operation: " + operation);

                Assert.IsTrue(operation);
                Assert.IsTrue(operation == 1);
                Assert.Null(operation.Error);

                Assert.IsTrue(!File.Exists(fs.ResourceToSystemPath(resource)));
            }


            // Delete
            DataSource_SacrumFileSystem.CreateResources(DataSource_SacrumFileSystem.validResources, fs);

        }

        [Test]
        public void B3_DeleteResource_WhenResourceNoExist_WillReturnTrueAndCero()
        {
            foreach (var resource in DataSource_SacrumFileSystem.validResources)
            {
                
                var operation = fs.DeleteResource(resource);
                Debug.Log("Delete: " + resource + " Operation: " + operation);

                Assert.IsTrue(operation);
                Assert.IsTrue(operation == 0);
                Assert.Null(operation.Error);

                Assert.IsTrue(!File.Exists(fs.ResourceToSystemPath(resource)));
            }

        }

        [Test]
        public void B4_DeleteResource_WhenInvalidResourcePasses_WillReturnFalse()
        {
            foreach (var resource in DataSource_SacrumFileSystem.validResources)
            {
                var operation = fs.DeleteResource(resource + "##invalid##");
                Debug.Log("Delete: " + resource + "##invalid## Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<FormatException>(() => throw operation.Error);

            }
        }

        [Test]
        public void B5_DeleteResource_WhenResourceIsALocation_WillReturnFalse()
        {
            var validResources = new string[]
            {
                "/cat",
                "/dog",
                "/lion/66",
            };

            var validResourcesAsLocations = new string[]
            {
                "/cat/",
                "/dog/",
                "/lion/66/",
            };

            // Create the locations
            DataSource_SacrumFileSystem.CreateLocations(validResourcesAsLocations, fs);

            // Delete the resource
            foreach (var resource in validResources)
            {
                var operation = fs.DeleteResource(resource);
                Debug.Log("Delete: " + resource + " Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<DuplicateNameException>(() => throw operation.Error);
            }


            // Delete the locations
            DataSource_SacrumFileSystem.DeleteLocations(validResourcesAsLocations, fs);
        }


    }
}
