using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using U.DalilaDB;
using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Data;
using System.Threading.Tasks;
using System.Linq;

public class DalilaCollection_A0Basics
{


    #region Example classes

    [KnownType(typeof(CollectionInDefaultPath))]
    [DataContract()]
    class CollectionInDefaultPath : DalilaDBCollection<CollectionInDefaultPath>
    {

        [DataMember()]
        public int count { get; set; }

        protected override string rootPath_ => Application.persistentDataPath;
    }

    [KnownType(typeof(CollectionInChangePath))]
    [DataContract()]
    class CollectionInChangePath : DalilaDBCollection<CollectionInChangePath>
    {

        protected override string rootPath_ => Application.persistentDataPath + "/OtherPath";

        [DataMember()]
        public int count { get; set; }

    }

    [KnownType(typeof(CollectionOne))]
    [DataContract()]
    class CollectionOne : DalilaDBCollection<CollectionOne>
    {

        protected override string rootPath_ => Application.persistentDataPath;

        protected override int cacheSize_ => 21;

        [DataMember()]
        public int count { get; set; }

    }

    [KnownType(typeof(CollectionTwo))]
    [DataContract()]
    class CollectionTwo : DalilaDBCollection<CollectionTwo>
    {

        protected override string rootPath_ => Application.persistentDataPath;

        protected override int cacheSize_ => 22;

        [DataMember()]
        public int count { get; set; }

    }

    [KnownType(typeof(CollectionThree))]
    [DataContract()]
    class CollectionThree : DalilaDBCollection<CollectionThree>
    {

        protected override string rootPath_ => Application.persistentDataPath;

        protected override int cacheSize_ => 23;

        [DataMember()]
        public int count { get; set; }

    }





    #endregion Example classes



    [SetUp]
    public void SetUp()
    {
        // Delete the directory to clear it
        try { Directory.Delete(CollectionOne.LocationPath, true); } catch (Exception) { }
        try { Directory.Delete(CollectionTwo.LocationPath, true); } catch (Exception) { }
        try { Directory.Delete(CollectionThree.LocationPath, true); } catch (Exception) { }
        try { Directory.Delete(CollectionInDefaultPath.LocationPath, true); } catch (Exception) { }
        try { Directory.Delete(CollectionInChangePath.LocationPath, true); } catch (Exception) { }
        CollectionOne.DeleteAll();
        CollectionTwo.DeleteAll();
        CollectionThree.DeleteAll();
        CollectionInChangePath.DeleteAll();
        CollectionInDefaultPath.DeleteAll();

    }




