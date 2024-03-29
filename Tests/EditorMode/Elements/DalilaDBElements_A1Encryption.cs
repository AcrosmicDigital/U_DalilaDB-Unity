﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using U.DalilaDB;
using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Linq;

public class DalilaElements_A1Encryption
{

    #region Example classes

    class SimpleEncryptedElements : DalilaDBElements<SimpleEncryptedElements>
    {
        protected override string rootPath_ => Application.persistentDataPath;

        protected override bool _aesEncryption => true;
        protected override DalilaFS.aesValidKeySizes _aesKeySize => DalilaFS.aesValidKeySizes.aes256;
        protected override string _aesFixedKey => "TheKeyIsNew";

    }

    #endregion Example classes


    [SetUp]
    public void SetUp()
    {
        // Delete the directory to clear it
        try { Directory.Delete(SimpleEncryptedElements.LocationPath, true); } catch (Exception) { }
        SimpleEncryptedElements.DeleteAll();

    }




    [Test]
    public void A000_Save_TwoElementsWithSameKey_WillOverwrite()
    {

        // If save a key
        int num = 10;
        var opp = SimpleEncryptedElements.Save("Number", num);
        Debug.Log("Saved as Int: " + opp);
        Assert.IsTrue(opp);

        // Now exist
        Debug.Log("Now Exist: " + SimpleEncryptedElements.Exist("Number"));
        Assert.IsTrue(SimpleEncryptedElements.Exist("Number"));
        Assert.IsTrue(SimpleEncryptedElements.Count() == 1);

        // And can de readed
        var numReaded = SimpleEncryptedElements.Find<int>("Number");
        Debug.Log("Readed as Int: Opp: " + numReaded + " V: " + numReaded.Data);
        Assert.IsTrue(numReaded);
        Assert.IsTrue(numReaded.Data == 10);


        // But if other value is saved with the same key
        string name = "Hello";
        var opp2 = SimpleEncryptedElements.Save("Number", name);
        Debug.Log("Saved as String: " + opp2);
        Assert.IsTrue(opp2);

        // Still Exist
        Debug.Log("Still Exist: " + SimpleEncryptedElements.Exist("Number"));
        Assert.IsTrue(SimpleEncryptedElements.Exist("Number"));
        Assert.IsTrue(SimpleEncryptedElements.Count() == 1);

        // But cant be readed as int any more
        var numReaded2 = SimpleEncryptedElements.Find<int>("Number");
        Debug.Log("Readed String as Int: Opp: " + numReaded2);
        Assert.IsFalse(numReaded2);
        Assert.Throws<InvalidCastException>(() => throw numReaded2.Error);

        // Still Exist
        Debug.Log("Still Exist: " + SimpleEncryptedElements.Exist("Number"));
        Assert.IsTrue(SimpleEncryptedElements.Exist("Number"));
        Assert.IsTrue(SimpleEncryptedElements.Count() == 1);

        // Now it must be readed as string
        var stringReaded = SimpleEncryptedElements.Find<string>("Number");
        Debug.Log("Readed as String: Opp: " + stringReaded + " V: " + stringReaded.Data);
        Assert.IsTrue(stringReaded);
        Assert.IsTrue(stringReaded.Data == "Hello");

    }

