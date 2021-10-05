using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using U.DalilaDB;
using System.IO;
using System.Data;
using System;

namespace DalilaFsTests
{
    public class DalilaFS_Operations_X6RenameLocationOrResource
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
        public void A1_RenameLocation_WhenLocationsExist_WillRenameAndDeleteOrigin()
        {
            var validFromLocations = new string[]
            {
                "/mouse/dog/cat/",
                "/jiraf/33/67/",
                "/34/66/99/99/",
                "/The/End/",
            };

            var validToLocations = new string[]
            {
                "/taco/",
                "/pizza/",
                "/99/66/",
                "/The/Start/",
            };

            // Create the resources and locations
            DataSource_SacrumFileSystem.CreateLocations(validFromLocations, fs);

            var j = 0;
            foreach (var location in validFromLocations)
            {
                Debug.Log("Rename: " + location);

                var operation = fs.RenameLocation(location, validToLocations[j]);
                Assert.IsTrue(operation);
                Assert.IsTrue(operation == 1);
                Assert.Null(operation.Error);

                Assert.IsFalse(Directory.Exists(fs.LocationToSystemPath(location)));
                Assert.IsTrue(Directory.Exists(fs.LocationToSystemPath(validToLocations[j])));

                j++;
            }

            // Delete the resources and locations
            DataSource_SacrumFileSystem.DeleteLocations(validToLocations, fs);

        }

        [Test]
        public void A2_RenameLocation_WhenLocationsExist_WillRenameAndDeleteOriginOnlyEmpty()
        {
            var validFromLocations = new string[]
            {
                "/mouse/lion/dog/",
                "/jiraf/mouse/lion/",
                "/34/66/66/",
            };

            var validFromLocationsExtras = new string[]
            {
                "/mouse/lion/22/",
                "/jiraf/mouse/22/",
                "/34/66/77/",
            };

            var validToLocations = new string[]
            {
                "/taco/",
                "/pizza/",
                "/99/66/",
            };

            // Create the resources and locations
            DataSource_SacrumFileSystem.CreateLocations(validFromLocations, fs);
            DataSource_SacrumFileSystem.CreateLocations(validFromLocationsExtras, fs);

            var j = 0;
            foreach (var location in validFromLocations)
            {
                Debug.Log("Rename: " + location);

                var operation = fs.RenameLocation(location, validToLocations[j]);
                Debug.Log("OOp: " + operation);
                Assert.IsTrue(operation);
                Assert.IsTrue(operation == 1);
                Assert.Null(operation.Error);

                Assert.IsTrue(Directory.Exists(fs.LocationToSystemPath(validFromLocationsExtras[j])));
                Assert.IsTrue(Directory.Exists(fs.LocationToSystemPath(validToLocations[j])));

                j++;
            }

            // Delete the resources and locations
            DataSource_SacrumFileSystem.DeleteLocations(validToLocations, fs);
            DataSource_SacrumFileSystem.DeleteLocations(validFromLocations, fs);

        }

        [Test]
        public void A3_RenameLocation_WhenToLocationAlreadyExistOrExistAsResource_WillReturnFalse()
        {
            var validFromLocations = new string[]
            {
                "/mouse/",
                "/jiraf/",
                "/34/66/",
            };

            var validResources = new string[]
            {
                "/cat",
                "/dog",
                "/lion/66",
            };

            var validLocations = new string[]
            {
                "/taco/",
                "/pizza/",
                "/99/66/",
            };

            // Create the resources and locations
            DataSource_SacrumFileSystem.CreateLocations(validFromLocations, fs);
            DataSource_SacrumFileSystem.CreateResources(validResources, fs);
            DataSource_SacrumFileSystem.CreateLocations(validLocations, fs);

            var i = 0;
            foreach (var location in validFromLocations)
            {
                Debug.Log("Rename: " + location);

                var operation = fs.RenameLocation(location, validResources[i]+"/");
                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<DuplicateNameException>(() => throw operation.Error);

                i++;
            }

            var j = 0;
            foreach (var location in validFromLocations)
            {
                Debug.Log("Rename: " + location);

                var operation = fs.RenameLocation(location, validLocations[j]);
                Assert.IsFalse(operation);
                Assert.Throws<DuplicateNameException>(() => throw operation.Error);

                j++;
            }


            // Create the resources and locations
            DataSource_SacrumFileSystem.DeleteLocations(validFromLocations, fs);
            DataSource_SacrumFileSystem.DeleteResources(validResources, fs);
            DataSource_SacrumFileSystem.DeleteLocations(validLocations, fs);

        }

        [Test]
        public void A4_RenameLocation_WhenFromLocationNoExist_WillReturnFalse()
        {
            foreach (var location in DataSource_SacrumFileSystem.validLocations)
            {
                if (location == "/") continue;  // root always exist

                Debug.Log("Rename: " + location);

                var operation = fs.RenameLocation(location, "/dog/cat/");
                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<DirectoryNotFoundException>(() => throw operation.Error);

                Assert.IsTrue(!Directory.Exists(fs.LocationToSystemPath(location)));
            }

        }

        [Test]
        public void A5_RenameLocation_WhenFromLocationIsAResource_WillReturnFalse()
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


            foreach (var location in validResourcesAsLocations)
            {
                
                Debug.Log("Rename: " + location);

                var operation = fs.RenameLocation(location, location + "dog/cat/");
                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<DirectoryNotFoundException>(() => throw operation.Error);

                Assert.IsTrue(!Directory.Exists(fs.LocationToSystemPath(location)));
            }

