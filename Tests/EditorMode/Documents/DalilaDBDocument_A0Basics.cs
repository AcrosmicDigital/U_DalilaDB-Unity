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

public class DalilaDocument_A0Basics
{


    #region Example classes

    [KnownType(typeof(DocumentInDefaultPath))]
    [DataContract()]
    class DocumentInDefaultPath : DalilaDBDocument<DocumentInDefaultPath>
    {

        [DataMember()]
        public int count;

        protected override string rootPath_ => Application.persistentDataPath;

    }

    [KnownType(typeof(DocumentInChangePath))]
    [DataContract()]
    class DocumentInChangePath : DalilaDBDocument<DocumentInChangePath>
    {

        protected override string rootPath_ => Application.persistentDataPath + "/OtherPath";

        [DataMember()]
        public int count;

    }

    [KnownType(typeof(DocumentOne))]
    [DataContract()]
    class DocumentOne : DalilaDBDocument<DocumentOne>
    {

        protected override string rootPath_ => Application.persistentDataPath;

        protected override bool cacheSize_ => true;

        [DataMember()]
        public int count;

    }

    [KnownType(typeof(DocumentTwo))]
    [DataContract()]
    class DocumentTwo : DalilaDBDocument<DocumentTwo>
    {

        protected override string rootPath_ => Application.persistentDataPath;

        protected override bool cacheSize_ => false;

        [DataMember()]
        public int count;

    }

    [KnownType(typeof(DocumentThree))]
    [DataContract()]
    class DocumentThree : DalilaDBDocument<DocumentThree>
    {

        protected override string rootPath_ => Application.persistentDataPath;

        protected override bool cacheSize_ => true;

        [DataMember()]
        public int count;

    }


    #endregion Example classes



    [SetUp]
    public void SetUp()
    {
        // Delete the directory to clear it
        try { Directory.Delete(DocumentInDefaultPath.LocationPath, true); } catch (Exception) { }
        try { Directory.Delete(DocumentInChangePath.LocationPath, true); } catch (Exception) { }
        DocumentInDefaultPath.Delete();
        DocumentInChangePath.Delete();

    }




    [TestCase()]
    [TestCase()]
    public void DalilaDocument_000PathsAndLocations()
    {

        // Check RootPath
        Debug.Log("Default RootPathReal: " + DocumentInDefaultPath.RootPath);
        Debug.Log("Default RootPathExp : " + Application.persistentDataPath + "/");
        Debug.Log("Change  RootPathReal: " + DocumentInChangePath.RootPath);
        Debug.Log("Change  RootPathExp : " + Application.persistentDataPath + "/OtherPath/");
        Assert.IsTrue(DocumentInDefaultPath.RootPath == Application.persistentDataPath + "/");
        Assert.IsTrue(DocumentInChangePath.RootPath == Application.persistentDataPath + "/OtherPath/");

        // Check Location
        Debug.Log("Default Location: " + DocumentInDefaultPath.Location);
        Debug.Log("Change  Location: " + DocumentInChangePath.Location);
        Assert.IsTrue(DocumentInDefaultPath.Location == "/DalilaDB/DalilaDocuments/DalilaDocument_A0Basics.DocumentInDefaultPath/");
        Assert.IsTrue(DocumentInChangePath.Location == "/DalilaDB/DalilaDocuments/DalilaDocument_A0Basics.DocumentInChangePath/");

        // Check ResourceLocation
        Debug.Log("Default ResourceLocation: " + DocumentInDefaultPath.ResourceLocation);
        Debug.Log("Change  ResourceLocation: " + DocumentInChangePath.ResourceLocation);
        Assert.IsTrue(DocumentInDefaultPath.ResourceLocation == "/DalilaDB/DalilaDocuments/DalilaDocument_A0Basics.DocumentInDefaultPath/DocumentInDefaultPath.xml");
        Assert.IsTrue(DocumentInChangePath.ResourceLocation == "/DalilaDB/DalilaDocuments/DalilaDocument_A0Basics.DocumentInChangePath/DocumentInChangePath.xml");

        // Check LocationPath
        Debug.Log("Default LocationPath: " + DocumentInDefaultPath.LocationPath);
        Debug.Log("Change  LocationPath: " + DocumentInChangePath.LocationPath);
        Assert.IsTrue(DocumentInDefaultPath.LocationPath == Application.persistentDataPath + "/DalilaDB/DalilaDocuments/DalilaDocument_A0Basics.DocumentInDefaultPath/");
        Assert.IsTrue(DocumentInChangePath.LocationPath == Application.persistentDataPath + "/OtherPath/DalilaDB/DalilaDocuments/DalilaDocument_A0Basics.DocumentInChangePath/");

        // Check ResourcePath
        Debug.Log("Default ResourcePath: " + DocumentInDefaultPath.ResourcePath);
        Debug.Log("Change  ResourcePath: " + DocumentInChangePath.ResourcePath);
        Assert.IsTrue(DocumentInDefaultPath.ResourcePath == Application.persistentDataPath + "/DalilaDB/DalilaDocuments/DalilaDocument_A0Basics.DocumentInDefaultPath/DocumentInDefaultPath.xml");
        Assert.IsTrue(DocumentInChangePath.ResourcePath == Application.persistentDataPath + "/OtherPath/DalilaDB/DalilaDocuments/DalilaDocument_A0Basics.DocumentInChangePath/DocumentInChangePath.xml");

    }


