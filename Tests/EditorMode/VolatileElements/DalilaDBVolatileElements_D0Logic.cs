using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using U.DalilaDB;
using System.Text.RegularExpressions;
using System;
using System.IO;

public class DalilaVolatileElements_D0Logic
{

    #region Example classes

    class SimpleElements : DalilaDBVolatileElements<SimpleElements>
    {

    }

    #endregion Example classes


    [SetUp]
    public void SetUp()
    {
        // Delete the directory to clear it
        SimpleElements.DeleteAll();

    }




    [Test]
    public void A000_Save_TwoElementsWithSameKey_WillOverwrite()
    {
        
        // If save a key
        int num = 10;
        var opp = SimpleElements.Save("Number", num);
        Debug.Log("Saved as Int: " + opp);
        Assert.IsTrue(opp);

        // Now exist
        Debug.Log("Now Exist: " + SimpleElements.Exist("Number"));
        Assert.IsTrue(SimpleElements.Exist("Number"));
        Assert.IsTrue(SimpleElements.Count() == 1);

        // And can de readed
        var numReaded = SimpleElements.Find<int>("Number");
        Debug.Log("Readed as Int: Opp: " + numReaded + " V: " + numReaded.Data);
        Assert.IsTrue(numReaded);
        Assert.IsTrue(numReaded.Data == 10);


        // But if other value is saved with the same key
        string name = "Hello";
        var opp2 = SimpleElements.Save("Number", name);
        Debug.Log("Saved as String: " + opp2);
        Assert.IsTrue(opp2);

        // Still Exist
        Debug.Log("Still Exist: " + SimpleElements.Exist("Number"));
        Assert.IsTrue(SimpleElements.Exist("Number"));
        Assert.IsTrue(SimpleElements.Count() == 1);

        // But cant be readed as int any more
        var numReaded2 = SimpleElements.Find<int>("Number");
        Debug.Log("Readed String as Int: Opp: " + numReaded2);
        Assert.IsFalse(numReaded2);
        Assert.Throws<InvalidCastException>(() => throw numReaded2.Error);

        // Still Exist
        Debug.Log("Still Exist: " + SimpleElements.Exist("Number"));
        Assert.IsTrue(SimpleElements.Exist("Number"));
        Assert.IsTrue(SimpleElements.Count() == 1);

        // Now it must be readed as string
        var stringReaded = SimpleElements.Find<string>("Number");
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
        Assert.Throws<FormatException>(() => SimpleElements.Save("Number 2.", num));
        Assert.Throws<FormatException>(() => SimpleElements.Save("Number#", num));
        Assert.Throws<FormatException>(() => SimpleElements.Save("Number/2.", num));
        Assert.Throws<FormatException>(() => SimpleElements.Save("Number.exx", num));
        Assert.Throws<FormatException>(() => SimpleElements.Save("Number22!", num));
        Assert.Throws<FormatException>(() => SimpleElements.Save("Number ", num));

        // Any document Exist
        Debug.Log("Any exist: " + SimpleElements.Count());
        Assert.IsTrue(SimpleElements.Count() == 0);
    }

    [Test]
    public void A002_Save_AElementWithNullKey_WillThrowArgumentNullException()
    {

        Debug.Log("Save with null or empty keys");
        Assert.Throws<ArgumentNullException>(() => SimpleElements.Save(null, 2));
        Assert.Throws<ArgumentNullException>(() => SimpleElements.Save("", 3));

        // Any document Exist
        Debug.Log("Any exist: " + SimpleElements.Count());
        Assert.IsTrue(SimpleElements.Count() == 0);
    }

    [Test]
    public void A003_Save_ANullElement_WillThrowArgumentNullException()
    {
        
        // If try to save a null object
        Assert.Throws<ArgumentNullException>(() => SimpleElements.Save<int[]>("List", null));

        // No document Exist
        Debug.Log("One exist: " + SimpleElements.Count());
        Assert.IsTrue(SimpleElements.Count() == 0);

    }




