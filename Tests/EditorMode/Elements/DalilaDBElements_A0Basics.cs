using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using U.DalilaDB;
using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Linq;

public class DalilaElements_A0Basics
{


    #region Example classes

    class ElementsInDefaultPath : DalilaDBElements<ElementsInDefaultPath>
    {
        protected override string rootPath_ => Application.persistentDataPath;

        protected override int cacheSize_ => 20;
    }

    class ElementsChangePath : DalilaDBElements<ElementsChangePath>
    {
        protected override string rootPath_ => Application.persistentDataPath + "/OtherPath";

        protected override int cacheSize_ => 20;
    }

    class ElementsOne : DalilaDBElements<ElementsOne>
    {
        protected override string rootPath_ => Application.persistentDataPath + "/One";

        protected override int cacheSize_ => 21;
    }

    class ElementsTwo : DalilaDBElements<ElementsTwo>
    {
        protected override string rootPath_ => Application.persistentDataPath + "/Two";

        protected override int cacheSize_ => 22;
    }

    class ElementsThree : DalilaDBElements<ElementsThree>
    {
        protected override string rootPath_ => Application.persistentDataPath + "/Three";

        protected override int cacheSize_ => 23;
    }




    #endregion Example classes



    [SetUp]
    public void SetUp()
    {
        // Delete the directory to clear it
        try { Directory.Delete(ElementsInDefaultPath.LocationPath, true); } catch (Exception) { }
        try { Directory.Delete(ElementsChangePath.LocationPath, true); } catch (Exception) { }
        try { Directory.Delete(ElementsOne.LocationPath + "/One", true); } catch (Exception) { }
        try { Directory.Delete(ElementsTwo.LocationPath + "/Two", true); } catch (Exception) { }
        try { Directory.Delete(ElementsThree.LocationPath + "/Three", true); } catch (Exception) { }
        ElementsInDefaultPath.DeleteAll();
        ElementsChangePath.DeleteAll();
        ElementsOne.DeleteAll();
        ElementsTwo.DeleteAll();
        ElementsThree.DeleteAll();

    }




    [TestCase()]
    [TestCase()]
    public void DalilaElements_000PathsAndLocations()
    {

        // Check the new RootPath
        Debug.Log("Default RootPath: " + ElementsInDefaultPath.RootPath);
        Debug.Log("Change RootPath: " + ElementsChangePath.RootPath);
        Assert.IsTrue(ElementsInDefaultPath.RootPath == Application.persistentDataPath + "/");
        Assert.IsTrue(ElementsChangePath.RootPath == Application.persistentDataPath + "/OtherPath/");

        // Check the new Location
        Debug.Log("Default Location: " + ElementsInDefaultPath.Location);
        Debug.Log("Change Location: " + ElementsChangePath.Location);
        Assert.IsTrue(ElementsInDefaultPath.Location == "/DalilaDB/DalilaElements/ElementsInDefaultPath/");
        Assert.IsTrue(ElementsChangePath.Location == "/DalilaDB/DalilaElements/ElementsChangePath/");

        // Check the new ResourceLocation
        Debug.Log("Default ResourceLocation: " + ElementsInDefaultPath.ResourceLocation("KeyValue"));
        Debug.Log("Change ResourceLocation: " + ElementsChangePath.ResourceLocation("KeyValue"));
        Assert.IsTrue(ElementsInDefaultPath.ResourceLocation("KeyValue") == "/DalilaDB/DalilaElements/ElementsInDefaultPath/KeyValue.xml");
        Assert.IsTrue(ElementsChangePath.ResourceLocation("KeyValue") == "/DalilaDB/DalilaElements/ElementsChangePath/KeyValue.xml");

        // Check the new LocationPath
        Debug.Log("Default LocationPath: " + ElementsInDefaultPath.LocationPath);
        Debug.Log("Change LocationPath: " + ElementsChangePath.LocationPath);
        Assert.IsTrue(ElementsInDefaultPath.LocationPath == Application.persistentDataPath + "/DalilaDB/DalilaElements/ElementsInDefaultPath/");
        Assert.IsTrue(ElementsChangePath.LocationPath == Application.persistentDataPath + "/OtherPath/DalilaDB/DalilaElements/ElementsChangePath/");

        // Check the new ResourcePath
        Debug.Log("Default ResourcePath: " + ElementsInDefaultPath.ResourceLocation("KeyValue"));
        Debug.Log("Change ResourcePath: " + ElementsChangePath.ResourceLocation("KeyValue"));
        Assert.IsTrue(ElementsInDefaultPath.ResourcePath("KeyValue") == Application.persistentDataPath + "/DalilaDB/DalilaElements/ElementsInDefaultPath/KeyValue.xml");
        Assert.IsTrue(ElementsChangePath.ResourcePath("KeyValue") == Application.persistentDataPath + "/OtherPath/DalilaDB/DalilaElements/ElementsChangePath/KeyValue.xml");

    }