    [Test]
    public void DalilaDocument_100CountAndExist()
    {

        // When no resources saved

        // Exist with no parameter
        Assert.IsFalse(DocumentInDefaultPath.Exist());



        // When resources saved

        // Create a new doocument
        Debug.Log("Save");
        new DocumentInDefaultPath { count = 66, }.Save();

        // Exist with no parameter
        Assert.IsTrue(DocumentInDefaultPath.Exist());
    }


    [Test]
    public void DalilaDocument_200MultiInstancesWork()
    {

        // HotCollectionMax
        Debug.Log("HotCollectionMax");
        Debug.Log("One: " + DocumentOne.CacheSize);
        Assert.IsTrue(DocumentOne.CacheSize);
        Debug.Log("Two: " + DocumentTwo.CacheSize);
        Assert.IsFalse(DocumentTwo.CacheSize);
        Debug.Log("Three: " + DocumentThree.CacheSize);
        Assert.IsTrue(DocumentThree.CacheSize);


        // Create a element with same key but diferent value
        new DocumentOne { count = 1, }.Save();
        new DocumentTwo { count = 2, }.Save();
        new DocumentThree { count = 3, }.Save();

        // Find the diferent Collection
        var dataOne = DocumentOne.Find();
        Debug.Log("FindedOne: " + dataOne);
        Assert.AreEqual(1, dataOne.Data.count);

        var dataTwo = DocumentTwo.Find();
        Debug.Log("FindedTwo: " + dataTwo);
        Assert.AreEqual(2, dataTwo.Data.count);

        var dataThree = DocumentThree.Find();
        Debug.Log("FindedThree: " + dataThree);
        Assert.AreEqual(3, dataThree.Data.count);


        // Delete in One dont delete others
        DocumentOne.Delete();
        Assert.IsFalse(DocumentOne.Exist());
        Assert.IsTrue(DocumentTwo.Exist());
        Assert.IsTrue(DocumentThree.Exist());

    }


    [Test]
    public void DalilaDocument_600Define()
    {

        // When no resources saved
        DocumentInDefaultPath.Define();

        // Create a new doocument
        Debug.Log("Save");
        new DocumentInDefaultPath { count = 66, }.Save();

        // When resources saved
        DocumentInDefaultPath.Define();

    }



    [Test]
    public void DalilaDocument_700CacheSize()
    {

    }


}