    [Test]
    public void A100_Find_AElementOfWrongType_WillReturnFalseAndDefaultTypeValue()
    {
        
        // If save a key as int
        int num = 10;
        var opp = SimpleElements.Save("Number", num);
        Debug.Log("Saved as Int: " + opp);
        Assert.IsTrue(opp);

        // Now exist
        Debug.Log("Now Exist: " + SimpleElements.Exist("Number"));
        Assert.IsTrue(SimpleElements.Exist("Number"));

        // It can be readed as Int
        var numReaded = SimpleElements.Find<int>("Number");
        Debug.Log("Readed as Int: Opp: " + numReaded + " V: " + numReaded.Data);
        Assert.IsTrue(numReaded);
        Assert.IsTrue(numReaded.Data == 10);

        // But cant be readed as string
        var numReaded2 = SimpleElements.Find<string>("Number");
        Debug.Log("Readed String as Int: Opp: " + numReaded2);
        Assert.IsFalse(numReaded2);
        Assert.Throws<InvalidCastException>(() => throw numReaded2.Error);
        // Default string is null
        Assert.IsTrue(numReaded2.Data == null);

        // Or as bool
        var numReaded3 = SimpleElements.Find<bool>("Number");
        Debug.Log("Readed String as Int: Opp: " + numReaded3);
        Assert.IsFalse(numReaded3);
        Assert.Throws<InvalidCastException>(() => throw numReaded3.Error);
        // Default bool is false
        Assert.IsTrue(numReaded3.Data == false);

        // But Still Exist
        Debug.Log("Still Exist: " + SimpleElements.Exist("Number"));
        Assert.IsTrue(SimpleElements.Exist("Number"));

        // And can still be read as int
        var numReaded4 = SimpleElements.Find<int>("Number");
        Debug.Log("Readed as Int: Opp: " + numReaded4 + " V: " + numReaded4.Data);
        Assert.IsTrue(numReaded4);
        Assert.IsTrue(numReaded4.Data == 10);

    }

    [Test]
    public void A102_Find_AElementWithInvalidKey_WillThrowException()
    {
        
        // If try to find a wrong key
        Debug.Log("Find with invalid keys");
        Assert.Throws<FormatException>(() => SimpleElements.Find<int>("Number 2."));
        Assert.Throws<FormatException>(() => SimpleElements.Find<int>("Number#"));
        Assert.Throws<FormatException>(() => SimpleElements.Find<int>("Number/2."));
        Assert.Throws<FormatException>(() => SimpleElements.Find<int>("Number.exx"));
        Assert.Throws<FormatException>(() => SimpleElements.Find<int>("Number22!"));
        Assert.Throws<FormatException>(() => SimpleElements.Find<int>("Number "));

        Debug.Log("Save with null or empty keys");
        Assert.Throws<ArgumentNullException>(() => SimpleElements.Find<int>(null));
        Assert.Throws<ArgumentNullException>(() => SimpleElements.Find<int>(""));

    }

    [Test]
    public void A104_Find_AElementThatDontExist_WillReturnFalseAndDefaultValue()
    {
        
        // Any exist
        Debug.Log("Files Are: " + SimpleElements.Count());
        Assert.IsTrue(SimpleElements.Count() == 0);

        // Dont exist
        Debug.Log("Any Exist: " + SimpleElements.Exist("Number"));
        Assert.IsFalse(SimpleElements.Exist("Number"));

        // Cant be readed
        var numReaded2 = SimpleElements.Find<string>("Number");
        Debug.Log("Readed: " + numReaded2);
        Assert.IsFalse(numReaded2);
        Assert.Throws<FileNotFoundException>(() => throw numReaded2.Error);
        // Default string is null
        Assert.IsTrue(numReaded2.Data == null);

        // Dont exist
        Debug.Log("Any Exist: " + SimpleElements.Exist("Number"));
        Assert.IsFalse(SimpleElements.Exist("Number"));

        // Cant be readed
        var numReaded3 = SimpleElements.Find<int>("Text");
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
        var opp = SimpleElements.Save("Number", num);
        Debug.Log("Saved as Int: " + opp);
        Assert.IsTrue(opp);

        // Now exist
        Debug.Log("Now Exist: " + SimpleElements.Exist("Number"));
        Assert.IsTrue(SimpleElements.Exist("Number"));

        // It can be readed as Int
        var numReaded = SimpleElements.FindOrDefault<int>("Number", 12);
        Debug.Log("Readed as Int: Opp: " + numReaded + " V: " + numReaded.Data);
        Assert.IsTrue(numReaded);
        Assert.IsTrue(numReaded.Data == 10);

        // But cant be readed as string
        var numReaded2 = SimpleElements.FindOrDefault<string>("Number", "Value");
        Debug.Log("Readed String as Int: Opp: " + numReaded2);
        Assert.IsTrue(numReaded2);
        // Default string is null
        Assert.IsTrue(numReaded2.Data == "Value");

        // Or as bool
        var numReaded3 = SimpleElements.FindOrDefault<bool>("Number", true);
        Debug.Log("Readed String as Int: Opp: " + numReaded3);
        Assert.IsTrue(numReaded3);
        // Default bool is false
        Assert.IsTrue(numReaded3.Data == true);

        // But Still Exist
        Debug.Log("Still Exist: " + SimpleElements.Exist("Number"));
        Assert.IsTrue(SimpleElements.Exist("Number"));

        // And can still be read as int
        var numReaded4 = SimpleElements.FindOrDefault<int>("Number", 22);
        Debug.Log("Readed as Int: Opp: " + numReaded4 + " V: " + numReaded4.Data);
        Assert.IsTrue(numReaded4);
        Assert.IsTrue(numReaded4.Data == 10);

    }

