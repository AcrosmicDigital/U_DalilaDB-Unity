using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using U.DalilaDB;
using System.IO;

namespace DalilaFsTests
{
    public class DalilaFS_Operations_X8GetInfo
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
        public void A1_GetLocations_WhenValidLocation_WillReturnAllLocationsAndSubLocations()
        {
            var locations = fs.GetLocations("/");

            foreach(var location in locations)
            {
                Debug.Log(location);
            }
        }

        [Test]
        public void B1_GetResources_WhenValidLocation_WillReturnAllResourcesAndSubresources()
        {
            var resources = fs.GetResources("/");

            foreach (var resource in resources)
            {
                Debug.Log(resource);
            }
        }

        [Test]
        public void C1_GetFileSystem_WhenValidLocation_WillReturnAllResourcesAndLocations()
        {
            var fileSystemLocAndRes = this.fs.GetFileSystem("/");

            foreach (var pair in fileSystemLocAndRes)
            {
                Debug.Log("----> " + pair.Key);

                foreach(var item in pair.Value)
                {
                    Debug.Log(item);
                }
            }
        }


        [Test]
        public void A2_GetLocations_WhenInvalidLocation_WillReturnEmptyArray()
        {
            var locations = fs.GetLocations("/###");

            Assert.IsTrue(locations.Length == 0);
        }

        [Test]
        public void B2_GetResources_WhenInvalidLocation_WillReturnEmptyArray()
        {
            var resources = fs.GetResources("/##");

            Assert.IsTrue(resources.Length == 0);
        }

        [Test]
        public void C2_GetFileSystem_WhenInvalidLocation_WillReturnEmptyArrays()
        {
            var fileSystemLocAndRes = this.fs.GetFileSystem("/##");

            foreach (var pair in fileSystemLocAndRes)
            {
                Assert.IsTrue(pair.Value.Length == 0);
            }
        }


        [Test]
        public void A3_GetLocations_WhenValidLocationAndSearchPattern_WillReturnAllLocationsAndSubLocationsWithThaPattern()
        {
            var locations = fs.GetLocations("/", "a*");

            foreach (var location in locations)
            {
                Debug.Log(location);
            }
        }

        [Test]
        public void B3_GetResources_WhenValidLocationAndSearchPattern_WillReturnAllResourcesAndSubResourcesWithThaPattern()
        {
            var resources = fs.GetResources("/", "*.4");

            foreach (var resource in resources)
            {
                Debug.Log(resource);
            }
        }

        [Test]
        public void C3_GetFileSystem_WhenValidLocationAndSearchPattern_WillReturnAllResourcesAndLocationsWithThatPattern()
        {
            var fileSystemLocAndRes = this.fs.GetFileSystem("/",".*");

            foreach (var pair in fileSystemLocAndRes)
            {
                Debug.Log("----> " + pair.Key);

                foreach (var item in pair.Value)
                {
                    Debug.Log(item);
                }
            }
        }


    }
}
