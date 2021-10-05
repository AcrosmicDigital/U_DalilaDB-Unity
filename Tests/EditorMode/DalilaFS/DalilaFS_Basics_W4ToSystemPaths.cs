using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using U.DalilaDB;
using System.Linq;
using System.IO;
using System;

namespace DalilaFsTests
{
    public class DalilaFS_Basics_W4ToSystemPaths
    {

        string rootPath = Application.persistentDataPath + "/ceewc34r34f3446778";

        DalilaFS fs;

        [SetUp]
        public void SetUp()
        {
            // Create the auxiliar path and filesystem
            Directory.CreateDirectory(rootPath);
            fs = new DalilaFS(rootPath);
        }

        [TearDown]
        public void TearDown()
        {
            // Delete the auxiliar path
            Directory.Delete(rootPath, true);
        }
        



        [Test]
        public void A1_LocationToSystemPath_WhenValidLocationsPasses_WillReturnTheFullPath()
        {

            foreach (var location in DataSource_SacrumFileSystem.validLocations)
            {
                Debug.Log(fs.LocationToSystemPath(location));
                Assert.IsTrue(fs.LocationToSystemPath(location).EndsWith("/"));
            }

        }

        [Test]
        public void A2_LocationToSystemPath_2WhenInvalidLocationsPasses_MustThrowAnException()
        {

            foreach (var location in DataSource_SacrumFileSystem.invalidlocations)
            {
                Debug.Log("inavlid location is: " + location);
                Assert.Throws<FormatException>(() => fs.LocationToSystemPath(location));
            }

        }

        
        [Test]
        public void B1_ResourceToSystemPath_WhenValidResourcePasses_WillReturnTheFullPath()
        {

            foreach (var resource in DataSource_SacrumFileSystem.validResources)
            {
                Debug.Log(fs.ResourceToSystemPath(resource));
                Assert.IsFalse(fs.ResourceToSystemPath(resource).EndsWith("/"));
            }

        }

        [Test]

        public void B2_ResourceToSystemPath_WhenInvalidResourcePasses_MustThrowAnException()
        {

            foreach (var resource in DataSource_SacrumFileSystem.invalidResources)
            {
                Assert.Throws<FormatException>(() => fs.ResourceToSystemPath(resource));
            }

        }




        [Test]
        public void C1_LocationFromSystemPath_WhenValidPathPasses_WillReturnTheLocation()
        {
            
            // Creates the array of locations
            var systemPaths = DataSource_SacrumFileSystem.validLocations.Select(location => rootPath + location);


            foreach (var path in systemPaths)
            {
                Debug.Log("Path: " + path);
                Debug.Log("Location: " + fs.LocationFromSystemPath(path));
            }


            // Check for paths with backslashes
            var systemPathsBackSlash = systemPaths.Select(path => path.Replace('/', '\\'));
            foreach (var path in systemPathsBackSlash)
            {
                Debug.Log("Path: " + path);
                Debug.Log("Location: " + fs.LocationFromSystemPath(path));
            }

        }

        [Test]
        public void C2_LocationFromSystemPath_WhenInvalidPathPasses_MustThrowAnException()
        {
            // Creates the array of locations
            var systemPaths = DataSource_SacrumFileSystem.validLocations.Select(location => rootPath + location + "##invalid##");


            foreach (var path in systemPaths)
            {
                Debug.Log("Path: " + path);
                Assert.Throws<FormatException>(() => fs.LocationFromSystemPath(path));
            }

        }


        [Test]
        public void D1_ResourceFromSystemPath_WhenValidPathPasses_WillReturnTheResource()
        {
            
            // Creates the array of locations
            var systemPaths = DataSource_SacrumFileSystem.validResources.Select(location => rootPath + location);


            foreach (var path in systemPaths)
            {
                Debug.Log("Path: " + path);
                Debug.Log("Resource: " + fs.ResourceFromSystemPath(path));
            }


            // Check for paths with backslashes
            var systemPathsBackSlash = systemPaths.Select(path => path.Replace('/', '\\'));
            foreach (var path in systemPathsBackSlash)
            {
                Debug.Log("Path: " + path);
                Debug.Log("Resource: " + fs.ResourceFromSystemPath(path));
            }

        }

        [Test]
        public void D2_ResourceFromSystemPath_WhenInvalidPathPasses_MustThrowAnException()
        {

            // Creates the array of invalid locations
            var systemPaths = DataSource_SacrumFileSystem.validResources.Select(location => rootPath + location + "##invalid##");


            foreach (var path in systemPaths)
            {
                Debug.Log("Path: " + path);
                Assert.Throws<FormatException>(() => fs.ResourceFromSystemPath(path));
            }

        }

    }
}
