using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using NUnit.Framework;
using U.DalilaDB;
using UnityEngine;
using UnityEngine.TestTools;

namespace DalilaFsTests
{
    public class DalilaFS_Operations_X7CopyResourceOrLocation
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
        public void A1_CopyResource_WhenAllIsFine_MustCopy()
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
                "/99/33/66",
                "/The/Start",
            };


            // Create the resources
            DataSource_SacrumFileSystem.CreateResources(validFromResources, fs);

            var j = 0;
            foreach (var resource in validFromResources)
            {

                var operation = fs.CopyResource(resource, validToResources[j]);
                Debug.Log("copy: " + resource + " Operation: " + operation);

                Assert.IsTrue(operation);
                Assert.IsTrue(operation == 1);
                Assert.Null(operation.Error);

                Assert.IsTrue(File.Exists(fs.ResourceToSystemPath(resource)));
                Assert.IsTrue(File.Exists(fs.ResourceToSystemPath(validToResources[j])));

                j++;
            }

        }

        [Test]
        public void A2_CopyResource_WhenFromResourceDontExist_MustReturnFalse()
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
                "/99/33/66",
                "/The/Start",
            };


            // Create the resources
            //DataSource_SacrumFileSystem.CreateResources(validFromResources, fileSystem);

            var j = 0;
            foreach (var resource in validFromResources)
            {

                var operation = fs.CopyResource(resource, validToResources[j]);
                Debug.Log("copy: " + resource + " Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<FileNotFoundException>(() => throw operation.Error);

                Assert.IsFalse(File.Exists(fs.ResourceToSystemPath(resource)));
                Assert.IsFalse(File.Exists(fs.ResourceToSystemPath(validToResources[j])));

                j++;
            }

        }

        [Test]
        public void A3_CopyResource_WhenFromResourceIsALocation_MustReturnFalse()
        {
            var validFromResources = new string[]
            {
                "/mouse/dog/cat",
                "/jiraf/33/67",
                "/34/66/99/99",
                "/The/End",
            };

            var validFromResourcesAsLocations = new string[]
            {
                "/mouse/dog/cat/",
                "/jiraf/33/67/",
                "/34/66/99/99/",
                "/The/End/",
            };

            var validToResources = new string[]
            {
                "/taco",
                "/pizza",
                "/99/33/66",
                "/The/Start",
            };


            // Create the resources as locations
            DataSource_SacrumFileSystem.CreateLocations(validFromResourcesAsLocations, fs);

            var j = 0;
            foreach (var resource in validFromResources)
            {

                var operation = fs.CopyResource(resource, validToResources[j]);
                Debug.Log("copy: " + resource + " Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<FileNotFoundException>(() => throw operation.Error);

                Assert.IsTrue(Directory.Exists(fs.LocationToSystemPath(validFromResourcesAsLocations[j])));
                Assert.IsFalse(File.Exists(fs.ResourceToSystemPath(validToResources[j])));

                j++;
            }

        }

        [Test]
        public void A4_CopyResource_WhenToResourceAlreadyExist_MustReturnFalse()
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
                "/99/33/66",
                "/The/Start",
            };


            // Create the resources
            DataSource_SacrumFileSystem.CreateResources(validFromResources, fs);
            DataSource_SacrumFileSystem.CreateResources(validToResources, fs);

            var j = 0;
            foreach (var resource in validFromResources)
            {

                var operation = fs.CopyResource(resource, validToResources[j]);
                Debug.Log("copy: " + resource + " Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<DuplicateNameException>(() => throw operation.Error);

                Assert.IsTrue(File.Exists(fs.ResourceToSystemPath(resource)));
                Assert.IsTrue(File.Exists(fs.ResourceToSystemPath(validToResources[j])));

                j++;
            }

        }

        [Test]
        public void A5_CopyResource_WhenFromResourceIsInvalid_MustReturnFalse()
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
                "/99/33/66",
                "/The/Start",
            };


            // Create the resources
            //DataSource_SacrumFileSystem.CreateResources(validFromResources, fileSystem);

            var j = 0;
            foreach (var resource in validFromResources)
            {

                var operation = fs.CopyResource(resource + "##invalid##", validToResources[j]);
                Debug.Log("copy: " + resource + "##invalid## Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<FormatException>(() => throw operation.Error);

                j++;
            }

        }

        [Test]
        public void A6_CopyResource_WhenToResourceIsInvalid_MustReturnFalse()
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
                "/99/33/66",
                "/The/Start",
            };


            // Create the resources
            //DataSource_SacrumFileSystem.CreateResources(validFromResources, fileSystem);

            var j = 0;
            foreach (var resource in validFromResources)
            {

                var operation = fs.CopyResource(resource, validToResources[j] + "##invalid##");
                Debug.Log("copy: " + resource + " Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<FormatException>(() => throw operation.Error);

                j++;
            }

        }




        [Test]
        public void B1_CopyLocation_WhenAllIsFine_MustCopy()
        {
            var validFromLocations = new string[]
            {
                "/mouse/dog/cat/",
                "/jiraf/33/67/",
                "/34/66/99/99/",
                "/The/End/",
            };

            var validFromSubResources = new string[]
            {
                "/mouse/dog/cat/dd",
                "/jiraf/33/67/ee",
                "/34/66/99/99/ee",
                "/The/End/ee",
                "/mouse/dog/cat/eee/uu",
                "/jiraf/33/67/11/78",
                "/34/66/99/99/78/77",
                "/The/End/dd",
                "/mouse/dog/cat/dddd",
                "/jiraf/33/67/dd",
                "/34/66/99/99/78/7888/677",
                "/The/End/377/yuu/66",
                "/mouse/dog/cat/344/uii",
                "/jiraf/33/67/333",
                "/34/66/99/99/333",
                "/The/End/444",
                "/mouse/dog/cat/44/88/888999",
                "/jiraf/33/67/33",
                "/34/66/99/99/44",
                "/The/End/3333",
                "/mouse/dog/cat/444",
                "/jiraf/33/67/22",
                "/34/66/99/99/2333",
                "/The/End/44",
            };

            var validToLocations = new string[]
            {
                "/taco/",
                "/pizza/",
                "/99/33/66/",
                "/The/Start/",
            };

            var validToSubResources = new string[]
            {
                "/taco/dd",
                "/pizza/ee",
                "/99/33/66/ee",
                "/The/Start/ee",
                "/taco/eee/uu",
                "/pizza/11/78",
                "/99/33/66/78/77",
                "/The/Start/dd",
                "/taco/dddd",
                "/pizza/dd",
                "/99/33/66/78/7888/677",
                "/The/Start/377/yuu/66",
                "/taco/344/uii",
                "/pizza/333",
                "/99/33/66/333",
                "/The/Start/444",
                "/taco/44/88/888999",
                "/pizza/33",
                "/99/33/66/44",
                "/The/Start/3333",
                "/taco/444",
                "/pizza/22",
                "/99/33/66/2333",
                "/The/Start/44",
            };


            // Create the locations
            DataSource_SacrumFileSystem.CreateLocations(validFromLocations, fs);
            DataSource_SacrumFileSystem.CreateResources(validFromSubResources, fs);

            var j = 0;
            foreach (var location in validFromLocations)
            {

                var operation = fs.CopyLocation(location, validToLocations[j]);
                Debug.Log("copy: " + location + " Operation: " + operation);

                Assert.IsTrue(operation);
                Assert.IsTrue(operation > 0);
                Assert.Null(operation.Error);

                Assert.IsTrue(Directory.Exists(fs.LocationToSystemPath(location)));
                Assert.IsTrue(Directory.Exists(fs.LocationToSystemPath(validToLocations[j])));

                j++;
            }


            // Check for the resources
            var k = 0;
            foreach(var resource in validFromSubResources)
            {
                Assert.IsTrue(File.Exists(fs.ResourceToSystemPath(resource)));
                Assert.IsTrue(File.Exists(fs.ResourceToSystemPath(validToSubResources[j])));

                k++;
            }

        }

        [Test]
        public void B2_CopyLocation_WhenFromResourceDontExist_MustReturnFalse()
        {
            var validFromLocations = new string[]
            {
                "/mouse/dog/cat/",
                "/jiraf/33/67/",
                "/34/66/99/99/",
                "/The/End/",
            };

            var validFromSubResources = new string[]
            {
                "/mouse/dog/cat/dd",
                "/jiraf/33/67/ee",
                "/34/66/99/99/ee",
                "/The/End/ee",
                "/mouse/dog/cat/eee/uu",
                "/jiraf/33/67/11/78",
                "/34/66/99/99/78/77",
                "/The/End/dd",
                "/mouse/dog/cat/dddd",
                "/jiraf/33/67/dd",
                "/34/66/99/99/78/7888/677",
                "/The/End/377/yuu/66",
                "/mouse/dog/cat/344/uii",
                "/jiraf/33/67/333",
                "/34/66/99/99/333",
                "/The/End/444",
                "/mouse/dog/cat/44/88/888999",
                "/jiraf/33/67/33",
                "/34/66/99/99/44",
                "/The/End/3333",
                "/mouse/dog/cat/444",
                "/jiraf/33/67/22",
                "/34/66/99/99/2333",
                "/The/End/44",
            };

            var validToLocations = new string[]
            {
                "/taco/",
                "/pizza/",
                "/99/33/66/",
                "/The/Start/",
            };

            var validToSubResources = new string[]
            {
                "/taco/dd",
                "/pizza/ee",
                "/99/33/66/ee",
                "/The/Start/ee",
                "/taco/eee/uu",
                "/pizza/11/78",
                "/99/33/66/78/77",
                "/The/Start/dd",
                "/taco/dddd",
                "/pizza/dd",
                "/99/33/66/78/7888/677",
                "/The/Start/377/yuu/66",
                "/taco/344/uii",
                "/pizza/333",
                "/99/33/66/333",
                "/The/Start/444",
                "/taco/44/88/888999",
                "/pizza/33",
                "/99/33/66/44",
                "/The/Start/3333",
                "/taco/444",
                "/pizza/22",
                "/99/33/66/2333",
                "/The/Start/44",
            };


            // Create the locations
            //DataSource_SacrumFileSystem.CreateLocations(validFromLocations, fileSystem);
            //DataSource_SacrumFileSystem.CreateResources(validFromSubResources, fileSystem);

            var j = 0;
            foreach (var location in validFromLocations)
            {

                var operation = fs.CopyLocation(location, validToLocations[j]);
                Debug.Log("copy: " + location + " Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<DirectoryNotFoundException>(() => throw operation.Error);

                Assert.IsFalse(Directory.Exists(fs.LocationToSystemPath(location)));
                Assert.IsFalse(Directory.Exists(fs.LocationToSystemPath(validToLocations[j])));

                j++;
            }


            // Check for the resources
            var k = 0;
            foreach (var resource in validFromSubResources)
            {
                Assert.IsFalse(File.Exists(fs.ResourceToSystemPath(resource)));
                Assert.IsFalse(File.Exists(fs.ResourceToSystemPath(validToSubResources[j])));

                k++;
            }

        }

        [Test]
        public void B3_CopyLocation_WhenToResourceAlreadyExist_MustReturnFalse()
        {
            var validFromLocations = new string[]
            {
                "/mouse/dog/cat/",
                "/jiraf/33/67/",
                "/34/66/99/99/",
                "/The/End/",
            };

            var validFromSubResources = new string[]
            {
                "/mouse/dog/cat/dd",
                "/jiraf/33/67/ee",
                "/34/66/99/99/ee",
                "/The/End/ee",
                "/mouse/dog/cat/eee/uu",
                "/jiraf/33/67/11/78",
                "/34/66/99/99/78/77",
                "/The/End/dd",
                "/mouse/dog/cat/dddd",
                "/jiraf/33/67/dd",
                "/34/66/99/99/78/7888/677",
                "/The/End/377/yuu/66",
                "/mouse/dog/cat/344/uii",
                "/jiraf/33/67/333",
                "/34/66/99/99/333",
                "/The/End/444",
                "/mouse/dog/cat/44/88/888999",
                "/jiraf/33/67/33",
                "/34/66/99/99/44",
                "/The/End/3333",
                "/mouse/dog/cat/444",
                "/jiraf/33/67/22",
                "/34/66/99/99/2333",
                "/The/End/44",
            };

            var validToLocations = new string[]
            {
                "/taco/",
                "/pizza/",
                "/99/33/66/",
                "/The/Start/",
            };

            var validToSubResources = new string[]
            {
                "/taco/dd",
                "/pizza/ee",
                "/99/33/66/ee",
                "/The/Start/ee",
                "/taco/eee/uu",
                "/pizza/11/78",
                "/99/33/66/78/77",
                "/The/Start/dd",
                "/taco/dddd",
                "/pizza/dd",
                "/99/33/66/78/7888/677",
                "/The/Start/377/yuu/66",
                "/taco/344/uii",
                "/pizza/333",
                "/99/33/66/333",
                "/The/Start/444",
                "/taco/44/88/888999",
                "/pizza/33",
                "/99/33/66/44",
                "/The/Start/3333",
                "/taco/444",
                "/pizza/22",
                "/99/33/66/2333",
                "/The/Start/44",
            };


            // Create the locations
            DataSource_SacrumFileSystem.CreateLocations(validFromLocations, fs);
            DataSource_SacrumFileSystem.CreateResources(validFromSubResources, fs);
            DataSource_SacrumFileSystem.CreateLocations(validToLocations, fs);
            DataSource_SacrumFileSystem.CreateResources(validToSubResources, fs);

            var j = 0;
            foreach (var location in validFromLocations)
            {

                var operation = fs.CopyLocation(location, validToLocations[j]);
                Debug.Log("copy: " + location + " Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<DuplicateNameException>(() => throw operation.Error);

                Assert.IsTrue(Directory.Exists(fs.LocationToSystemPath(location)));
                Assert.IsTrue(Directory.Exists(fs.LocationToSystemPath(validToLocations[j])));

                j++;
            }


            // Check for the resources
            var k = 0;
            foreach (var resource in validFromSubResources)
            {
                Assert.IsTrue(File.Exists(fs.ResourceToSystemPath(resource)));
                Assert.IsTrue(File.Exists(fs.ResourceToSystemPath(validToSubResources[j])));

                k++;
            }

        }

        [Test]
        public void B4_CopyLocation_WhenFromResourceIsInvalid_MustReturnFalse()
        {
            var validFromLocations = new string[]
            {
                "/mouse/dog/cat/",
                "/jiraf/33/67/",
                "/34/66/99/99/",
                "/The/End/",
            };

            var validFromSubResources = new string[]
            {
                "/mouse/dog/cat/dd",
                "/jiraf/33/67/ee",
                "/34/66/99/99/ee",
                "/The/End/ee",
                "/mouse/dog/cat/eee/uu",
                "/jiraf/33/67/11/78",
                "/34/66/99/99/78/77",
                "/The/End/dd",
                "/mouse/dog/cat/dddd",
                "/jiraf/33/67/dd",
                "/34/66/99/99/78/7888/677",
                "/The/End/377/yuu/66",
                "/mouse/dog/cat/344/uii",
                "/jiraf/33/67/333",
                "/34/66/99/99/333",
                "/The/End/444",
                "/mouse/dog/cat/44/88/888999",
                "/jiraf/33/67/33",
                "/34/66/99/99/44",
                "/The/End/3333",
                "/mouse/dog/cat/444",
                "/jiraf/33/67/22",
                "/34/66/99/99/2333",
                "/The/End/44",
            };

            var validToLocations = new string[]
            {
                "/taco/",
                "/pizza/",
                "/99/33/66/",
                "/The/Start/",
            };

            var validToSubResources = new string[]
            {
                "/taco/dd",
                "/pizza/ee",
                "/99/33/66/ee",
                "/The/Start/ee",
                "/taco/eee/uu",
                "/pizza/11/78",
                "/99/33/66/78/77",
                "/The/Start/dd",
                "/taco/dddd",
                "/pizza/dd",
                "/99/33/66/78/7888/677",
                "/The/Start/377/yuu/66",
                "/taco/344/uii",
                "/pizza/333",
                "/99/33/66/333",
                "/The/Start/444",
                "/taco/44/88/888999",
                "/pizza/33",
                "/99/33/66/44",
                "/The/Start/3333",
                "/taco/444",
                "/pizza/22",
                "/99/33/66/2333",
                "/The/Start/44",
            };



            var j = 0;
            foreach (var location in validFromLocations)
            {

                var operation = fs.CopyLocation(location + "##invalid##", validToLocations[j]);
                Debug.Log("copy: " + location + " Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<FormatException>(() => throw operation.Error);


                j++;
            }

        }

        [Test]
        public void B5_CopyLocation_WhenToResourceIsInvalid_MustReturnFalse()
        {
            var validFromLocations = new string[]
            {
                "/mouse/dog/cat/",
                "/jiraf/33/67/",
                "/34/66/99/99/",
                "/The/End/",
            };

            var validFromSubResources = new string[]
            {
                "/mouse/dog/cat/dd",
                "/jiraf/33/67/ee",
                "/34/66/99/99/ee",
                "/The/End/ee",
                "/mouse/dog/cat/eee/uu",
                "/jiraf/33/67/11/78",
                "/34/66/99/99/78/77",
                "/The/End/dd",
                "/mouse/dog/cat/dddd",
                "/jiraf/33/67/dd",
                "/34/66/99/99/78/7888/677",
                "/The/End/377/yuu/66",
                "/mouse/dog/cat/344/uii",
                "/jiraf/33/67/333",
                "/34/66/99/99/333",
                "/The/End/444",
                "/mouse/dog/cat/44/88/888999",
                "/jiraf/33/67/33",
                "/34/66/99/99/44",
                "/The/End/3333",
                "/mouse/dog/cat/444",
                "/jiraf/33/67/22",
                "/34/66/99/99/2333",
                "/The/End/44",
            };

            var validToLocations = new string[]
            {
                "/taco/",
                "/pizza/",
                "/99/33/66/",
                "/The/Start/",
            };

            var validToSubResources = new string[]
            {
                "/taco/dd",
                "/pizza/ee",
                "/99/33/66/ee",
                "/The/Start/ee",
                "/taco/eee/uu",
                "/pizza/11/78",
                "/99/33/66/78/77",
                "/The/Start/dd",
                "/taco/dddd",
                "/pizza/dd",
                "/99/33/66/78/7888/677",
                "/The/Start/377/yuu/66",
                "/taco/344/uii",
                "/pizza/333",
                "/99/33/66/333",
                "/The/Start/444",
                "/taco/44/88/888999",
                "/pizza/33",
                "/99/33/66/44",
                "/The/Start/3333",
                "/taco/444",
                "/pizza/22",
                "/99/33/66/2333",
                "/The/Start/44",
            };



            var j = 0;
            foreach (var location in validFromLocations)
            {

                var operation = fs.CopyLocation(location, validToLocations[j] + "##invalid##");
                Debug.Log("copy: " + location + " Operation: " + operation);

                Assert.IsFalse(operation);
                Assert.IsTrue(operation == 0);
                Assert.Throws<FormatException>(() => throw operation.Error);


                j++;
            }

        }

    }
}