    [Test]
    public void A302_FindOrDefault_AElementWithInvalidKey_WillThrowException()
    {

        // If try to find a wrong key
        Debug.Log("Find with invalid keys");
        Assert.Throws<FormatException>(() => SimpleElements.FindOrDefault<int>("Number 2.", 1));
        Assert.Throws<FormatException>(() => SimpleElements.FindOrDefault<int>("Number#", 1));
        Assert.Throws<FormatException>(() => SimpleElements.FindOrDefault<int>("Number/2.", 1));
        Assert.Throws<FormatException>(() => SimpleElements.FindOrDefault<int>("Number.exx", 1));
        Assert.Throws<FormatException>(() => SimpleElements.FindOrDefault<int>("Number22!", 1));
        Assert.Throws<FormatException>(() => SimpleElements.FindOrDefault<int>("Number ", 1));

        Debug.Log("Save with null or empty keys");
        Assert.Throws<ArgumentNullException>(() => SimpleElements.FindOrDefault<int>(null, 1));
        Assert.Throws<ArgumentNullException>(() => SimpleElements.FindOrDefault<int>("", 1));

    }

    [Test]
    public void A304_FindOrDefault_AElementThatDontExist_WillReturnFalseAndDefaultValuePassed()
    {

        // Any exist
        Debug.Log("Files Are: " + SimpleElements.Count());
        Assert.IsTrue(SimpleElements.Count() == 0);

        // Dont exist
        Debug.Log("Any Exist: " + SimpleElements.Exist("Number"));
        Assert.IsFalse(SimpleElements.Exist("Number"));

        // Cant be readed
        var numReaded2 = SimpleElements.FindOrDefault<string>("Number", "Data");
        Debug.Log("Readed: " + numReaded2);
        Assert.IsTrue(numReaded2);
        // Default string is null
        Assert.IsTrue(numReaded2.Data == "Data");

        // Dont exist
        Debug.Log("Any Exist: " + SimpleElements.Exist("Number"));
        Assert.IsFalse(SimpleElements.Exist("Number"));

        // Cant be readed
        var numReaded3 = SimpleElements.FindOrDefault<int>("Text", 11);
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
        var newd1 = SimpleElements.Save("Data1", 22);
        var newd2 = SimpleElements.Save("Data2", "String");
        var newd3 = SimpleElements.Save("Data3", true);

        // The first one will be saved
        Debug.Log("Save");
        Assert.IsTrue(newd1);
        Assert.IsTrue(newd2);
        Assert.IsTrue(newd3);

        // Only one document will be stored
        Debug.Log("Save");
        Assert.IsTrue(SimpleElements.Count() == 3);


        // Find the elemnts
        Debug.Log("Save");
        var opp1 = SimpleElements.Find<int>("Data1");
        var opp2 = SimpleElements.Find<string>("Data2");
        var opp3 = SimpleElements.Find<bool>("Data3");
        Assert.IsTrue(opp1);
        Assert.IsTrue(opp2);
        Assert.IsTrue(opp3);
        Assert.AreEqual(22, opp1.Data);
        Assert.AreEqual("String", opp2.Data);
        Assert.AreEqual(true, opp3.Data);

        // Update the eleemnts
        Debug.Log("Update");
        var upp1 = SimpleElements.Update<int>("Data1", v => v + 44);
        var upp2 = SimpleElements.Update<string>("Data2", v => "New" + v);
        var upp3 = SimpleElements.Update<bool>("Data3", v => !v);
        Assert.IsTrue(upp1);
        Assert.IsTrue(upp2);
        Assert.IsTrue(upp3);

        // Find the elements
        Debug.Log("Save");
        opp1 = SimpleElements.Find<int>("Data1");
        opp2 = SimpleElements.Find<string>("Data2");
        opp3 = SimpleElements.Find<bool>("Data3");
        Assert.IsTrue(opp1);
        Assert.IsTrue(opp2);
        Assert.IsTrue(opp3);
        Assert.AreEqual(66, opp1.Data);
        Assert.AreEqual("NewString", opp2.Data);
        Assert.AreEqual(false, opp3.Data);


        // Try to Update an wrong type document will return false
        Debug.Log("UpdateWrongType");
        upp1 = SimpleElements.Update<string>("Data1", v => "Hello");
        upp2 = SimpleElements.Update<bool>("Data2", v => !v);
        upp3 = SimpleElements.Update<int>("Data3", v => v - 11);
        Debug.Log(upp1);
        Assert.IsFalse(upp1);
        Assert.IsFalse(upp2);
        Assert.IsFalse(upp3);


        // Find the elements are nopt changed
        Debug.Log("Save");
        opp1 = SimpleElements.Find<int>("Data1");
        opp2 = SimpleElements.Find<string>("Data2");
        opp3 = SimpleElements.Find<bool>("Data3");
        Assert.IsTrue(opp1);
        Assert.IsTrue(opp2);
        Assert.IsTrue(opp3);
        Assert.AreEqual(66, opp1.Data);
        Assert.AreEqual("NewString", opp2.Data);
        Assert.AreEqual(false, opp3.Data);


        // Try to Update an unexistent document will return false
        Debug.Log("UpdateUnexistent");
        var eupp = SimpleElements.Update<int>("DataDont", v => v + 44);
        Assert.IsFalse(eupp);

        // Document is not stored
        Assert.IsTrue(SimpleElements.Count() == 3);

    }