    public void DalilaCollection_000PathsAndLocations()
    {

        // Check RootPath
        Debug.Log("Default Root: " + CollectionInDefaultPath.RootPath);
        Debug.Log("Change  Root: " + CollectionInChangePath.RootPath);
        Assert.IsTrue(CollectionInDefaultPath.RootPath == Application.persistentDataPath + "/");
        Assert.IsTrue(CollectionInChangePath.RootPath == Application.persistentDataPath + "/OtherPath/");

        // Check Location
        Debug.Log("Default Location" + CollectionInDefaultPath.Location);
        Debug.Log("Change Location" + CollectionInChangePath.Location);
        Assert.IsTrue(CollectionInDefaultPath.Location == "/DalilaDB/DalilaCollections/CollectionInDefaultPath/");
        Assert.IsTrue(CollectionInChangePath.Location == "/DalilaDB/DalilaCollections/CollectionInChangePath/");

        // Check ResourceLocation giving a SID
        Debug.Log("Default ResourceLocation giving SID: " + CollectionInDefaultPath.ResourceLocation("4961429416443432069"));
        Debug.Log("Change  ResourceLocation giving SID: " + CollectionInDefaultPath.ResourceLocation("4961429416443432069"));
        Assert.IsTrue(CollectionInDefaultPath.ResourceLocation("4961429416443432069") == "/DalilaDB/DalilaCollections/CollectionInDefaultPath/4961429416443432069.xml");
        Assert.IsTrue(CollectionInChangePath.ResourceLocation("4961429416443432069") == "/DalilaDB/DalilaCollections/CollectionInChangePath/4961429416443432069.xml");

        // Check ResourceLocation giving a document
        var document1 = new CollectionInDefaultPath { _id = "4961429416443432000" };
        var document1c = new CollectionInChangePath { _id = "4961429416443432000" };
        Debug.Log("Default ResourceLocation giving a document: " + CollectionInDefaultPath.ResourceLocation(document1));
        Debug.Log("Change  ResourceLocation giving a document:" + CollectionInChangePath.ResourceLocation(document1c));
        Assert.IsTrue(CollectionInDefaultPath.ResourceLocation(document1) == "/DalilaDB/DalilaCollections/CollectionInDefaultPath/4961429416443432000.xml");
        Assert.IsTrue(CollectionInChangePath.ResourceLocation(document1c) == "/DalilaDB/DalilaCollections/CollectionInChangePath/4961429416443432000.xml");

        // Check LocationPath
        Debug.Log("Default LocationPath: " + CollectionInDefaultPath.LocationPath);
        Debug.Log("Change  LocationPath: " + CollectionInChangePath.LocationPath);
        Assert.IsTrue(CollectionInDefaultPath.LocationPath == Application.persistentDataPath + "/DalilaDB/DalilaCollections/CollectionInDefaultPath/");
        Assert.IsTrue(CollectionInChangePath.LocationPath == Application.persistentDataPath + "/OtherPath/DalilaDB/DalilaCollections/CollectionInChangePath/");

        // Check ResourcePath giving a SID
        Debug.Log("Default ResourcePath giving a SID: " + CollectionInDefaultPath.ResourcePath("4961429416443432069"));
        Debug.Log("Change  ResourcePath giving a SID: " + CollectionInChangePath.ResourcePath("4961429416443432069"));
        Assert.IsTrue(CollectionInDefaultPath.ResourcePath("4961429416443432069") == Application.persistentDataPath + "/DalilaDB/DalilaCollections/CollectionInDefaultPath/4961429416443432069.xml");
        Assert.IsTrue(CollectionInChangePath.ResourcePath("4961429416443432069") == Application.persistentDataPath + "/OtherPath/DalilaDB/DalilaCollections/CollectionInChangePath/4961429416443432069.xml");

        // Check ResourcePath giving a document
        var document2 = new CollectionInDefaultPath { _id = "4961429416443432001" };
        var document2c = new CollectionInChangePath { _id = "4961429416443432001" };
        Debug.Log("Default ResourcePath giving a document: " + CollectionInDefaultPath.ResourcePath(document2));
        Debug.Log("Change  ResourcePath giving a document: " + CollectionInChangePath.ResourcePath(document2c));
        Assert.IsTrue(CollectionInDefaultPath.ResourcePath(document2) == Application.persistentDataPath + "/DalilaDB/DalilaCollections/CollectionInDefaultPath/4961429416443432001.xml");
        Assert.IsTrue(CollectionInChangePath.ResourcePath(document2c) == Application.persistentDataPath + "/OtherPath/DalilaDB/DalilaCollections/CollectionInChangePath/4961429416443432001.xml");

    }


    [Test]
    public void DalilaCollection_100CountAndExist()
    {

        // When no resources saved

        // Count
        Assert.AreEqual(0, CollectionInDefaultPath.Count());

        // Exist with parameter
        Assert.IsFalse(CollectionInDefaultPath.Exist("4961429416443432069"));
        Assert.IsFalse(CollectionInDefaultPath.Exist("4961429416443432068"));
        Assert.IsFalse(CollectionInDefaultPath.Exist("4961429416443432062"));
        Assert.IsFalse(CollectionInDefaultPath.Exist("4961429416443432061"));
        Assert.IsFalse(CollectionInDefaultPath.Exist("4961429416443432233"));

        // Exist without  parameter
        var keys = CollectionInDefaultPath.Exist().ToList();

        foreach (var key in keys)
        {
            Debug.Log(key);
        }

        // Orther is not defined
        Assert.AreEqual(0, keys.Count);





        // When resources saved

        // Create a new doocument
        Debug.Log("Save");
        new CollectionInDefaultPath { _id = "4961429416443432069", count = 66, }.Save();
        new CollectionInDefaultPath { _id = "4961429416443432068", count = 77, }.Save();
        new CollectionInDefaultPath { _id = "4961429416443432062", count = 88, }.Save();
        new CollectionInDefaultPath { _id = "4961429416443432061", count = 99, }.Save();
        new CollectionInDefaultPath { _id = "4961429416443432233", count = 99, }.Save();

        // Count
        Assert.AreEqual(5, CollectionInDefaultPath.Count());

        // Exist with parameter
        Assert.AreEqual(true, CollectionInDefaultPath.Exist("4961429416443432069"));
        Assert.AreEqual(true, CollectionInDefaultPath.Exist("4961429416443432068"));
        Assert.AreEqual(true, CollectionInDefaultPath.Exist("4961429416443432062"));
        Assert.AreEqual(true, CollectionInDefaultPath.Exist("4961429416443432061"));
        Assert.AreEqual(true, CollectionInDefaultPath.Exist("4961429416443432233"));

        // Exist without  parameter
        keys = CollectionInDefaultPath.Exist().ToList();

        foreach (var key in keys)
        {
            Debug.Log(key);
        }

        // Orther is not defined
        Assert.IsTrue(keys.Contains("4961429416443432069"));
        Assert.IsTrue(keys.Contains("4961429416443432068"));
        Assert.IsTrue(keys.Contains("4961429416443432062"));
        Assert.IsTrue(keys.Contains("4961429416443432061"));
        Assert.IsTrue(keys.Contains("4961429416443432233"));

    }