    // Enum to store the names
    enum ElementsKeys
    {
        Uno,
        Dos,
        Tres,
        Cuatro,
        Cinco,
    }
    [Test]
    public void DalilaElements_005EnumKeysAndObjectsAsKeys()
    {

        // Uno

        // Save
        var saveOperation = ElementsInDefaultPath.Save(ElementsKeys.Uno, 32);
        Debug.Log("Save: " + saveOperation);
        Assert.IsTrue(saveOperation);
        Assert.IsTrue(File.Exists(ElementsInDefaultPath.ResourcePath(ElementsKeys.Uno.ToString())));

        // Find
        var finddd = ElementsInDefaultPath.Find<int>(ElementsKeys.Uno);
        Debug.Log("Find: " + finddd);
        Assert.IsTrue(finddd);
        Assert.IsTrue(finddd.Data == 32);

        // Exist
        Debug.Log("Exist: " + ElementsInDefaultPath.Exist(ElementsKeys.Uno));
        Assert.IsTrue(ElementsInDefaultPath.Exist(ElementsKeys.Uno.ToString()));

        // Delete
        var deletedd = ElementsInDefaultPath.Delete(ElementsKeys.Uno);
        Debug.Log("Delete: " + deletedd);
        Assert.IsTrue(deletedd);
        Assert.IsFalse(File.Exists(ElementsInDefaultPath.ResourcePath(ElementsKeys.Uno.ToString())));



        // Dos

        // Save
        saveOperation = ElementsInDefaultPath.Save(ElementsKeys.Dos, 32);
        Debug.Log("Save: " + saveOperation);
        Assert.IsTrue(saveOperation);
        Assert.IsTrue(File.Exists(ElementsInDefaultPath.ResourcePath(ElementsKeys.Dos.ToString())));

        // Find
        finddd = ElementsInDefaultPath.Find<int>(ElementsKeys.Dos);
        Debug.Log("Find: " + finddd);
        Assert.IsTrue(finddd);
        Assert.IsTrue(finddd.Data == 32);

        // Exist
        Debug.Log("Exist: " + ElementsInDefaultPath.Exist(ElementsKeys.Dos));
        Assert.IsTrue(ElementsInDefaultPath.Exist(ElementsKeys.Dos.ToString()));

        // Delete
        deletedd = ElementsInDefaultPath.Delete(ElementsKeys.Dos);
        Debug.Log("Delete: " + deletedd);
        Assert.IsTrue(deletedd);
        Assert.IsFalse(File.Exists(ElementsInDefaultPath.ResourcePath(ElementsKeys.Dos.ToString())));



        // Tres

        // Save
        saveOperation = ElementsInDefaultPath.Save(ElementsKeys.Tres, 32);
        Debug.Log("Save: " + saveOperation);
        Assert.IsTrue(saveOperation);
        Assert.IsTrue(File.Exists(ElementsInDefaultPath.ResourcePath(ElementsKeys.Tres.ToString())));

        // Find
        finddd = ElementsInDefaultPath.Find<int>(ElementsKeys.Tres);
        Debug.Log("Find: " + finddd);
        Assert.IsTrue(finddd);
        Assert.IsTrue(finddd.Data == 32);

        // Exist
        Debug.Log("Exist: " + ElementsInDefaultPath.Exist(ElementsKeys.Tres));
        Assert.IsTrue(ElementsInDefaultPath.Exist(ElementsKeys.Tres.ToString()));

        // Delete
        deletedd = ElementsInDefaultPath.Delete(ElementsKeys.Tres);
        Debug.Log("Delete: " + deletedd);
        Assert.IsTrue(deletedd);
        Assert.IsFalse(File.Exists(ElementsInDefaultPath.ResourcePath(ElementsKeys.Tres.ToString())));

    }


