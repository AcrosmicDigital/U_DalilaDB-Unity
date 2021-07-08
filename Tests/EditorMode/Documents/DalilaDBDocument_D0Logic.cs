using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using U.DalilaDB;
using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Runtime.Serialization;

public class DalilaDocument_D0Logic
{

    #region Example classes

    [KnownType(typeof(UserDocument))]
    [DataContract()]
    class UserDocument : DalilaDBDocument<UserDocument>
    {

        [DataMember()]
        public int count;

        protected override string rootPath_ => Application.persistentDataPath;

    }

    #endregion Example classes


    [SetUp]
    public void SetUp()
    {
        // Delete the directory to clear it
        try { Directory.Delete(UserDocument.LocationPath, true); } catch (Exception) { }
        UserDocument.Delete();

    }




    [Test]
    public void A000_Save()
    {

        // You can create multiple instances of the class
        var user1 = new UserDocument(); user1.count = 33;
        var user2 = new UserDocument(); user2.count = 44;
        var user3 = new UserDocument(); user3.count = 55;
        var user4 = new UserDocument(); user4.count = 66;
        var user5 = new UserDocument(); user5.count = 77;

        // And you can save all of them
        Debug.Log("Saving");
        Assert.IsTrue(user5.Save());
        Assert.IsTrue(user4.Save());
        Assert.IsTrue(user3.Save());
        Assert.IsTrue(user2.Save());
        Assert.IsTrue(user1.Save());

        // But only the las save is stored in memory, value finded will be always the last save
        Assert.IsTrue(UserDocument.Find().Data.count == 33);

        // You can save again
        Assert.IsTrue(user4.Save());

        // And data finded will be this new save
        Assert.IsTrue(UserDocument.Find().Data.count == 66);

    }

    [Test]
    public void A100_Find()
    {

        // You can create multiple instances of the class
        var user1 = new UserDocument(); user1.count = 33;
        var user2 = new UserDocument(); user2.count = 44;
        var user3 = new UserDocument(); user3.count = 55;
        var user4 = new UserDocument(); user4.count = 66;
        var user5 = new UserDocument(); user5.count = 77;

        // If you dont save, the document dont exist
        Assert.IsFalse(UserDocument.Exist());
        Assert.IsFalse(UserDocument.Find());
        Assert.IsTrue(UserDocument.Find().Data == null);

        // When you save
        Debug.Log("Saving");
        Assert.IsTrue(user5.Save());
        Assert.IsTrue(user4.Save());
        Assert.IsTrue(user3.Save());
        Assert.IsTrue(user2.Save());
        Assert.IsTrue(user1.Save());

        // Now the document exist
        Assert.IsTrue(UserDocument.Exist());
        Assert.IsTrue(UserDocument.Find());
        Assert.IsFalse(UserDocument.Find().Data == null);

        // But only the las save is stored in memory, value finded will be always the last save
        Assert.IsTrue(UserDocument.Find().Data.count == 33);

        // You can save again
        Assert.IsTrue(user4.Save());

        // And data finded will be this new save
        Assert.IsTrue(UserDocument.Find().Data.count == 66);

    }

    [Test]
    public void A300_FindOrDefault()
    {

        // You can create multiple instances of the class
        var user1 = new UserDocument(); user1.count = 33;

        var defaultDoc = new UserDocument { count = 11, };

        // If you dont save, the document dont exist, dut findOrDefault will return the default document and a succesful operation
        Assert.IsFalse(UserDocument.Exist());
        Assert.IsTrue(UserDocument.FindOrDefault(defaultDoc));
        Assert.AreEqual(11, UserDocument.FindOrDefault(defaultDoc).Data.count);

        // When you save
        Debug.Log("Saving");
        Assert.IsTrue(user1.Save());

        // Now the document exist and will be returned
        Assert.IsTrue(UserDocument.Exist());
        Assert.IsTrue(UserDocument.FindOrDefault(defaultDoc));
        Assert.AreEqual(33, UserDocument.FindOrDefault(defaultDoc).Data.count);

        // You can save delete
        Assert.IsTrue(UserDocument.Delete());

        // Now the document dont exist, dut findOrDefault will return the default document and a succesful operation
        Assert.IsFalse(UserDocument.Exist());
        Assert.IsTrue(UserDocument.FindOrDefault(defaultDoc));
        Assert.AreEqual(11, UserDocument.FindOrDefault(defaultDoc).Data.count);

    }




