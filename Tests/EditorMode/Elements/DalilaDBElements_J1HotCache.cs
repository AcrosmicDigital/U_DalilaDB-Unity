using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using U.DalilaDB;
using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Diagnostics;

public class DalilaElements_J1HotCache
{

    #region Example classes

    class SimpleElements : DalilaDBElements<SimpleElements>
    {
        protected override string rootPath_ => Application.persistentDataPath;

    }

    class NoCacheElements : DalilaDBElements<NoCacheElements>
    {
        protected override string rootPath_ => Application.persistentDataPath;

        protected override int cacheSize_ => 0;

    }

    #endregion Example classes


    [SetUp]
    public void SetUp()
    {
        // Delete the directory to clear it
        try { Directory.Delete(SimpleElements.LocationPath, true); } catch (Exception) { }
        try { Directory.Delete(NoCacheElements.LocationPath, true); } catch (Exception) { }
        SimpleElements.DeleteAll();
        NoCacheElements.DeleteAll();

    }


    [Test]
    public void DalilaElements_HotCacheEnabled_ValueTypes()
    {
        
        // Save some files
        SimpleElements.Save("One", 33);
        SimpleElements.Save("Two", 33);
        SimpleElements.Save("Three", 33);
        SimpleElements.Save("Four", 33);
        SimpleElements.Save("Five", 33);
        SimpleElements.Save("Six", 33);
        Assert.IsTrue(SimpleElements.Count() == 6);


        // Read for First time the files and measure the time
        Stopwatch sw = new Stopwatch(); sw.Start();
        var ob11 = SimpleElements.Find<int>("One").Data;
        Assert.IsTrue(ob11 == 33);
        var ob21 = SimpleElements.Find<int>("Two").Data;
        Assert.IsTrue(ob21 == 33);
        var ob31 = SimpleElements.Find<int>("Three").Data;
        Assert.IsTrue(ob31 == 33);
        var ob41 = SimpleElements.Find<int>("Four").Data;
        Assert.IsTrue(ob41 == 33);
        var ob51 = SimpleElements.Find<int>("Five").Data;
        Assert.IsTrue(ob51 == 33);
        var ob61 = SimpleElements.Find<int>("Six").Data;
        Assert.IsTrue(ob61 == 33);
        sw.Stop();
        UnityEngine.Debug.Log("First read Time: " + sw.ElapsedTicks);

        // Read for Second time the files and measure the time
        sw.Restart();
        var ob12 = SimpleElements.Find<int>("One").Data;
        Assert.IsTrue(ob12 == 33);
        var ob22 = SimpleElements.Find<int>("Two").Data;
        Assert.IsTrue(ob22 == 33);
        var ob32 = SimpleElements.Find<int>("Three").Data;
        Assert.IsTrue(ob32 == 33);
        var ob42 = SimpleElements.Find<int>("Four").Data;
        Assert.IsTrue(ob42 == 33);
        var ob52 = SimpleElements.Find<int>("Five").Data;
        Assert.IsTrue(ob52 == 33);
        var ob62 = SimpleElements.Find<int>("Six").Data;
        Assert.IsTrue(ob62 == 33);
        sw.Stop();
        UnityEngine.Debug.Log("Second read Time from cache: " + sw.ElapsedTicks);


        // Read for Thirth time the files and measure the time
        sw.Restart();
        var ob13 = SimpleElements.Find<int>("One").Data;
        Assert.IsTrue(ob13 == 33);
        var ob23 = SimpleElements.Find<int>("Two").Data;
        Assert.IsTrue(ob23 == 33);
        var ob33 = SimpleElements.Find<int>("Three").Data;
        Assert.IsTrue(ob33 == 33);
        var ob43 = SimpleElements.Find<int>("Four").Data;
        Assert.IsTrue(ob43 == 33);
        var ob53 = SimpleElements.Find<int>("Five").Data;
        Assert.IsTrue(ob53 == 33);
        var ob63 = SimpleElements.Find<int>("Six").Data;
        Assert.IsTrue(ob63 == 33);
        sw.Stop();
        UnityEngine.Debug.Log("Thirth read Time from cache: " + sw.ElapsedTicks);

        // Read for Thirth time the files and measure the time
        sw.Restart();
        var ob14 = SimpleElements.Find<int>("One").Data;
        Assert.IsTrue(ob14 == 33);
        var ob24 = SimpleElements.Find<int>("Two").Data;
        Assert.IsTrue(ob24 == 33);
        var ob34 = SimpleElements.Find<int>("Three").Data;
        Assert.IsTrue(ob34 == 33);
        var ob44 = SimpleElements.Find<int>("Four").Data;
        Assert.IsTrue(ob44 == 33);
        var ob54 = SimpleElements.Find<int>("Five").Data;
        Assert.IsTrue(ob54 == 33);
        var ob64 = SimpleElements.Find<int>("Six").Data;
        Assert.IsTrue(ob64 == 33);
        sw.Stop();
        UnityEngine.Debug.Log("Four read Time clearing the cache: " + sw.ElapsedTicks);


        //UnityEngine.Debug.Log("References");
        Assert.IsFalse(object.ReferenceEquals(ob11, ob12));
        Assert.IsFalse(object.ReferenceEquals(ob11, ob13));
        Assert.IsFalse(object.ReferenceEquals(ob11, ob14));
        Assert.IsFalse(object.ReferenceEquals(ob12, ob13));
        Assert.IsFalse(object.ReferenceEquals(ob12, ob14));
        Assert.IsFalse(object.ReferenceEquals(ob13, ob14));

        //UnityEngine.Debug.Log("References");
        Assert.IsFalse(object.ReferenceEquals(ob21, ob22));
        Assert.IsFalse(object.ReferenceEquals(ob21, ob23));
        Assert.IsFalse(object.ReferenceEquals(ob21, ob24));
        Assert.IsFalse(object.ReferenceEquals(ob22, ob23));
        Assert.IsFalse(object.ReferenceEquals(ob22, ob24));
        Assert.IsFalse(object.ReferenceEquals(ob23, ob24));

        //UnityEngine.Debug.Log("References");
        Assert.IsFalse(object.ReferenceEquals(ob31, ob32));
        Assert.IsFalse(object.ReferenceEquals(ob31, ob33));
        Assert.IsFalse(object.ReferenceEquals(ob31, ob34));
        Assert.IsFalse(object.ReferenceEquals(ob32, ob33));
        Assert.IsFalse(object.ReferenceEquals(ob32, ob34));
        Assert.IsFalse(object.ReferenceEquals(ob33, ob34));

        //UnityEngine.Debug.Log("References");
        Assert.IsFalse(object.ReferenceEquals(ob41, ob42));
        Assert.IsFalse(object.ReferenceEquals(ob41, ob43));
        Assert.IsFalse(object.ReferenceEquals(ob41, ob44));
        Assert.IsFalse(object.ReferenceEquals(ob42, ob43));
        Assert.IsFalse(object.ReferenceEquals(ob42, ob44));
        Assert.IsFalse(object.ReferenceEquals(ob43, ob44));

        //UnityEngine.Debug.Log("References");
        Assert.IsFalse(object.ReferenceEquals(ob51, ob52));
        Assert.IsFalse(object.ReferenceEquals(ob51, ob53));
        Assert.IsFalse(object.ReferenceEquals(ob51, ob54));
        Assert.IsFalse(object.ReferenceEquals(ob52, ob53));
        Assert.IsFalse(object.ReferenceEquals(ob52, ob54));
        Assert.IsFalse(object.ReferenceEquals(ob53, ob54));

       // UnityEngine.Debug.Log("References");
        Assert.IsFalse(object.ReferenceEquals(ob61, ob62));
        Assert.IsFalse(object.ReferenceEquals(ob61, ob63));
        Assert.IsFalse(object.ReferenceEquals(ob61, ob64));
        Assert.IsFalse(object.ReferenceEquals(ob62, ob63));
        Assert.IsFalse(object.ReferenceEquals(ob62, ob64));
        Assert.IsFalse(object.ReferenceEquals(ob63, ob64));
    }

