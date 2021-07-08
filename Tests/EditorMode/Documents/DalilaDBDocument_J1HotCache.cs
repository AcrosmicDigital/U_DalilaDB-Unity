using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using U.DalilaDB;
using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;

public class DalilaDocument_J1HotCache
{


    [KnownType(typeof(UserHotCache))]
    [DataContract()]
    class UserHotCache : DalilaDBDocument<UserHotCache>
    {

        [DataMember()]
        public int count;

        protected override string rootPath_ => Application.persistentDataPath;
    }

    [KnownType(typeof(UserNotCache))]
    [DataContract()]
    class UserNotCache : DalilaDBDocument<UserNotCache>
    {

        [DataMember()]
        public int count;

        // Disable hot cache
        protected override bool cacheSize_ => false;

        protected override string rootPath_ => Application.persistentDataPath;

    }




    [Test]
    public void DalilaDocument_HotCacheEnabled()
    {

        // Delete the directory to clear it
        try { File.Delete(UserHotCache.LocationPath); } catch (Exception) { }
        UserHotCache.Delete();


        // Save some file
        UnityEngine.Debug.Log("Saving");
        var user = new UserHotCache();
        user.count = 66;
        var opp = user.Save();
        Assert.IsTrue(opp);
        Assert.IsTrue(UserHotCache.Exist());

        // Read for First time the files and measure the time
        Stopwatch sw = new Stopwatch(); sw.Start();
        var fpp0 = UserHotCache.Find();
        Assert.IsTrue(fpp0.Data.count == 66);
        var file0 = fpp0.Data;
        UnityEngine.Debug.Log("First read Time: " + sw.ElapsedTicks);

        // Read for Second time the files and measure the time
        sw.Restart();
        var fpp1 = UserHotCache.Find();
        Assert.IsTrue(fpp1.Data.count == 66);
        var file1 = fpp1.Data;
        sw.Stop();
        UnityEngine.Debug.Log("Second read Time from cache: " + sw.ElapsedTicks);

        // Read for Thirth time the files and measure the time
        sw.Restart();
        var fpp2 = UserHotCache.Find();
        Assert.IsTrue(fpp2.Data.count == 66);
        var file2 = fpp2.Data;
        sw.Stop();
        UnityEngine.Debug.Log("Thirth read Time from cache: " + sw.ElapsedTicks);

        // Read for Thirth time the files and measure the time
        sw.Restart();
        var fpp3 = UserHotCache.Find();
        Assert.IsTrue(fpp3.Data.count == 66);
        var file3 = fpp3.Data;
        sw.Stop();
        UnityEngine.Debug.Log("Clear cache and read Time: " + sw.ElapsedTicks);

        // Check that the documents are diferent instances
        //UnityEngine.Debug.Log("check: file0 == file1: " + object.ReferenceEquals(file0, file1));
        Assert.IsFalse(object.ReferenceEquals(file0, file1));
        //UnityEngine.Debug.Log("check: file0 == file2: " + object.ReferenceEquals(file0, file2));
        Assert.IsFalse(object.ReferenceEquals(file0, file2));
        //UnityEngine.Debug.Log("check: file0 == file3: " + object.ReferenceEquals(file0, file3));
        Assert.IsFalse(object.ReferenceEquals(file0, file3));
        //UnityEngine.Debug.Log("check: file1 == file2: " + object.ReferenceEquals(file1, file2));
        Assert.IsFalse(object.ReferenceEquals(file1, file2));
        //UnityEngine.Debug.Log("check: file1 == file3: " + object.ReferenceEquals(file1, file3));
        Assert.IsFalse(object.ReferenceEquals(file1, file3));
        //UnityEngine.Debug.Log("check: file2 == file3: " + object.ReferenceEquals(file2, file3));
        Assert.IsFalse(object.ReferenceEquals(file2, file3));

    }

    [Test]
    public void DalilaDocument_HotCacheDisabled()
    {

        // Delete the directory to clear it
        try { File.Delete(UserNotCache.LocationPath); } catch (Exception) { }
        UserNotCache.Delete();


        // Save some file
        UnityEngine.Debug.Log("Saving");
        var user = new UserNotCache();
        user.count = 66;
        var opp = user.Save();
        Assert.IsTrue(opp);
        Assert.IsTrue(UserNotCache.Exist());

        // Read for First time the files and measure the time
        Stopwatch sw = new Stopwatch(); sw.Start();
        Assert.IsTrue(UserNotCache.Find().Data.count == 66);
        UnityEngine.Debug.Log("First read Time: " + sw.ElapsedTicks);

        // Read for Second time the files and measure the time
        sw.Restart();
        Assert.IsTrue(UserNotCache.Find().Data.count == 66);
        sw.Stop();
        UnityEngine.Debug.Log("Second read Time: " + sw.ElapsedTicks);



        // Read for Thirth time the files and measure the time
        sw.Restart();
        Assert.IsTrue(UserNotCache.Find().Data.count == 66);
        sw.Stop();
        UnityEngine.Debug.Log("Thirth read Time: " + sw.ElapsedTicks);

        // Read for Thirth time the files and measure the time
        sw.Restart();
        Assert.IsTrue(UserNotCache.Find().Data.count == 66);
        sw.Stop();
        UnityEngine.Debug.Log("Four read Time: " + sw.ElapsedTicks);

    }
}
