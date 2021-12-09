using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using U.DalilaDB;
using UnityEngine;
using UnityEngine.TestTools;

public class TimeLapseTest
{


    [Test]
    public void TimeLapse_Create()
    {
        TimeLapse tl;
        

        tl = new TimeLapse
        {
            seconds = 20,
        };

        Debug.Log(tl + "");
        Assert.AreEqual(20, tl.seconds);
        Assert.AreEqual(0, tl.minutes);
        Assert.AreEqual(0, tl.hours);
        Assert.AreEqual(0, tl.days);
        Assert.AreEqual(0, tl.months);
        Assert.AreEqual(0, tl.years);


        tl = new TimeLapse
        {
            seconds = 122,
        };

        Debug.Log(tl + "");
        Assert.AreEqual(2, tl.seconds);
        Assert.AreEqual(2, tl.minutes);
        Assert.AreEqual(0, tl.hours);
        Assert.AreEqual(0, tl.days);
        Assert.AreEqual(0, tl.months);
        Assert.AreEqual(0, tl.years);


        tl = new TimeLapse
        {
            minutes = 120,
        };

        Debug.Log(tl + "");
        Assert.AreEqual(0, tl.seconds);
        Assert.AreEqual(0, tl.minutes);
        Assert.AreEqual(2, tl.hours);
        Assert.AreEqual(0, tl.days);
        Assert.AreEqual(0, tl.months);
        Assert.AreEqual(0, tl.years);


        tl = new TimeLapse
        {
            hours = 49,
        };

        Debug.Log(tl + "");
        Assert.AreEqual(0, tl.seconds);
        Assert.AreEqual(0, tl.minutes);
        Assert.AreEqual(1, tl.hours);
        Assert.AreEqual(2, tl.days);
        Assert.AreEqual(0, tl.months);
        Assert.AreEqual(0, tl.years);


        tl = new TimeLapse
        {
            days = 30,
        };

        Debug.Log(tl + "");
        Assert.AreEqual(0, tl.seconds);
        Assert.AreEqual(0, tl.minutes);
        Assert.AreEqual(0, tl.hours);
        Assert.AreEqual(0, tl.days);
        Assert.AreEqual(1, tl.months);
        Assert.AreEqual(0, tl.years);



        tl = new TimeLapse
        {
            months = 27,
        };

        Debug.Log(tl + "");
        Assert.AreEqual(0, tl.seconds);
        Assert.AreEqual(0, tl.minutes);
        Assert.AreEqual(0, tl.hours);
        Assert.AreEqual(0, tl.days);
        Assert.AreEqual(3, tl.months);
        Assert.AreEqual(2, tl.years);


        tl = new TimeLapse
        {
            years = 23,
        };

        Debug.Log(tl + "");
        Assert.AreEqual(0, tl.seconds);
        Assert.AreEqual(0, tl.minutes);
        Assert.AreEqual(0, tl.hours);
        Assert.AreEqual(0, tl.days);
        Assert.AreEqual(0, tl.months);
        Assert.AreEqual(23, tl.years);

    }


    [Test]
    public void TimeLapse_OperatorAdd()
    {
        TimeLapse tl1, tl2, tl;


        tl1 = new TimeLapse
        {
            minutes = 2,
            seconds = 20,
        };

        tl2 = new TimeLapse
        {
            seconds = 50,
        };

        tl = tl1 + tl2;
        Debug.Log(tl + "");
        Assert.AreEqual(10, tl.seconds);
        Assert.AreEqual(3, tl.minutes);
        Assert.AreEqual(0, tl.hours);
        Assert.AreEqual(0, tl.days);
        Assert.AreEqual(0, tl.months);
        Assert.AreEqual(0, tl.years);

        tl = tl2 + tl1;
        Debug.Log(tl + "");
        Assert.AreEqual(10, tl.seconds);
        Assert.AreEqual(3, tl.minutes);
        Assert.AreEqual(0, tl.hours);
        Assert.AreEqual(0, tl.days);
        Assert.AreEqual(0, tl.months);
        Assert.AreEqual(0, tl.years);

    }