    [Test]
    public void DalilaElements_100CountAndExist()
    {

        // When no resources saved

        // Count
        Assert.AreEqual(0, ElementsInDefaultPath.Count());

        // Exist with parameter
        Assert.IsFalse(ElementsInDefaultPath.Exist("One"));
        Assert.IsFalse(ElementsInDefaultPath.Exist("Two"));
        Assert.IsFalse(ElementsInDefaultPath.Exist("Three"));
        Assert.IsFalse(ElementsInDefaultPath.Exist("Four"));
        Assert.IsFalse(ElementsInDefaultPath.Exist("Five"));

        // Exist without  parameter
        var keys = ElementsInDefaultPath.Exist().ToList();

        foreach (var key in keys)
        {
            Debug.Log(key);
        }

        // Orther is not defined
        Assert.AreEqual(0, keys.Count);


        // When resources saved

        // Save
        ElementsInDefaultPath.Save("One", 31);
        ElementsInDefaultPath.Save("Two", 32);
        ElementsInDefaultPath.Save("Three", 33);
        ElementsInDefaultPath.Save("Four", 34);
        ElementsInDefaultPath.Save("Five", 35);

        // Count
        Assert.AreEqual(5, ElementsInDefaultPath.Count());

        // Exist with parameter
        Assert.AreEqual(true, ElementsInDefaultPath.Exist("One"));
        Assert.AreEqual(true, ElementsInDefaultPath.Exist("Two"));
        Assert.AreEqual(true, ElementsInDefaultPath.Exist("Three"));
        Assert.AreEqual(true, ElementsInDefaultPath.Exist("Four"));
        Assert.AreEqual(true, ElementsInDefaultPath.Exist("Five"));

        // Exist without  parameter
        keys = ElementsInDefaultPath.Exist().ToList();

        foreach (var key in keys)
        {
            Debug.Log(key);
        }

        // Orther is not defined
        Assert.IsTrue(keys.Contains("One"));
        Assert.IsTrue(keys.Contains("Two"));
        Assert.IsTrue(keys.Contains("Three"));
        Assert.IsTrue(keys.Contains("Four"));
        Assert.IsTrue(keys.Contains("Five"));

    }


    [Test]
    public void DalilaElements_200MultiInstancesWork()
    {
        
        // HotElementsMax
        Debug.Log("HotElementsMax");
        Debug.Log("One: " + ElementsOne.CacheSize);
        Assert.IsTrue(ElementsOne.CacheSize == 21);
        Debug.Log("Two: " + ElementsTwo.CacheSize);
        Assert.IsTrue(ElementsTwo.CacheSize == 22);
        Debug.Log("Three: " + ElementsThree.CacheSize);
        Assert.IsTrue(ElementsThree.CacheSize == 23);


        // Create a element with same key but diferent value
        ElementsOne.Save("data", "One");
        ElementsTwo.Save("data", "Two");
        ElementsThree.Save("data", "Three");

        // Find the diferent elements
        var dataOne = ElementsOne.Find<string>("data");
        Debug.Log("FindedOne: " + dataOne);
        Assert.AreEqual("One", dataOne.Data);

        var dataTwo = ElementsTwo.Find<string>("data");
        Debug.Log("FindedTwo: " + dataTwo);
        Assert.AreEqual("Two", dataTwo.Data);

        var dataThree = ElementsThree.Find<string>("data");
        Debug.Log("FindedThree: " + dataThree);
        Assert.AreEqual("Three", dataThree.Data);


        // Delete in One dont delete others
        ElementsOne.Delete("data");
        Assert.IsFalse(ElementsOne.Exist("data"));
        Assert.IsTrue(ElementsTwo.Exist("data"));
        Assert.IsTrue(ElementsThree.Exist("data"));

    }



    [Test]
    public void DalilaElements_600Define()
    {

        // When no resources saved
        ElementsInDefaultPath.Define();


        // Save
        ElementsInDefaultPath.Save("One", 31);
        ElementsInDefaultPath.Save("Two", 32);
        ElementsInDefaultPath.Save("Three", 33);
        ElementsInDefaultPath.Save("Four", 34);
        ElementsInDefaultPath.Save("Five", 35);


        // When resources saved
        ElementsInDefaultPath.Define();



    }



    [Test]
    public void DalilaElements_700CacheSize()
    {

        // When no resources saved

        // Save 21 that are the full size of cache
        Debug.Log("Save");
        for (int i = 0; i < 21; i++)
        {
            ElementsInDefaultPath.Save("File" + i, 30 + 1);
        }


        // Save when cache is full will remove one element and add the new one
        for (int i = 0; i < 10; i++)
        {
            // When resources saved
            // Get the list of strings
            var cache0 = ElementsInDefaultPath.CacheGet();
            // Save a new element
            ElementsInDefaultPath.Save("FileB" + i, 30 + 1);
            // Get again the cache
            var cache1 = ElementsInDefaultPath.CacheGet();
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