    [Test]
    public void DalilaCollection_200MultiInstancesWork()
    {

        // HotCollectionMax
        Debug.Log("HotCollectionMax");
        Debug.Log("One: " + CollectionOne.CacheSize);
        Assert.IsTrue(CollectionOne.CacheSize == 21);
        Debug.Log("Two: " + CollectionTwo.CacheSize);
        Assert.IsTrue(CollectionTwo.CacheSize == 22);
        Debug.Log("Three: " + CollectionThree.CacheSize);
        Assert.IsTrue(CollectionThree.CacheSize == 23);


        // Create a element with same key but diferent value
        new CollectionOne { count = 1, _id = "0000000000000000000" }.Save();
        new CollectionTwo { count = 2, _id = "0000000000000000000" }.Save();
        new CollectionThree { count = 3, _id = "0000000000000000000" }.Save();

        // Find the diferent Collection
        var dataOne = CollectionOne.FindById("0000000000000000000");
        Debug.Log("FindedOne: " + dataOne);
        Assert.AreEqual(1, dataOne.Data.count);

        var dataTwo = CollectionTwo.FindById("0000000000000000000");
        Debug.Log("FindedTwo: " + dataTwo);
        Assert.AreEqual(2, dataTwo.Data.count);

        var dataThree = CollectionThree.FindById("0000000000000000000");
        Debug.Log("FindedThree: " + dataThree);
        Assert.AreEqual(3, dataThree.Data.count);


        // Delete in One dont delete others
        CollectionOne.DeleteById("0000000000000000000");
        Assert.IsFalse(CollectionOne.Exist("0000000000000000000"));
        Assert.IsTrue(CollectionTwo.Exist("0000000000000000000"));
        Assert.IsTrue(CollectionThree.Exist("0000000000000000000"));

    }




    [Test]
    public void DalilaCollection_600Define()
    {

        // When no resources saved
        CollectionInDefaultPath.Define();


        // Create a new doocument
        Debug.Log("Save");
        new CollectionInDefaultPath { _id = "4961429416443432069", count = 66, }.Save();
        new CollectionInDefaultPath { _id = "4961429416443432068", count = 77, }.Save();
        new CollectionInDefaultPath { _id = "4961429416443432062", count = 88, }.Save();
        new CollectionInDefaultPath { _id = "4961429416443432061", count = 99, }.Save();
        new CollectionInDefaultPath { _id = "4961429416443432233", count = 99, }.Save();


        // When resources saved
        CollectionInDefaultPath.Define();


    }





    [Test]
    public void DalilaCollection_700CacheSize()
    {

        // When no resources saved

        // Save 21 that are the full size of cache
        Debug.Log("Save");
        for (int i = 0; i < 21; i++)
        {
            new CollectionOne { count = i, }.Save();
        }


        // Save when cache is full will remove one element and add the new one
        for (int i = 0; i < 10; i++)
        {
            // When resources saved
            // Get the list of strings
            var cache0 = CollectionOne.CacheGet();
            // Save a new element
            new CollectionOne { count = 66, }.Save();
            // Get again the cache
            var cache1 = CollectionOne.CacheGet();
            // Compare cache
            var cmp = S.DiferencesArrays(cache1, cache0);
            Debug.Log("Diferents: " + cmp.Length);
            foreach (var item in cmp)
            {
                Debug.Log(item);
            }
            Assert.AreEqual(2, cmp.Length);   // Two becouse one is removed and one is added
        }



    }


}