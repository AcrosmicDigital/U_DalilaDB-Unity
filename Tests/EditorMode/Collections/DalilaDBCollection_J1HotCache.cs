using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using NUnit.Framework;
using U.DalilaDB;
using UnityEngine;
using UnityEngine.TestTools;

public class DalilaCollection_J1HotCache
{

    #region Example classes

    [KnownType(typeof(UserHotCache))]
    [DataContract()]
    class UserHotCache : DalilaDBCollection<UserHotCache>
    {

        [DataMember()]
        public int count;

        protected override string rootPath_ => Application.persistentDataPath;

    }

    [KnownType(typeof(UserNotCache))]
    [DataContract()]
    class UserNotCache : DalilaDBCollection<UserNotCache>
    {

        // Disable hot cache
        protected override int cacheSize_ => 0;


        [DataMember()]
        public int count;

        protected override string rootPath_ => Application.persistentDataPath;

    }




    #endregion Example classes




    [Test]
    public void DalilaCollection_HotCacheEnabled()
    {


        // Delete the directory to clear it
        try { Directory.Delete(UserHotCache.LocationPath, true); } catch (Exception) { }
        UserHotCache.DeleteAll();



        // Create a new doocument

        var newd1 = new UserHotCache { count = 66, };
        var newd2 = new UserHotCache { count = 66, };
        var newd3 = new UserHotCache { count = 66, };
        var newd4 = new UserHotCache { count = 66, };
        var newd5 = new UserHotCache { count = 66, };
        var newd6 = new UserHotCache { count = 66, };
        var newd7 = new UserHotCache { count = 66, };
        var newd8 = new UserHotCache { count = 66, };
        Assert.IsTrue(newd1.Save());
        Assert.IsTrue(newd2.Save());
        Assert.IsTrue(newd3.Save());
        Assert.IsTrue(newd4.Save());
        Assert.IsTrue(newd5.Save());
        Assert.IsTrue(newd6.Save());
        Assert.IsTrue(newd7.Save());
        Assert.IsTrue(newd8.Save());
        Assert.IsTrue(UserHotCache.Count() == 8);

        // FindAll documents
        UnityEngine.Debug.Log("Reading First time");
        UserHotCache.Define();
        Stopwatch sw = new Stopwatch(); sw.Start();
        var finded1 = UserHotCache.FindAll().Data;
        UnityEngine.Debug.Log("First read Time: " + sw.ElapsedTicks);

        // FindAll documents from cache
        UnityEngine.Debug.Log("Reading Second time");
        UserHotCache.Define();
        sw = new Stopwatch(); sw.Start();
        var finded2 = UserHotCache.FindAll().Data;
        UnityEngine.Debug.Log("from cache read Time: " + sw.ElapsedTicks);

        // FindAll documents from cache
        UnityEngine.Debug.Log("Reading Thirth time");
        UserHotCache.Define();
        sw = new Stopwatch(); sw.Start();
        var finded3 = UserHotCache.FindAll().Data;
        UnityEngine.Debug.Log("from cache read Time: " + sw.ElapsedTicks);

        // FindAll documents clear cache
        UnityEngine.Debug.Log("Reading After Clear cache time");
        UserHotCache.Define();
        sw = new Stopwatch(); sw.Start();
        var finded4 = UserHotCache.FindAll().Data;
        UnityEngine.Debug.Log("AfterClearCache read Time: " + sw.ElapsedTicks);



        for (int i = 0; i < finded1.Length; i++)
        {
            Assert.IsFalse(object.ReferenceEquals(finded1[i], finded2[i]));
            Assert.IsFalse(object.ReferenceEquals(finded1[i], finded3[i]));
            Assert.IsFalse(object.ReferenceEquals(finded1[i], finded4[i]));
            Assert.IsFalse(object.ReferenceEquals(finded2[i], finded3[i]));
            Assert.IsFalse(object.ReferenceEquals(finded2[i], finded4[i]));
            Assert.IsFalse(object.ReferenceEquals(finded3[i], finded4[i]));
        }
    }



    [Test]
    public void DalilaCollection_HotCacheDisabled()
    {


        // Delete the directory to clear it
        try { Directory.Delete(UserNotCache.LocationPath, true); } catch (Exception) { }
        UserNotCache.DeleteAll();



        // Create a new doocument

        var newd1 = new UserNotCache { count = 66, };
        var newd2 = new UserNotCache { count = 66, };
        var newd3 = new UserNotCache { count = 66, };
        var newd4 = new UserNotCache { count = 66, };
        var newd5 = new UserNotCache { count = 66, };
        var newd6 = new UserNotCache { count = 66, };
        var newd7 = new UserNotCache { count = 66, };
        var newd8 = new UserNotCache { count = 66, };

        Assert.IsTrue(newd1.Save());

        Assert.IsTrue(newd2.Save());
        Assert.IsTrue(newd3.Save());
        Assert.IsTrue(newd4.Save());
        Assert.IsTrue(newd5.Save());
        Assert.IsTrue(newd6.Save());
        Assert.IsTrue(newd7.Save());
        Assert.IsTrue(newd8.Save());

        Assert.IsTrue(UserNotCache.Count() == 8);

        // FindAll documents

        Stopwatch sw = new Stopwatch(); sw.Start();
        var finded1 = UserNotCache.FindAll().Data;
        UnityEngine.Debug.Log("First read Time: " + sw.ElapsedTicks);

        // FindAll documents from cache
        sw = new Stopwatch(); sw.Start();
        var finded2 = UserNotCache.FindAll().Data;
        UnityEngine.Debug.Log("from cache read Time: " + sw.ElapsedTicks);

        // FindAll documents
        sw = new Stopwatch(); sw.Start();
        var finded3 = UserNotCache.FindAll().Data;
        UnityEngine.Debug.Log("from cache read Time: " + sw.ElapsedTicks);

        // FindAll documents
        sw = new Stopwatch(); sw.Start();
        var finded4 = UserNotCache.FindAll().Data;
        UnityEngine.Debug.Log("AfterClearCache read Time: " + sw.ElapsedTicks);



        for (int i = 0; i < finded1.Length; i++)
        {
            Assert.IsFalse(object.ReferenceEquals(finded1[i], finded2[i]));
            Assert.IsFalse(object.ReferenceEquals(finded1[i], finded3[i]));
            Assert.IsFalse(object.ReferenceEquals(finded1[i], finded4[i]));
            Assert.IsFalse(object.ReferenceEquals(finded2[i], finded3[i]));
            Assert.IsFalse(object.ReferenceEquals(finded2[i], finded4[i]));
            Assert.IsFalse(object.ReferenceEquals(finded3[i], finded4[i]));
        }
    }

}