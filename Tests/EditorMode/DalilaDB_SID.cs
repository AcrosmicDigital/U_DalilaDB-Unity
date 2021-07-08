using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using U.DalilaDB;
using System;

public class Dalila_SID
{
    // A Test behaves as an ordinary method
    [Test]
    public void SID_MustBe19CharsLongAndUniqueIn1000000OfCreates()
    {
        var lastSid = new SID();

        for (int i = 0; i < 1000000; i++)
        {
            var newSid = new SID();
            Assert.IsTrue(newSid.ToString().Length == 19);
            Assert.IsTrue(newSid != lastSid);
            //Debug.Log("Last: " + lastSid + " New: " + newSid);
            lastSid = newSid;
        }
    }

    [TestCase("1903894768347583745")]
    [TestCase("1098345878345938744")]
    [TestCase("0000000000000000000")]
    [TestCase("2309435879043589439")]
    [TestCase("0000999999922222203")]
    [TestCase("3209482309482309099")]
    public void SID_CanBecreatedFromAStringOf19NumericChars(string sidStr)
    {
        var sid = new SID(sidStr);
        Assert.IsTrue(sid.ToString().Length == 19);
    }


    [TestCase(null)]
    [TestCase("")]
    public void SID_CanBeCreatedFromNullOrEmptyString(string sidStr)
    {
        Assert.Throws<ArgumentNullException>(() => { var sid = new SID(sidStr); });
    }


    [TestCase("10029")]
    [TestCase("092039312493214329423932")]
    [TestCase("230943587904358943")]
    [TestCase("23094358790435894396")]
    [TestCase("euimdscldscsdcjkdsc")]
    [TestCase("                   ")]
    [TestCase("94903j349r9349r994j")]
    [TestCase("329.2332321.2321222")]
    [TestCase("10029 234234 343243")]
    [TestCase("10029-323-3234-3433")]
    public void SID_CanBeCreatedFromInvalidString(string sidStr)
    {
        Assert.Throws<FormatException>(() => { var sid = new SID(sidStr); });
    }

    
    [Test]
    public void SID_EqualsOperatorOverload()
    {
        Debug.Log("SID == SID");
        Assert.IsTrue(new SID("0000000000000000000") == new SID("0000000000000000000"));

        Debug.Log("SID == SID");
        Assert.IsFalse(new SID("0000000000000000001") == new SID("0000000000000000000"));

        Debug.Log("SID != SID");
        Assert.IsTrue(new SID("0000000000000000001") != new SID("0000000000000000000"));

        Debug.Log("SID != SID");
        Assert.IsFalse(new SID("0000000000000000000") != new SID("0000000000000000000"));


        Debug.Log("SID.Equals(SID)");
        Assert.IsTrue(new SID("0000000000000000000").Equals(new SID("0000000000000000000")));

        Debug.Log("SID.Equals(SID)");
        Assert.IsFalse(new SID("0000000000000000001").Equals(new SID("0000000000000000000")));





        Debug.Log("SID.Equals(null)");
        Assert.IsFalse(new SID("0000000000000000000").Equals(null));


        Debug.Log("null.Equals(SID)");
        SID sid = null;
        Assert.Throws<NullReferenceException>(() => sid.Equals(new SID("0000000000000000000")));


        Debug.Log("SID == null");
        Assert.IsFalse(new SID("0000000000000000001") == null);

        Debug.Log("SID != null");
        Assert.IsTrue(new SID("0000000000000000000") != null);

        Debug.Log("null == null");
        SID _id = null;
        Assert.IsTrue(_id == null);
        Assert.IsTrue(null == _id);

        Debug.Log("null != null");
        SID __id = null;
        Assert.IsFalse(__id != null);
        Assert.IsFalse(null != __id);
    }

}
