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
    public class DalilaFS_Operations_X5DeleteEmptyLocations
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
        public void A1_DeleteEmpties_WhenSubLocationsExistAndAreEmpty_WillDeleteAllAndReturnTrueExeptForRoot()
        {
            var validLocations = new string[]
            {
                "/cat/dog/lion/mouse/",
                "/dog/lion/mouse/",
                "/lion/66/77/88/99/",
                "/cat/288/23/dog/",
                "/",
            };


            // Create the locations
            DataSource_SacrumFileSystem.CreateLocations(validLocations, fs);


            // Delete the locations
            foreach (var location in validLocations)
            {
                if (location == "/") continue;

                Debug.Log("DeleteEmpties: " + location);
                var operation = fs.DeleteEmptyLocations(location);
                Assert.IsTrue(operation);
                Assert.Null(operation.Error);
                Assert.IsFalse(Directory.Exists(fs.LocationToSystemPath(location)));
            }


            // Delete the locations
            DataSource_SacrumFileSystem.DeleteLocations(validLocations, fs);

        }

        [Test]
        public void A2_DeleteEmpties_WhenSubLocationsExistAndAreNotEmpty_WillNotDeleteAndReturnTrue()
        {
            var validLocations = new string[]
            {
                "/cat/dog/lion/mouse/",
                "/dog/lion/mouse/",
                "/lion/66/77/88/99/",
                "/cat/288/23/dog/",
            };

            var validLocationsTwo = new string[]
            {
                "/cat/dog/lion/mouse/cat/",
                "/dog/lion/mouse/dog/",
                "/lion/66/77/88/99/22/",
                "/cat/288/23/dog/11/",
            };


            // Create the locations
            DataSource_SacrumFileSystem.CreateLocations(validLocations, fs);
            DataSource_SacrumFileSystem.CreateLocations(validLocationsTwo, fs);


            // Delete the locations
            foreach (var location in validLocations)
            {
                
                Debug.Log("DeleteEmpties: " + location);
                var operation = fs.DeleteEmptyLocations(location);
                Assert.IsTrue(operation);
                Assert.Null(operation.Error);
                //Assert.Throws<ResourceOrLocationDoesntExistsException>(() => throw operation.Error);
                Assert.IsTrue(Directory.Exists(fs.LocationToSystemPath(location)));
            }


            // Delete the locations
            DataSource_SacrumFileSystem.DeleteLocations(validLocations, fs);
            DataSource_SacrumFileSystem.DeleteLocations(validLocationsTwo, fs);

        }

        [Test]
        public void A3_DeleteEmpties_WhenSubLocationsExistAndAreNotEmptyButWithFiles_WillNotDeleteAndReturnTrue()
        {
            var validLocations = new string[]
            {
                "/cat/dog/lion/mouse/",
                "/dog/lion/mouse/",
                "/lion/66/77/88/99/",
                "/cat/288/23/dog/",
            };

            var validResources = new string[]
            {
                "/cat/dog/lion/mouse/cat",
                "/dog/lion/mouse/dog",
                "/lion/66/77/88/99/22",
                "/cat/288/23/dog/11",
            };


            // Create the locations
            DataSource_SacrumFileSystem.CreateLocations(validLocations, fs);
            DataSource_SacrumFileSystem.CreateResources(validResources, fs);


            // Delete the locations
            foreach (var location in validLocations)
            {

                Debug.Log("DeleteEmpties: " + location);
                var operation = fs.DeleteEmptyLocations(location);
                Assert.IsTrue(operation);
                Assert.Null(operation.Error);
                //Assert.Throws<ResourceOrLocationDoesntExistsException>(() => throw operation.Error);
                Assert.IsTrue(Directory.Exists(fs.LocationToSystemPath(location)));
            }


            // Delete the locations
            DataSource_SacrumFileSystem.DeleteLocations(validLocations, fs);
            DataSource_SacrumFileSystem.DeleteResources(validResources, fs);

        }

        [Test]
        public void A4_DeleteEmpties_WhenSubLocationsExistAndAreNotEmptyAtAll_WillDeleteOnlyEmptiesAndReturnTrue()
        {
            var validLocations = new string[]
            {
                "/cat/dog/lion/mouse/",
                "/dog/lion/mouse/",
                "/lion/66/77/88/99/",
                "/cat/288/23/dog/",
            };

            var validLocationsTwo = new string[]
            {
                "/cat/dog/cat/",
                "/dog/lion/dog/",
                "/lion/66/77/22/",
                "/cat/288/23/22/",
            };


            // Create the locations
            DataSource_SacrumFileSystem.CreateLocations(validLocations, fs);
            DataSource_SacrumFileSystem.CreateLocations(validLocationsTwo, fs);


            // Delete the locations
            int i = 0;
            foreach (var location in validLocations)
            {
                Debug.Log("DeleteEmpties: " + location);
                var operation = fs.DeleteEmptyLocations(location);
                Assert.IsTrue(operation);
                Assert.Null(operation.Error);
                //Assert.Throws<ResourceOrLocationDoesntExistsException>(() => throw operation.Error);
                Assert.IsFalse(Directory.Exists(fs.LocationToSystemPath(location)));
                Assert.IsTrue(Directory.Exists(fs.LocationToSystemPath(validLocationsTwo[i])));
                i++;
            }


            // Delete the locations
            DataSource_SacrumFileSystem.DeleteLocations(validLocations, fs);
            DataSource_SacrumFileSystem.DeleteLocations(validLocationsTwo, fs);

        }

        [Test]
        public void A4_DeleteEmpties_WhenSubLocationsExistAndAreNotEmptyAtAllButResources_WillDeleteOnlyEmptiesAndReturnTrue()
        {
            var validLocations = new string[]
            {
                "/cat/dog/lion/mouse/",
                "/dog/lion/mouse/",
                "/lion/66/77/88/99/",
                "/cat/288/23/dog/",
            };

            var validResources = new string[]
            {
                "/cat/dog/cat",
                "/dog/lion/dog",
                "/lion/66/77/22",
                "/cat/288/23/22",
            };


            // Create the locations
            DataSource_SacrumFileSystem.CreateLocations(validLocations, fs);
            DataSource_SacrumFileSystem.CreateResources(validResources, fs);


            // Delete the locations
            int i = 0;
            foreach (var location in validLocations)
            {
                Debug.Log("DeleteEmpties: " + location);
                var operation = fs.DeleteEmptyLocations(location);
                Assert.IsTrue(operation);
                Assert.Null(operation.Error);
                //Assert.Throws<ResourceOrLocationDoesntExistsException>(() => throw operation.Error);
                Assert.IsFalse(Directory.Exists(fs.LocationToSystemPath(location)));
                Assert.IsTrue(File.Exists(fs.ResourceToSystemPath(validResources[i])));
                i++;
            }


            // Delete the locations
            DataSource_SacrumFileSystem.DeleteLocations(validLocations, fs);
            DataSource_SacrumFileSystem.DeleteResources(validResources, fs);

        }

        [Test]
        public void A4_DeleteEmpties_WhenInvalidLocationPasses_WillReturnFalse()
        {
            foreach (var location in DataSource_SacrumFileSystem.validLocations)
            {
                Debug.Log("Delete: " + location + "%%invalid))");

                var operation = fs.DeleteEmptyLocations(location + "%%invalid))");
                Assert.IsFalse(operation);
                Assert.Throws<FormatException>(() => throw operation.Error);

            }
        }

    }
}