    [Test]
    public void A400_Update_01WillFindAndUpdateAnDocumentIfExist()
    {

        // Create one document
        Debug.Log("Save");
        var newd1 = new UserDocument { count = 66 };

        // The first one will be saved
        Debug.Log("Save");
        Assert.IsTrue(newd1.Save());

        // Only one document will be stored
        Debug.Log("Save");
        Assert.IsTrue(UserDocument.Exist());


        // Find the document by the id
        Debug.Log("Save");
        var opp = UserDocument.Find();
        Assert.IsTrue(opp);
        Assert.IsTrue(opp.Data.count == 66);

        // Update the document
        Debug.Log("Update");
        var upp = UserDocument.Update(d => { d.count = 11; return d; });
        Assert.IsTrue(upp);

        // Find the document by the id
        Debug.Log("FindAgain");
        opp = UserDocument.Find();
        Assert.IsTrue(opp);
        Assert.IsTrue(opp.Data.count == 11);




        // Try to Update an unexistent document will return false
        UserDocument.Delete();
        Debug.Log("UpdateUnexistent");
        var opp2 = UserDocument.Update(d => { d.count = 11; return d; });
        Assert.IsFalse(opp2);

        // Document is not stored
        Assert.IsFalse(UserDocument.Exist());

    }

    [Test]
    public void A400_Update_IfUpdatingThrowsError_DontUpdate()
    {

        // Create one document
        Debug.Log("Save");
        var newd1 = new UserDocument { count = 66};

        // The first one will be saved
        Debug.Log("Save");
        Assert.IsTrue(newd1.Save());

        Debug.Log("Save");
        Assert.IsTrue(UserDocument.Exist());


        // Find the document by the id
        Debug.Log("Save");
        var opp = UserDocument.Find();
        Assert.IsTrue(opp);
        Assert.IsTrue(opp.Data.count == 66);

        // Update the document
        Debug.Log("Update");
        var upp = UserDocument.Update(d => { d.count = 11; throw new Exception("Expected exception"); });
        Assert.IsFalse(upp);

        // Find the same document by the id
        Debug.Log("Save");
        opp = UserDocument.Find();
        Assert.IsTrue(opp);
        Assert.IsTrue(opp.Data.count == 66);


    }

    [Test]
    public void A400_Update_61InvalidOrNullKeyOrUpdateFunc_WillThrowError()
    {

        // Null update func
        Assert.Throws<ArgumentNullException>(() => UserDocument.Update(null));
    }





    [Test]
    public void A200_Delete()
    {

        // You can create multiple instances of the class
        var user1 = new UserDocument(); user1.count = 33;
        var user2 = new UserDocument(); user2.count = 44;
        var user3 = new UserDocument(); user3.count = 55;
        var user4 = new UserDocument(); user4.count = 66;
        var user5 = new UserDocument(); user5.count = 77;

        // If you dont save, the document dont exist
        Assert.IsFalse(UserDocument.Exist());
        Assert.IsFalse(UserDocument.Find());
        Assert.IsTrue(UserDocument.Find().Data == null);

        // If you delete the document and dont exist, will return true
        Assert.IsTrue(UserDocument.Delete());

        // And you can save a document
        Debug.Log("Saving");
        Assert.IsTrue(user5.Save());

        // Now the document exist
        Assert.IsTrue(UserDocument.Exist());
        Assert.IsTrue(UserDocument.Find());
        Assert.IsFalse(UserDocument.Find().Data == null);
        Assert.IsTrue(UserDocument.Find().Data.count == 77);

        // If you delete the document
        Assert.IsTrue(UserDocument.Delete());

        // The document will be deleted
        Assert.IsFalse(UserDocument.Exist());
        Assert.IsFalse(UserDocument.Find());
        Assert.IsTrue(UserDocument.Find().Data == null);

        // If you delete the document and dont exist, will return true
        Assert.IsTrue(UserDocument.Delete());


        // And you can save a document again or many ogf them
        Debug.Log("Saving");
        Assert.IsTrue(user3.Save());
        Assert.IsTrue(user4.Save());

        // Now the document exist and is the last saved
        Assert.IsTrue(UserDocument.Exist());
        Assert.IsTrue(UserDocument.Find());
        Assert.IsFalse(UserDocument.Find().Data == null);
        Assert.IsTrue(UserDocument.Find().Data.count == 66);

        // If you delete the document
        Assert.IsTrue(UserDocument.Delete());

        // The document will be deleted
        Assert.IsFalse(UserDocument.Exist());
        Assert.IsFalse(UserDocument.Find());
        Assert.IsTrue(UserDocument.Find().Data == null);

    }



}