    [Test]
    public void A400_Update_IfUpdatingThrowsError_DontUpdate()
    {

        // Save elements
        Debug.Log("Save");
        var newd1 = SimpleElements.Save("Data1", 22);
        var newd2 = SimpleElements.Save("Data2", "String");
        var newd3 = SimpleElements.Save("Data3", true);

        // The first one will be saved
        Debug.Log("Save");
        Assert.IsTrue(newd1);
        Assert.IsTrue(newd2);
        Assert.IsTrue(newd3);

        // Only one document will be stored
        Debug.Log("Save");
        Assert.IsTrue(SimpleElements.Count() == 3);


        // Find the elemnts
        Debug.Log("Save");
        var opp1 = SimpleElements.Find<int>("Data1");
        var opp2 = SimpleElements.Find<string>("Data2");
        var opp3 = SimpleElements.Find<bool>("Data3");
        Assert.IsTrue(opp1);
        Assert.IsTrue(opp2);
        Assert.IsTrue(opp3);
        Assert.AreEqual(22, opp1.Data);
        Assert.AreEqual("String", opp2.Data);
        Assert.AreEqual(true, opp3.Data);

        // Update the eleemnts
        Debug.Log("Update");
        var upp1 = SimpleElements.Update<int>("Data1", v => throw new Exception("Expected exception"));
        var upp2 = SimpleElements.Update<string>("Data2", v => throw new Exception("Expected exception"));
        var upp3 = SimpleElements.Update<bool>("Data3", v => throw new Exception("Expected exception"));
        Assert.IsFalse(upp1);
        Assert.IsFalse(upp2);
        Assert.IsFalse(upp3);


        // Find the elemnts
        Debug.Log("Save");
        opp1 = SimpleElements.Find<int>("Data1");
        opp2 = SimpleElements.Find<string>("Data2");
        opp3 = SimpleElements.Find<bool>("Data3");
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
        Assert.Throws<ArgumentNullException>(() => SimpleElements.Update<int>(null, u => 88));
        Assert.Throws<ArgumentNullException>(() => SimpleElements.Update<int>("", u => 88));
        Assert.Throws<FormatException>(() => SimpleElements.Update<int>("ff ff", u => 88));

        // Null update func
        Assert.Throws<ArgumentNullException>(() => SimpleElements.Update<int>("2893128734678923478", null));
        Assert.Throws<ArgumentNullException>(() => SimpleElements.Update<int>(null, null));

    }