    [Test]
    public void A001_Save_AElementWithInvalidKey_WillThrowFormatException()
    {

        // If try to save a wrong key
        int num = 10;
        Debug.Log("Save with invalid keys");
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.Save("Number 2.", num));
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.Save("Number#", num));
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.Save("Number/2.", num));
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.Save("Number.exx", num));
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.Save("Number22!", num));
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.Save("Number ", num));

        // Any document Exist
        Debug.Log("Any exist: " + SimpleEncryptedElements.Count());
        Assert.IsTrue(SimpleEncryptedElements.Count() == 0);
    }

    [Test]
    public void A002_Save_AElementWithNullKey_WillThrowArgumentNullException()
    {

        Debug.Log("Save with null or empty keys");
        Assert.Throws<ArgumentNullException>(() => SimpleEncryptedElements.Save(null, 2));
        Assert.Throws<ArgumentNullException>(() => SimpleEncryptedElements.Save("", 3));

        // Any document Exist
        Debug.Log("Any exist: " + SimpleEncryptedElements.Count());
        Assert.IsTrue(SimpleEncryptedElements.Count() == 0);
    }

    [Test]
    public void A003_Save_ANullElement_WillThrowArgumentNullException()
    {

        // If try to save a null object
        Assert.Throws<ArgumentNullException>(() => SimpleEncryptedElements.Save<int[]>("List", null));

        // No document Exist
        Debug.Log("One exist: " + SimpleEncryptedElements.Count());
        Assert.IsTrue(SimpleEncryptedElements.Count() == 0);

    }




    [Test]
    public void A100_Find_AElementOfWrongType_WillReturnFalseAndDefaultTypeValue()
    {

        // If save a key as int
        int num = 10;
        var opp = SimpleEncryptedElements.Save("Number", num);
        Debug.Log("Saved as Int: " + opp);
        Assert.IsTrue(opp);

        // Now exist
        Debug.Log("Now Exist: " + SimpleEncryptedElements.Exist("Number"));
        Assert.IsTrue(SimpleEncryptedElements.Exist("Number"));

        // It can be readed as Int
        var numReaded = SimpleEncryptedElements.Find<int>("Number");
        Debug.Log("Readed as Int: Opp: " + numReaded + " V: " + numReaded.Data);
        Assert.IsTrue(numReaded);
        Assert.IsTrue(numReaded.Data == 10);

        // But cant be readed as string
        var numReaded2 = SimpleEncryptedElements.Find<string>("Number");
        Debug.Log("Readed String as Int: Opp: " + numReaded2);
        Assert.IsFalse(numReaded2);
        Assert.Throws<InvalidCastException>(() => throw numReaded2.Error);
        // Default string is null
        Assert.IsTrue(numReaded2.Data == null);

        // Or as bool
        var numReaded3 = SimpleEncryptedElements.Find<bool>("Number");
        Debug.Log("Readed String as Int: Opp: " + numReaded3);
        Assert.IsFalse(numReaded3);
        Assert.Throws<InvalidCastException>(() => throw numReaded3.Error);
        // Default bool is false
        Assert.IsTrue(numReaded3.Data == false);

        // But Still Exist
        Debug.Log("Still Exist: " + SimpleEncryptedElements.Exist("Number"));
        Assert.IsTrue(SimpleEncryptedElements.Exist("Number"));

        // And can still be read as int
        var numReaded4 = SimpleEncryptedElements.Find<int>("Number");
        Debug.Log("Readed as Int: Opp: " + numReaded4 + " V: " + numReaded4.Data);
        Assert.IsTrue(numReaded4);
        Assert.IsTrue(numReaded4.Data == 10);

    }

    [Test]
    public void A102_Find_AElementWithInvalidKey_WillThrowException()
    {

        // If try to find a wrong key
        Debug.Log("Find with invalid keys");
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.Find<int>("Number 2."));
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.Find<int>("Number#"));
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.Find<int>("Number/2."));
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.Find<int>("Number.exx"));
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.Find<int>("Number22!"));
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.Find<int>("Number "));

        Debug.Log("Save with null or empty keys");
        Assert.Throws<ArgumentNullException>(() => SimpleEncryptedElements.Find<int>(null));
        Assert.Throws<ArgumentNullException>(() => SimpleEncryptedElements.Find<int>(""));

    }

    [Test]
    public void A104_Find_AElementThatDontExist_WillReturnFalseAndDefaultValue()
    {

        // Any exist
        Debug.Log("Files Are: " + SimpleEncryptedElements.Count());
        Assert.IsTrue(SimpleEncryptedElements.Count() == 0);

        // Dont exist
        Debug.Log("Any Exist: " + SimpleEncryptedElements.Exist("Number"));
        Assert.IsFalse(SimpleEncryptedElements.Exist("Number"));

        // Cant be readed
        var numReaded2 = SimpleEncryptedElements.Find<string>("Number");
        Debug.Log("Readed: " + numReaded2);
        Assert.IsFalse(numReaded2);
        Assert.Throws<FileNotFoundException>(() => throw numReaded2.Error);
        // Default string is null
        Assert.IsTrue(numReaded2.Data == null);

        // Dont exist
        Debug.Log("Any Exist: " + SimpleEncryptedElements.Exist("Number"));
        Assert.IsFalse(SimpleEncryptedElements.Exist("Number"));

        // Cant be readed
        var numReaded3 = SimpleEncryptedElements.Find<int>("Text");
        Debug.Log("Readed: " + numReaded3);
        Assert.IsFalse(numReaded3);
        Assert.Throws<FileNotFoundException>(() => throw numReaded3.Error);
        // Default int is 0
        Assert.IsTrue(numReaded3.Data == 0);


    }




    [Test]
    public void A300_FindOrDefault_AElementOfWrongType_WillReturnFalseAndDefaultValuePassed()
    {

        // If save a key as int
        int num = 10;
        var opp = SimpleEncryptedElements.Save("Number", num);
        Debug.Log("Saved as Int: " + opp);
        Assert.IsTrue(opp);

        // Now exist
        Debug.Log("Now Exist: " + SimpleEncryptedElements.Exist("Number"));
        Assert.IsTrue(SimpleEncryptedElements.Exist("Number"));

        // It can be readed as Int
        var numReaded = SimpleEncryptedElements.FindOrDefault<int>("Number", 12);
        Debug.Log("Readed as Int: Opp: " + numReaded + " V: " + numReaded.Data);
        Assert.IsTrue(numReaded);
        Assert.IsTrue(numReaded.Data == 10);

        // But cant be readed as string
        var numReaded2 = SimpleEncryptedElements.FindOrDefault<string>("Number", "Value");
        Debug.Log("Readed String as Int: Opp: " + numReaded2);
        Assert.IsTrue(numReaded2);
        // Default string is null
        Assert.IsTrue(numReaded2.Data == "Value");

        // Or as bool
        var numReaded3 = SimpleEncryptedElements.FindOrDefault<bool>("Number", true);
        Debug.Log("Readed String as Int: Opp: " + numReaded3);
        Assert.IsTrue(numReaded3);
        // Default bool is false
        Assert.IsTrue(numReaded3.Data == true);

        // But Still Exist
        Debug.Log("Still Exist: " + SimpleEncryptedElements.Exist("Number"));
        Assert.IsTrue(SimpleEncryptedElements.Exist("Number"));

        // And can still be read as int
        var numReaded4 = SimpleEncryptedElements.FindOrDefault<int>("Number", 22);
        Debug.Log("Readed as Int: Opp: " + numReaded4 + " V: " + numReaded4.Data);
        Assert.IsTrue(numReaded4);
        Assert.IsTrue(numReaded4.Data == 10);

    }

    [Test]
    public void A302_FindOrDefault_AElementWithInvalidKey_WillThrowException()
    {

        // If try to find a wrong key
        Debug.Log("Find with invalid keys");
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.FindOrDefault<int>("Number 2.", 1));
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.FindOrDefault<int>("Number#", 1));
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.FindOrDefault<int>("Number/2.", 1));
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.FindOrDefault<int>("Number.exx", 1));
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.FindOrDefault<int>("Number22!", 1));
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.FindOrDefault<int>("Number ", 1));

        Debug.Log("Save with null or empty keys");
        Assert.Throws<ArgumentNullException>(() => SimpleEncryptedElements.FindOrDefault<int>(null, 1));
        Assert.Throws<ArgumentNullException>(() => SimpleEncryptedElements.FindOrDefault<int>("", 1));

    }

    [Test]
    public void A304_FindOrDefault_AElementThatDontExist_WillReturnFalseAndDefaultValuePassed()
    {

        // Any exist
        Debug.Log("Files Are: " + SimpleEncryptedElements.Count());
        Assert.IsTrue(SimpleEncryptedElements.Count() == 0);

        // Dont exist
        Debug.Log("Any Exist: " + SimpleEncryptedElements.Exist("Number"));
        Assert.IsFalse(SimpleEncryptedElements.Exist("Number"));

        // Cant be readed
        var numReaded2 = SimpleEncryptedElements.FindOrDefault<string>("Number", "Data");
        Debug.Log("Readed: " + numReaded2);
        Assert.IsTrue(numReaded2);
        // Default string is null
        Assert.IsTrue(numReaded2.Data == "Data");

        // Dont exist
        Debug.Log("Any Exist: " + SimpleEncryptedElements.Exist("Number"));
        Assert.IsFalse(SimpleEncryptedElements.Exist("Number"));

        // Cant be readed
        var numReaded3 = SimpleEncryptedElements.FindOrDefault<int>("Text", 11);
        Debug.Log("Readed: " + numReaded3);
        Assert.IsTrue(numReaded3);
        // Default int is 0
        Assert.IsTrue(numReaded3.Data == 11);


    }



    [Test]
    public void A400_Update_01WillFindAndUpdateAnDocumentIfExist()
    {

        // Save elements
        Debug.Log("Save");
        var newd1 = SimpleEncryptedElements.Save("Data1", 22);
        var newd2 = SimpleEncryptedElements.Save("Data2", "String");
        var newd3 = SimpleEncryptedElements.Save("Data3", true);

        // The first one will be saved
        Debug.Log("Save");
        Assert.IsTrue(newd1);
        Assert.IsTrue(newd2);
        Assert.IsTrue(newd3);

        // Only one document will be stored
        Debug.Log("Save");
        Assert.IsTrue(SimpleEncryptedElements.Count() == 3);


        // Find the elemnts
        Debug.Log("Save");
        var opp1 = SimpleEncryptedElements.Find<int>("Data1");
        var opp2 = SimpleEncryptedElements.Find<string>("Data2");
        var opp3 = SimpleEncryptedElements.Find<bool>("Data3");
        Assert.IsTrue(opp1);
        Assert.IsTrue(opp2);
        Assert.IsTrue(opp3);
        Assert.AreEqual(22, opp1.Data);
        Assert.AreEqual("String", opp2.Data);
        Assert.AreEqual(true, opp3.Data);

        // Update the eleemnts
        Debug.Log("Update");
        var upp1 = SimpleEncryptedElements.Update<int>("Data1", v => v + 44);
        var upp2 = SimpleEncryptedElements.Update<string>("Data2", v => "New" + v);
        var upp3 = SimpleEncryptedElements.Update<bool>("Data3", v => !v);
        Assert.IsTrue(upp1);
        Assert.IsTrue(upp2);
        Assert.IsTrue(upp3);

        // Find the elements
        Debug.Log("Save");
        opp1 = SimpleEncryptedElements.Find<int>("Data1");
        opp2 = SimpleEncryptedElements.Find<string>("Data2");
        opp3 = SimpleEncryptedElements.Find<bool>("Data3");
        Assert.IsTrue(opp1);
        Assert.IsTrue(opp2);
        Assert.IsTrue(opp3);
        Assert.AreEqual(66, opp1.Data);
        Assert.AreEqual("NewString", opp2.Data);
        Assert.AreEqual(false, opp3.Data);


        // Try to Update an wrong type document will return false
        Debug.Log("UpdateWrongType");
        upp1 = SimpleEncryptedElements.Update<string>("Data1", v => "Hello");
        upp2 = SimpleEncryptedElements.Update<bool>("Data2", v => !v);
        upp3 = SimpleEncryptedElements.Update<int>("Data3", v => v - 11);
        Debug.Log(upp1);
        Assert.IsFalse(upp1);
        Assert.IsFalse(upp2);
        Assert.IsFalse(upp3);


        // Find the elements are nopt changed
        Debug.Log("Save");
        opp1 = SimpleEncryptedElements.Find<int>("Data1");
        opp2 = SimpleEncryptedElements.Find<string>("Data2");
        opp3 = SimpleEncryptedElements.Find<bool>("Data3");
        Assert.IsTrue(opp1);
        Assert.IsTrue(opp2);
        Assert.IsTrue(opp3);
        Assert.AreEqual(66, opp1.Data);
        Assert.AreEqual("NewString", opp2.Data);
        Assert.AreEqual(false, opp3.Data);


        // Try to Update an unexistent document will return false
        Debug.Log("UpdateUnexistent");
        var eupp = SimpleEncryptedElements.Update<int>("DataDont", v => v + 44);
        Assert.IsFalse(eupp);

        // Document is not stored
        Assert.IsTrue(SimpleEncryptedElements.Count() == 3);

    }

    [Test]
    public void A400_Update_IfUpdatingThrowsError_DontUpdate()
    {

        // Save elements
        Debug.Log("Save");
        var newd1 = SimpleEncryptedElements.Save("Data1", 22);
        var newd2 = SimpleEncryptedElements.Save("Data2", "String");
        var newd3 = SimpleEncryptedElements.Save("Data3", true);

        // The first one will be saved
        Debug.Log("Save");
        Assert.IsTrue(newd1);
        Assert.IsTrue(newd2);
        Assert.IsTrue(newd3);

        // Only one document will be stored
        Debug.Log("Save");
        Assert.IsTrue(SimpleEncryptedElements.Count() == 3);


        // Find the elemnts
        Debug.Log("Save");
        var opp1 = SimpleEncryptedElements.Find<int>("Data1");
        var opp2 = SimpleEncryptedElements.Find<string>("Data2");
        var opp3 = SimpleEncryptedElements.Find<bool>("Data3");
        Assert.IsTrue(opp1);
        Assert.IsTrue(opp2);
        Assert.IsTrue(opp3);
        Assert.AreEqual(22, opp1.Data);
        Assert.AreEqual("String", opp2.Data);
        Assert.AreEqual(true, opp3.Data);

        // Update the eleemnts
        Debug.Log("Update");
        var upp1 = SimpleEncryptedElements.Update<int>("Data1", v => throw new Exception("Expected exception"));
        var upp2 = SimpleEncryptedElements.Update<string>("Data2", v => throw new Exception("Expected exception"));
        var upp3 = SimpleEncryptedElements.Update<bool>("Data3", v => throw new Exception("Expected exception"));
        Assert.IsFalse(upp1);
        Assert.IsFalse(upp2);
        Assert.IsFalse(upp3);


        // Find the elemnts
        Debug.Log("Save");
        opp1 = SimpleEncryptedElements.Find<int>("Data1");
        opp2 = SimpleEncryptedElements.Find<string>("Data2");
        opp3 = SimpleEncryptedElements.Find<bool>("Data3");
        Assert.IsTrue(opp1);
        Assert.IsTrue(opp2);
        Assert.IsTrue(opp3);
        Assert.AreEqual(22, opp1.Data);
        Assert.AreEqual("String", opp2.Data);
        Assert.AreEqual(true, opp3.Data);


    }

    [Test]
    public void A400_Update_61InvalidOrNullKeyOrUpdateFunc_WillThrowError()
    {

        // Find the document by the id
        Assert.Throws<ArgumentNullException>(() => SimpleEncryptedElements.Update<int>(null, u => 88));
        Assert.Throws<ArgumentNullException>(() => SimpleEncryptedElements.Update<int>("", u => 88));
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.Update<int>("ff ff", u => 88));

        // Null update func
        Assert.Throws<ArgumentNullException>(() => SimpleEncryptedElements.Update<int>("2893128734678923478", null));
        Assert.Throws<ArgumentNullException>(() => SimpleEncryptedElements.Update<int>(null, null));

    }





    [Test]
    public void A200_Delete_WithValidKeyPassed_WillDeleteThatKey()
    {

        // Save some files
        SimpleEncryptedElements.Save("One", 33);
        SimpleEncryptedElements.Save("Two", 33);
        SimpleEncryptedElements.Save("Three", 33);
        SimpleEncryptedElements.Save("Four", 33);
        SimpleEncryptedElements.Save("Five", 33);
        SimpleEncryptedElements.Save("Six", 33);
        Assert.IsTrue(SimpleEncryptedElements.Count() == 6);

        Debug.Log("Delete Keys");
        Assert.IsTrue(SimpleEncryptedElements.Find<int>("One"));
        var opp = SimpleEncryptedElements.Delete("One");
        Assert.IsTrue(opp);
        Assert.IsTrue(SimpleEncryptedElements.Count() == 5);
        Assert.IsFalse(SimpleEncryptedElements.Find<int>("One"));

        Assert.IsTrue(SimpleEncryptedElements.Find<int>("Two"));
        opp = SimpleEncryptedElements.Delete("Two");
        Assert.IsTrue(opp);
        Assert.IsTrue(SimpleEncryptedElements.Count() == 4);
        Assert.IsFalse(SimpleEncryptedElements.Find<int>("Two"));

        Assert.IsTrue(SimpleEncryptedElements.Find<int>("Four"));
        opp = SimpleEncryptedElements.Delete("Four");
        Assert.IsTrue(opp);
        Assert.IsTrue(SimpleEncryptedElements.Count() == 3);
        Assert.IsFalse(SimpleEncryptedElements.Find<int>("Four"));

        Assert.IsTrue(SimpleEncryptedElements.Find<int>("Three"));
        opp = SimpleEncryptedElements.Delete("Three");
        Assert.IsTrue(opp);
        Assert.IsTrue(SimpleEncryptedElements.Count() == 2);
        Assert.IsFalse(SimpleEncryptedElements.Find<int>("Three"));

    }

    [Test]
    public void A203_Delete_WithUnexistentKeyPassed_WillReturnTrue()
    {

        //// Save some files
        Assert.IsTrue(SimpleEncryptedElements.Count() == 0);

        Debug.Log("Delete unexistent");
        Assert.IsTrue(SimpleEncryptedElements.Delete("One"));
        Assert.IsTrue(SimpleEncryptedElements.Delete("Two"));
        Assert.IsTrue(SimpleEncryptedElements.Delete("Three"));
        Assert.IsTrue(SimpleEncryptedElements.Delete("Four"));
        Assert.IsTrue(SimpleEncryptedElements.Delete("Five"));
        Assert.IsTrue(SimpleEncryptedElements.Delete("Six"));

    }

    [Test]
    public void A206_Delete_AElementWithInvalidKey_WillThrowException()
    {

        // If try to find a wrong key
        Debug.Log("Find with invalid keys");
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.Delete("Number 2."));
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.Delete("Number#"));
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.Delete("Number/2."));
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.Delete("Number.exx"));
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.Delete("Number22!"));
        Assert.Throws<FormatException>(() => SimpleEncryptedElements.Delete("Number "));

        Debug.Log("Save with null or empty keys");
        Assert.Throws<ArgumentNullException>(() => SimpleEncryptedElements.Delete(""));

    }




    [Test]
    public void A500_DeleteAll_WillDeleteAllFiles()
    {

        // Save some files
        SimpleEncryptedElements.Save("Uno", 33);
        SimpleEncryptedElements.Save("Dos", 33);
        SimpleEncryptedElements.Save("Tres", 33);
        SimpleEncryptedElements.Save("Four", 33);
        SimpleEncryptedElements.Save("Five", 33);
        SimpleEncryptedElements.Save("Six", 33);
        Assert.IsTrue(SimpleEncryptedElements.Count() == 6);

        Debug.Log("Delete all");
        var opp = SimpleEncryptedElements.DeleteAll();
        Assert.IsTrue(opp);
        Assert.IsTrue(SimpleEncryptedElements.Count() == 0);

    }


}