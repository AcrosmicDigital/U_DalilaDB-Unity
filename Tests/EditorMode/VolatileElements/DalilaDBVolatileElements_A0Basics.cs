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

public class DalilaVolatileElements_A0Basics
{


    #region Example classes

    class ElementsInDefaultPath : DalilaDBVolatileElements<ElementsInDefaultPath>
    {
        
        protected override int cacheSize_ => 20;
    }

    class ElementsChangePath : DalilaDBVolatileElements<ElementsChangePath>
    {
        protected override int cacheSize_ => 20;
    }

    class ElementsOne : DalilaDBVolatileElements<ElementsOne>
    {
        protected override int cacheSize_ => 21;
    }

    class ElementsTwo : DalilaDBVolatileElements<ElementsTwo>
    {
        protected override int cacheSize_ => 22;
    }

    class ElementsThree : DalilaDBVolatileElements<ElementsThree>
    {
        protected override int cacheSize_ => 23;
    }




    #endregion Example classes



    [SetUp]
    public void SetUp()
    {

        ElementsInDefaultPath.DeleteAll();
        ElementsChangePath.DeleteAll();
        ElementsOne.DeleteAll();
        ElementsTwo.DeleteAll();
        ElementsThree.DeleteAll();

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
    public void DalilaVolatileElements_005EnumKeysAndObjectsAsKeys()
    {

        // Uno

        // Save
        var saveOperation = ElementsInDefaultPath.Save(ElementsKeys.Uno, 32);
        Debug.Log("Save: " + saveOperation);
        Assert.IsTrue(saveOperation);

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



        // Dos

        // Save
        saveOperation = ElementsInDefaultPath.Save(ElementsKeys.Dos, 32);
        Debug.Log("Save: " + saveOperation);
        Assert.IsTrue(saveOperation);

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



        // Tres

        // Save
        saveOperation = ElementsInDefaultPath.Save(ElementsKeys.Tres, 32);
        Debug.Log("Save: " + saveOperation);
        Assert.IsTrue(saveOperation);

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

    }


    [Test]
    public void DalilaVolatileElements_100CountAndExist()
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
    public void DalilaVolatileElements_200MultiInstancesWork()
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
    public void DalilaVolatileElements_600Define()
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
    public void DalilaVolatileElements_700CacheSize()
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
            var opp = ElementsInDefaultPath.Save("FileB" + i, 30 + 1);
            Assert.IsFalse(opp); // Cant save becouse is full
            // Get again the cache
            var cache1 = ElementsInDefaultPath.CacheGet();
            // Compare cache
            var cmp = S.DiferencesArrays(cache1, cache0);
            Debug.Log("Diferents: " + cmp.Length);
            Assert.AreEqual(0, cmp.Length);   // If is full dont save a new element
        }



    }



}
