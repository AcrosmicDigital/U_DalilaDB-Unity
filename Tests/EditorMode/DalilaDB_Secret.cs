﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using U.DalilaDB;
using System;
using System.IO;
using System.Runtime.Serialization;

public class CustomType_Secret
{

    #region Example classes


    class HashedElementsStore : DalilaDBElements<HashedElementsStore>
    {
        protected override string rootPath_ => Application.persistentDataPath;

    }

    [KnownType(typeof(HashedDocument))]
    [DataContract()]
    class HashedDocument : DalilaDBDocument<HashedDocument>
    {

        [DataMember()]
        public Secret password;

        protected override string rootPath_ => Application.persistentDataPath;

    }

    [KnownType(typeof(HashedCollection))]
    [DataContract()]
    class HashedCollection : DalilaDBCollection<HashedCollection>
    {

        [DataMember()]
        public Secret password;

        protected override string rootPath_ => Application.persistentDataPath;

    }

    #endregion Example classes


    [SetUp]
    public void SetUp()
    {
        // Delete the directory to clear it
        try { Directory.Delete(HashedElementsStore.LocationPath, true); } catch (Exception) { }
        HashedElementsStore.DeleteAll();


        // Delete the directory to clear it
        try { Directory.Delete(HashedDocument.LocationPath, true); } catch (Exception) { }
        HashedDocument.Delete();


        // Delete the directory to clear it
        try { Directory.Delete(HashedCollection.LocationPath, true); } catch (Exception) { }
        HashedCollection.DeleteAll();

    }


    [Test]
    public void A000_SecretSimplePasses()
    {
        var ss = new Secret("Hola");
        Secret sp = "Hola";
        Secret sd = "Adios";

        Debug.Log("s1: " + ss);
        Debug.Log("s2: " + sp);
        Debug.Log("s3: " + sd);

        Assert.IsTrue(ss == sp);
        Assert.IsFalse(ss == sd);

    }


    [Test]
    public void A000_Save_TwoHashedElements()
    {

        // Save the password
        var opp = HashedElementsStore.Save("Password", new Secret("PasswordInPlainText"));
        Debug.Log("Saved as Int: " + opp);
        Assert.IsTrue(opp);

        // Now exist
        Debug.Log("Now Exist: " + HashedElementsStore.Exist("Password"));
        Assert.IsTrue(HashedElementsStore.Exist("Password"));
        Assert.IsTrue(HashedElementsStore.Count() == 1);

        // Read the hash
        var readOpp = HashedElementsStore.Find<Secret>("Password");
        Assert.IsTrue(readOpp);

        // Compare Secrets
        Assert.IsTrue(readOpp.Data == "PasswordInPlainText");
        Assert.IsFalse(readOpp.Data == "ThisIs not the pasword");

    }


    [Test]
    public void A100_HashedValueInDocument()
    {

        // Create a instance with a password
        var user1 = new HashedDocument(); user1.password = new Secret("PasswordInPlainText");

        // save
        Debug.Log("Saving");
        Assert.IsTrue(user1.Save());

        // Now find the document
        var readOpp = HashedDocument.Find();
        Assert.IsTrue(readOpp);

        // Compare Secrets
        Assert.IsTrue(readOpp.Data.password == "PasswordInPlainText");
        Assert.IsFalse(readOpp.Data.password == "ThisIs not the pasword");

    }


    [Test]
    public void A100_HashedValueInCollection()
    {

        // You can create multiple instances of the class with diferent passwords
        var user1 = new HashedCollection(); user1.password = new Secret("PasswordInPlainTextOne");
        var user2 = new HashedCollection(); user2.password = new Secret("PasswordInPlainTextTwo");

        // save
        Debug.Log("Saving");
        Assert.IsTrue(user1.Save());
        Assert.IsTrue(user2.Save());

        // Now find the data
        var readOpp1 = HashedCollection.FindById(user1._id);
        var readOpp2 = HashedCollection.FindById(user2._id);
        Assert.IsTrue(readOpp1);
        Assert.IsTrue(readOpp2);

        // Compare Secrets of User One
        Assert.IsTrue(readOpp1.Data.password == "PasswordInPlainTextOne");
        Assert.IsFalse(readOpp1.Data.password == "PasswordInPlainTextTwo");
        Assert.IsFalse(readOpp1.Data.password == "ThisIs not the pasword");

        // Compare Secrets of User Two
        Assert.IsFalse(readOpp2.Data.password == "PasswordInPlainTextOne");
        Assert.IsTrue(readOpp2.Data.password == "PasswordInPlainTextTwo");
        Assert.IsFalse(readOpp2.Data.password == "ThisIs not the pasword");

    }

}