    [Test]
    public void TimeLapse_OperatorSubstract()
    {
        TimeLapse tl1, tl2, tl;


        tl1 = new TimeLapse
        {
            minutes = 2,
            seconds = 20,
        };

        tl2 = new TimeLapse
        {
            seconds = 50,
        };

        tl = tl1 - tl2;
        Debug.Log(tl + "");
        Assert.AreEqual(30, tl.seconds);
        Assert.AreEqual(1, tl.minutes);
        Assert.AreEqual(0, tl.hours);
        Assert.AreEqual(0, tl.days);
        Assert.AreEqual(0, tl.months);
        Assert.AreEqual(0, tl.years);

        tl = tl2 - tl1;
        Debug.Log(tl + "");
        Assert.AreEqual(0, tl.seconds);
        Assert.AreEqual(0, tl.minutes);
        Assert.AreEqual(0, tl.hours);
        Assert.AreEqual(0, tl.days);
        Assert.AreEqual(0, tl.months);
        Assert.AreEqual(0, tl.years);




        tl1 = new TimeLapse
        {
            years = 2,
        };

        tl2 = new TimeLapse
        {
            seconds = 50,
        };

        tl = tl1 - tl2;
        Debug.Log(tl + "");
        Assert.AreEqual(10, tl.seconds);
        Assert.AreEqual(59, tl.minutes);
        Assert.AreEqual(23, tl.hours);
        Assert.AreEqual(29, tl.days);
        Assert.AreEqual(11, tl.months);
        Assert.AreEqual(1, tl.years);

    }


    [Test]
    public void TimeLapse_OperatorEqual()
    {
        TimeLapse tl1, tl2, tl3;


        tl1 = new TimeLapse
        {
            minutes = 2,
            seconds = 20,
        };

        tl2 = new TimeLapse
        {
            minutes = 2,
            seconds = 20,
        };


        tl3 = new TimeLapse
        {
            seconds = 50,
        };

        Assert.IsTrue(tl1.Equals(tl2));
        Assert.IsTrue(tl1 == tl2);
        Assert.IsTrue(tl2.Equals(tl1));
        Assert.IsTrue(tl2 == tl1);

        Assert.IsFalse(tl1 == tl3);
        Assert.IsFalse(tl1.Equals(tl3));
        Assert.IsFalse(tl3 == tl1);
        Assert.IsFalse(tl3.Equals(tl1));

        Assert.IsFalse(tl1 != tl2);
        Assert.IsFalse(tl2 != tl1);

        Assert.IsTrue(tl1 != tl3);
        Assert.IsTrue(tl3 != tl1);

    }



    [Test]
    public void TimeLapse_OperatorGratherThan()
    {
        TimeLapse tl1, tl2, tl3;


        tl1 = new TimeLapse
        {
            minutes = 2,
            seconds = 20,
        };

        tl2 = new TimeLapse
        {
            minutes = 2,
            seconds = 20,
        };


        tl3 = new TimeLapse
        {
            seconds = 50,
        };


        Assert.IsTrue(tl1 > tl3);
        Assert.IsTrue(tl3 < tl1);

        Assert.IsTrue(tl1 >= tl2);
        Assert.IsTrue(tl2 <= tl1);

    }


    [Test]
    public void TimeLapse_CastFromInt()
    {
        TimeLapse tl1, tl2, tl3,tl;


        tl1 = new TimeLapse
        {
            minutes = 2,
            seconds = 20,
        };

        tl2 = new TimeLapse
        {
            minutes = 2,
            seconds = 20,
        };


        tl3 = new TimeLapse
        {
            seconds = 50,
        };

        tl = tl1 + 60;

        Debug.Log(tl + "");
        Assert.AreEqual(20, tl.seconds);
        Assert.AreEqual(3, tl.minutes);
        Assert.AreEqual(0, tl.hours);
        Assert.AreEqual(0, tl.days);
        Assert.AreEqual(0, tl.months);
        Assert.AreEqual(0, tl.years);


        tl = tl1 + 60.45f;

        Debug.Log(tl + "");
        Assert.AreEqual(20, tl.seconds);
        Assert.AreEqual(3, tl.minutes);
        Assert.AreEqual(0, tl.hours);
        Assert.AreEqual(0, tl.days);
        Assert.AreEqual(0, tl.months);
        Assert.AreEqual(0, tl.years);

    }



    [Test]
    public void TimeLapse_CastToUint()
    {
        TimeLapse tl1, tl2, tl3;


        tl1 = new TimeLapse
        {
            minutes = 2,
            seconds = 20,
        };

        tl2 = new TimeLapse
        {
            minutes = 2,
            seconds = 20,
        };


        tl3 = new TimeLapse
        {
            seconds = 50,
        };

        var sec = (uint)tl2;

        Debug.Log(sec);
        Assert.AreEqual(140, sec);

    }



    [Test]
    public void TimeLapse_CastToInt()
    {
        TimeLapse tl1, tl2, tl3;


        tl1 = new TimeLapse
        {
            minutes = 2,
            seconds = 20,
        };

        tl2 = new TimeLapse
        {
            minutes = 2,
            seconds = 20,
        };


        tl3 = new TimeLapse
        {
            seconds = 50,
        };

        var sec = (int)tl2;

        Debug.Log(sec);
        Assert.AreEqual(140, sec);

    }


}
