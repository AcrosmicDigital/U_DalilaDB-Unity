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

public class DalilaVolatileDocument_A0Basics
{


    #region Example classes

    [KnownType(typeof(DocumentInDefaultPath))]
    [DataContract()]
    class DocumentInDefaultPath : DalilaDBVolatileDocument<DocumentInDefaultPath>
    {

        [DataMember()]
        public int count;


    }

    [KnownType(typeof(DocumentInChangePath))]
    [DataContract()]
    class DocumentInChangePath : DalilaDBVolatileDocument<DocumentInChangePath>
    {


        [DataMember()]
        public int count;

    }

    [KnownType(typeof(DocumentOne))]
    [DataContract()]
    class DocumentOne : DalilaDBVolatileDocument<DocumentOne>
    {


        [DataMember()]
        public int count;

    }

    [KnownType(typeof(DocumentTwo))]
    [DataContract()]
    class DocumentTwo : DalilaDBVolatileDocument<DocumentTwo>
    {

        [DataMember()]
        public int count;

    }

    [KnownType(typeof(DocumentThree))]
    [DataContract()]
    class DocumentThree : DalilaDBVolatileDocument<DocumentThree>
    {

        [DataMember()]
        public int count;

    }


    #endregion Example classes



    [SetUp]
    public void SetUp()
    {
        // Delete the directory to clear it
        DocumentInDefaultPath.Delete();
        DocumentInChangePath.Delete();

    }




    [Test]
    public void DalilaVolatileDocument_100CountAndExist()
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
    public void DalilaVolatileDocument_200MultiInstancesWork()
    {

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
    public void DalilaVolatileDocument_600Define()
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
    public void DalilaVolatileDocument_700CacheSize()
    {

    }


}