    [Test]
    public void A200_Delete_WithValidKeyPassed_WillDeleteThatKey()
    {
        
        // Save some files
        SimpleElements.Save("One", 33);
        SimpleElements.Save("Two", 33);
        SimpleElements.Save("Three", 33);
        SimpleElements.Save("Four", 33);
        SimpleElements.Save("Five", 33);
        SimpleElements.Save("Six", 33);
        Assert.IsTrue(SimpleElements.Count() == 6);

        Debug.Log("Delete Keys");
        Assert.IsTrue(SimpleElements.Find<int>("One"));
        var opp = SimpleElements.Delete("One");
        Assert.IsTrue(opp);
        Assert.IsTrue(SimpleElements.Count() == 5);
        Assert.IsFalse(SimpleElements.Find<int>("One"));

        Assert.IsTrue(SimpleElements.Find<int>("Two"));
        opp = SimpleElements.Delete("Two");
        Assert.IsTrue(opp);
        Assert.IsTrue(SimpleElements.Count() == 4);
        Assert.IsFalse(SimpleElements.Find<int>("Two"));

        Assert.IsTrue(SimpleElements.Find<int>("Four"));
        opp = SimpleElements.Delete("Four");
        Assert.IsTrue(opp);
        Assert.IsTrue(SimpleElements.Count() == 3);
        Assert.IsFalse(SimpleElements.Find<int>("Four"));

        Assert.IsTrue(SimpleElements.Find<int>("Three"));
        opp = SimpleElements.Delete("Three");
        Assert.IsTrue(opp);
        Assert.IsTrue(SimpleElements.Count() == 2);
        Assert.IsFalse(SimpleElements.Find<int>("Three"));

    }

    [Test]
    public void A203_Delete_WithUnexistentKeyPassed_WillReturnTrue()
    {
        
        //// Save some files
        Assert.IsTrue(SimpleElements.Count() == 0);

        Debug.Log("Delete unexistent");
        Assert.IsTrue(SimpleElements.Delete("One"));
        Assert.IsTrue(SimpleElements.Delete("Two"));
        Assert.IsTrue(SimpleElements.Delete("Three"));
        Assert.IsTrue(SimpleElements.Delete("Four"));
        Assert.IsTrue(SimpleElements.Delete("Five"));
        Assert.IsTrue(SimpleElements.Delete("Six"));

    }

    [Test]
    public void A206_Delete_AElementWithInvalidKey_WillThrowException()
    {
        
        // If try to find a wrong key
        Debug.Log("Find with invalid keys");
        Assert.Throws<FormatException>(() => SimpleElements.Delete("Number 2."));
        Assert.Throws<FormatException>(() => SimpleElements.Delete("Number#"));
        Assert.Throws<FormatException>(() => SimpleElements.Delete("Number/2."));
        Assert.Throws<FormatException>(() => SimpleElements.Delete("Number.exx"));
        Assert.Throws<FormatException>(() => SimpleElements.Delete("Number22!"));
        Assert.Throws<FormatException>(() => SimpleElements.Delete("Number "));

        Debug.Log("Save with null or empty keys");
        Assert.Throws<ArgumentNullException>(() => SimpleElements.Delete(""));

    }




    [Test]
    public void A500_DeleteAll_WillDeleteAllFiles()
    {
       
        // Save some files
        SimpleElements.Save("Uno", 33);
        SimpleElements.Save("Dos", 33);
        SimpleElements.Save("Tres", 33);
        SimpleElements.Save("Four", 33);
        SimpleElements.Save("Five", 33);
        SimpleElements.Save("Six", 33);
        Assert.IsTrue(SimpleElements.Count() == 6);

        Debug.Log("Delete all");
        var opp = SimpleElements.DeleteAll();
        Assert.IsTrue(opp);
        Assert.IsTrue(SimpleElements.Count() == 0);

    }


}