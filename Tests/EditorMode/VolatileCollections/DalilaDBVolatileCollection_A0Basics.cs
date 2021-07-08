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

public class DalilaVolatileCollection_A0Basics
{


    #region Example classes

    [KnownType(typeof(CollectionInDefaultPath))]
    [DataContract()]
    class CollectionInDefaultPath : DalilaDBVolatileCollection<CollectionInDefaultPath>
    {

        [DataMember()]
        public int count { get; set; }

    }

    [KnownType(typeof(CollectionInChangePath))]
    [DataContract()]
    class CollectionInChangePath : DalilaDBVolatileCollection<CollectionInChangePath>
    {

        [DataMember()]
        public int count { get; set; }

    }

    [KnownType(typeof(CollectionOne))]
    [DataContract()]
    class CollectionOne : DalilaDBVolatileCollection<CollectionOne>
    {

        protected override int cacheSize_ => 21;

        [DataMember()]
        public int count { get; set; }

    }

    [KnownType(typeof(CollectionTwo))]
    [DataContract()]
    class CollectionTwo : DalilaDBVolatileCollection<CollectionTwo>
    {

        protected override int cacheSize_ => 22;

        [DataMember()]
        public int count { get; set; }

    }

    [KnownType(typeof(CollectionThree))]
    [DataContract()]
    class CollectionThree : DalilaDBVolatileCollection<CollectionThree>
    {

        protected override int cacheSize_ => 23;

        [DataMember()]
        public int count { get; set; }

    }





    #endregion Example classes



    [SetUp]
    public void SetUp()
    {
        // Delete the directory to clear it
        CollectionOne.DeleteAll();
        CollectionTwo.DeleteAll();
        CollectionThree.DeleteAll();
        CollectionInChangePath.DeleteAll();
        CollectionInDefaultPath.DeleteAll();

    }


    [Test]
    public void DalilaVolatileCollection_100CountAndExist()
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
    public void DalilaVolatileCollection_200MultiInstancesWork()
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
    public void DalilaVolatileCollection_600Define()
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
    public void DalilaVolatileCollection_700CacheSize()
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
            var opp = new CollectionOne { count = 66, }.Save();
            Assert.IsFalse(opp); // Cant save if is full
            // Get again the cache
            var cache1 = CollectionOne.CacheGet();
            // Compare cache
            var cmp = S.DiferencesArrays(cache1, cache0);
            Debug.Log("Diferents: " + cmp.Length);
            Assert.AreEqual(0, cmp.Length);   // Cero becouse if is full dont save a new one
        }



    }



}