    [Serializable]
    class Pdata
    {
        public int count;
    }
    [Test]
    public void DalilaElements_HotCacheEnabled_ReferenceTypes()
    {
        
        // Save some files
        SimpleElements.Save("One", new Pdata { count = 33 });
        SimpleElements.Save("Two", new Pdata { count = 33 });
        SimpleElements.Save("Three", new Pdata { count = 33 });
        SimpleElements.Save("Four", new Pdata { count = 33 });
        SimpleElements.Save("Five", new Pdata { count = 33 });
        SimpleElements.Save("Six", new Pdata { count = 33 });
        Assert.IsTrue(SimpleElements.Count() == 6);


        // Read for First time the files and measure the time
        Stopwatch sw = new Stopwatch(); sw.Start();
        var ob11 = SimpleElements.Find<Pdata>("One").Data;
        Assert.IsTrue(ob11.count == 33);
        var ob21 = SimpleElements.Find<Pdata>("Two").Data;
        Assert.IsTrue(ob21.count == 33);
        var ob31 = SimpleElements.Find<Pdata>("Three").Data;
        Assert.IsTrue(ob31.count == 33);
        var ob41 = SimpleElements.Find<Pdata>("Four").Data;
        Assert.IsTrue(ob41.count == 33);
        var ob51 = SimpleElements.Find<Pdata>("Five").Data;
        Assert.IsTrue(ob51.count == 33);
        var ob61 = SimpleElements.Find <Pdata>("Six").Data;
        Assert.IsTrue(ob61.count == 33);
        sw.Stop();
        UnityEngine.Debug.Log("First read Time: " + sw.ElapsedTicks);

        // Read for Second time the files and measure the time
        sw.Restart();
        var ob12 = SimpleElements.Find<Pdata>("One").Data;
        Assert.IsTrue(ob12.count == 33);
        var ob22 = SimpleElements.Find<Pdata>("Two").Data;
        Assert.IsTrue(ob22.count == 33);
        var ob32 = SimpleElements.Find<Pdata>("Three").Data;
        Assert.IsTrue(ob32.count == 33);
        var ob42 = SimpleElements.Find<Pdata>("Four").Data;
        Assert.IsTrue(ob42.count == 33);
        var ob52 = SimpleElements.Find<Pdata>("Five").Data;
        Assert.IsTrue(ob52.count == 33);
        var ob62 = SimpleElements.Find<Pdata>("Six").Data;
        Assert.IsTrue(ob62.count == 33);
        sw.Stop();
        UnityEngine.Debug.Log("Second read Time from cache: " + sw.ElapsedTicks);


        // Read for Thirth time the files and measure the time
        sw.Restart();
        var ob13 = SimpleElements.Find<Pdata>("One").Data;
        Assert.IsTrue(ob13.count == 33);
        var ob23 = SimpleElements.Find<Pdata>("Two").Data;
        Assert.IsTrue(ob23.count == 33);
        var ob33 = SimpleElements.Find<Pdata>("Three").Data;
        Assert.IsTrue(ob33.count == 33);
        var ob43 = SimpleElements.Find<Pdata>("Four").Data;
        Assert.IsTrue(ob43.count == 33);
        var ob53 = SimpleElements.Find<Pdata>("Five").Data;
        Assert.IsTrue(ob53.count == 33);
        var ob63 = SimpleElements.Find<Pdata>("Six").Data;
        Assert.IsTrue(ob63.count == 33);
        sw.Stop();
        UnityEngine.Debug.Log("Thirth read Time from cache: " + sw.ElapsedTicks);

        // Read for Thirth time the files and measure the time
        sw.Restart();
        var ob14 = SimpleElements.Find<Pdata>("One").Data;
        Assert.IsTrue(ob14.count == 33);
        var ob24 = SimpleElements.Find<Pdata>("Two").Data;
        Assert.IsTrue(ob24.count == 33);
        var ob34 = SimpleElements.Find<Pdata>("Three").Data;
        Assert.IsTrue(ob34.count == 33);
        var ob44 = SimpleElements.Find<Pdata>("Four").Data;
        Assert.IsTrue(ob44.count == 33);
        var ob54 = SimpleElements.Find<Pdata>("Five").Data;
        Assert.IsTrue(ob54.count == 33);
        var ob64 = SimpleElements.Find<Pdata>("Six").Data;
        Assert.IsTrue(ob64.count == 33);
        sw.Stop();
        UnityEngine.Debug.Log("Four read Time clearing the cache: " + sw.ElapsedTicks);


        //UnityEngine.Debug.Log("References");
        Assert.IsFalse(object.ReferenceEquals(ob11, ob12));
        Assert.IsFalse(object.ReferenceEquals(ob11, ob13));
        Assert.IsFalse(object.ReferenceEquals(ob11, ob14));
        Assert.IsFalse(object.ReferenceEquals(ob12, ob13));
        Assert.IsFalse(object.ReferenceEquals(ob12, ob14));
        Assert.IsFalse(object.ReferenceEquals(ob13, ob14));

        //UnityEngine.Debug.Log("References");
        Assert.IsFalse(object.ReferenceEquals(ob21, ob22));
        Assert.IsFalse(object.ReferenceEquals(ob21, ob23));
        Assert.IsFalse(object.ReferenceEquals(ob21, ob24));
        Assert.IsFalse(object.ReferenceEquals(ob22, ob23));
        Assert.IsFalse(object.ReferenceEquals(ob22, ob24));
        Assert.IsFalse(object.ReferenceEquals(ob23, ob24));

        //UnityEngine.Debug.Log("References");
        Assert.IsFalse(object.ReferenceEquals(ob31, ob32));
        Assert.IsFalse(object.ReferenceEquals(ob31, ob33));
        Assert.IsFalse(object.ReferenceEquals(ob31, ob34));
        Assert.IsFalse(object.ReferenceEquals(ob32, ob33));
        Assert.IsFalse(object.ReferenceEquals(ob32, ob34));
        Assert.IsFalse(object.ReferenceEquals(ob33, ob34));

        //UnityEngine.Debug.Log("References");
        Assert.IsFalse(object.ReferenceEquals(ob41, ob42));
        Assert.IsFalse(object.ReferenceEquals(ob41, ob43));
        Assert.IsFalse(object.ReferenceEquals(ob41, ob44));
        Assert.IsFalse(object.ReferenceEquals(ob42, ob43));
        Assert.IsFalse(object.ReferenceEquals(ob42, ob44));
        Assert.IsFalse(object.ReferenceEquals(ob43, ob44));

        //UnityEngine.Debug.Log("References");
        Assert.IsFalse(object.ReferenceEquals(ob51, ob52));
        Assert.IsFalse(object.ReferenceEquals(ob51, ob53));
        Assert.IsFalse(object.ReferenceEquals(ob51, ob54));
        Assert.IsFalse(object.ReferenceEquals(ob52, ob53));
        Assert.IsFalse(object.ReferenceEquals(ob52, ob54));
        Assert.IsFalse(object.ReferenceEquals(ob53, ob54));

        //UnityEngine.Debug.Log("References");
        Assert.IsFalse(object.ReferenceEquals(ob61, ob62));
        Assert.IsFalse(object.ReferenceEquals(ob61, ob63));
        Assert.IsFalse(object.ReferenceEquals(ob61, ob64));
        Assert.IsFalse(object.ReferenceEquals(ob62, ob63));
        Assert.IsFalse(object.ReferenceEquals(ob62, ob64));
        Assert.IsFalse(object.ReferenceEquals(ob63, ob64));
    }