            // Create the resources
            DataSource_SacrumFileSystem.DeleteResources(validResources, fs);

        }

        [Test]
        public void A6_RenameLocation_WhenInvalidFromLocationPasses_WillReturnFalse()
        {
            foreach (var location in DataSource_SacrumFileSystem.validLocations)
            {
                Debug.Log("Delete: " + location + "%%invalid))");

                var operation = fs.RenameLocation(location + "%%invalid))", location);
                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<FormatException>(() => throw operation.Error);

            }
        }

        [Test]
        public void A7_RenameLocation_WhenInvalidToLocationToPasses_WillReturnFalse()
        {
            foreach (var location in DataSource_SacrumFileSystem.validLocations)
            {
                Debug.Log("Delete: " + location + "%%invalid))");

                var operation = fs.RenameLocation(location, location + "%%invalid))");
                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<FormatException>(() => throw operation.Error);

            }
        }




        [Test]
        public void B1_RenameResource_WhenResourceExist_WillRename()
        {
            var validFromResources = new string[]
            {
                "/mouse/dog/cat",
                "/jiraf/33/67",
                "/34/66/99/99",
                "/The/End",
            };

            var validToResources = new string[]
            {
                "/taco",
                "/pizza",
                "/99/66",
                "/The/Start",
            };

            // Create the resources and locations
            DataSource_SacrumFileSystem.CreateResources(validFromResources, fs);

            var j = 0;
            foreach (var resource in validFromResources)
            {
                
                var operation = fs.RenameResource(resource, validToResources[j]);
                Debug.Log("Rename: " + resource + " Operation: " + operation);
                
                Assert.IsTrue(operation);
                Assert.IsTrue(operation == 1);
                Assert.Null(operation.Error);

                Assert.IsFalse(File.Exists(fs.ResourceToSystemPath(resource)));
                Assert.IsTrue(File.Exists(fs.ResourceToSystemPath(validToResources[j])));

                j++;
            }

            // Delete the resources and locations
            DataSource_SacrumFileSystem.DeleteResources(validToResources, fs);

        }

        [Test]
        public void B2_RenameResource_WhenToResourceAlreadyExistOrExistAsLocation_WillReturnFalse()
        {
            var validFromResourcess = new string[]
            {
                "/mouse",
                "/jiraf",
                "/34/66",
            };

            var validToResources = new string[]
            {
                "/cat",
                "/dog",
                "/lion/66",
            };

            var validLocations = new string[]
            {
                "/taco/",
                "/pizza/",
                "/99/66/",
            };

            // Create the resources and locations
            DataSource_SacrumFileSystem.CreateResources(validFromResourcess, fs);
            DataSource_SacrumFileSystem.CreateResources(validToResources, fs);
            DataSource_SacrumFileSystem.CreateLocations(validLocations, fs);

            var i = 0;
            foreach (var resource in validFromResourcess)
            {
                var operation = fs.RenameResource(resource, validToResources[i]);
                Debug.Log("Rename: " + resource + " Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<DuplicateNameException>(() => throw operation.Error);

                i++;
            }

            var j = 0;
            foreach (var resource in validFromResourcess)
            {
                var operation = fs.RenameResource(resource, validLocations[j].TrimEnd('/'));
                Debug.Log("Rename: " + resource + " Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<DuplicateNameException>(() => throw operation.Error);

                j++;
            }


            // Create the resources and locations
            DataSource_SacrumFileSystem.DeleteResources(validFromResourcess, fs);
            DataSource_SacrumFileSystem.DeleteResources(validToResources, fs);
            DataSource_SacrumFileSystem.DeleteLocations(validLocations, fs);

        }

        [Test]
        public void B3_RenameResource_WhenFromResourceNoExist_WillReturnFalse()
        {
            foreach (var resource in DataSource_SacrumFileSystem.validResources)
            {
                var operation = fs.RenameResource(resource, "/dog/cat");
                Debug.Log("Rename: " + resource + " Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<FileNotFoundException>(() => throw operation.Error);

                Assert.IsTrue(!File.Exists(fs.ResourceToSystemPath(resource)));
            }

        }

        [Test]
        public void B4_RenameResource_WhenFromResourceIsALocation_WillReturnFalse()
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


            foreach (var resource in validResources)
            {

                var operation = fs.RenameResource(resource, resource + "dog/cat");
                Debug.Log("Rename: " + resource + " Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<FileNotFoundException>(() => throw operation.Error);

                Assert.IsTrue(!File.Exists(fs.ResourceToSystemPath(resource)));
            }

            // delete the resources
            DataSource_SacrumFileSystem.DeleteLocations(validResourcesAsLocations, fs);

        }

        [Test]
        public void B5_RenameResource_WhenInvalidFromResourcePasses_WillReturnFalse()
        {
            foreach (var resource in DataSource_SacrumFileSystem.validResources)
            {
                Debug.Log("Delete: " + resource + "%%invalid))");

                var operation = fs.RenameResource(resource + "%%invalid))", resource);
                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<FormatException>(() => throw operation.Error);

            }
        }

        [Test]
        public void B6_RenameResource_WhenInvalidToResourcePasses_WillReturnFalse()
        {
            foreach (var resource in DataSource_SacrumFileSystem.validResources)
            {
                Debug.Log("Delete: " + resource + "%%invalid))");

                var operation = fs.RenameResource(resource, resource + "%%invalid))");
                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<FormatException>(() => throw operation.Error);

            }
        }


    }
}