    [Test]
    public void DalilaElements_HotCacheDisabled()
    {

        // Save some files
        NoCacheElements.Save("One", 33);
        NoCacheElements.Save("Two", 33);
        NoCacheElements.Save("Three", 33);
        NoCacheElements.Save("Four", 33);
        NoCacheElements.Save("Five", 33);
        NoCacheElements.Save("Six", 33);
        Assert.IsTrue(NoCacheElements.Count() == 6);


        // Read for First time the files and measure the time
        Stopwatch sw = new Stopwatch(); sw.Start();
        Assert.IsTrue(NoCacheElements.Find<int>("One").Data == 33);
        Assert.IsTrue(NoCacheElements.Find<int>("Two").Data == 33);
        Assert.IsTrue(NoCacheElements.Find<int>("Three").Data == 33);
        Assert.IsTrue(NoCacheElements.Find<int>("Four").Data == 33);
        Assert.IsTrue(NoCacheElements.Find<int>("Five").Data == 33);
        Assert.IsTrue(NoCacheElements.Find<int>("Six").Data == 33);
        sw.Stop();
        UnityEngine.Debug.Log("First read Time: " + sw.ElapsedTicks);

        // Read for Second time the files and measure the time
        sw.Restart();
        Assert.IsTrue(NoCacheElements.Find<int>("One").Data == 33);
        Assert.IsTrue(NoCacheElements.Find<int>("Two").Data == 33);
        Assert.IsTrue(NoCacheElements.Find<int>("Three").Data == 33);
        Assert.IsTrue(NoCacheElements.Find<int>("Four").Data == 33);
        Assert.IsTrue(NoCacheElements.Find<int>("Five").Data == 33);
        Assert.IsTrue(NoCacheElements.Find<int>("Six").Data == 33);
        sw.Stop();
        UnityEngine.Debug.Log("Second read Time: " + sw.ElapsedTicks);


        // Read for Thirth time the files and measure the time
        sw.Restart();
        Assert.IsTrue(NoCacheElements.Find<int>("One").Data == 33);
        Assert.IsTrue(NoCacheElements.Find<int>("Two").Data == 33);
        Assert.IsTrue(NoCacheElements.Find<int>("Three").Data == 33);
        Assert.IsTrue(NoCacheElements.Find<int>("Four").Data == 33);
        Assert.IsTrue(NoCacheElements.Find<int>("Five").Data == 33);
        Assert.IsTrue(NoCacheElements.Find<int>("Six").Data == 33);
        sw.Stop();
        UnityEngine.Debug.Log("Thirth read Time: " + sw.ElapsedTicks);

        // Read for Thirth time the files and measure the time
        sw.Restart();
        Assert.IsTrue(NoCacheElements.Find<int>("One").Data == 33);
        Assert.IsTrue(NoCacheElements.Find<int>("Two").Data == 33);
        Assert.IsTrue(NoCacheElements.Find<int>("Three").Data == 33);
        Assert.IsTrue(NoCacheElements.Find<int>("Four").Data == 33);
        Assert.IsTrue(NoCacheElements.Find<int>("Five").Data == 33);
        Assert.IsTrue(NoCacheElements.Find<int>("Six").Data == 33);
        sw.Stop();
        UnityEngine.Debug.Log("Four read Time: " + sw.ElapsedTicks);

    }

}
