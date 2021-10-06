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

public class DalilaDocument_A1Encryption
{

    #region Example classes

    [KnownType(typeof(UserEncryptedDocument))]
    [DataContract()]
    class UserEncryptedDocument : DalilaDBDocument<UserEncryptedDocument>
    {

        [DataMember()]
        public int count;

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
        try { Directory.Delete(UserEncryptedDocument.LocationPath, true); } catch (Exception) { }
        UserEncryptedDocument.Delete();
        UserEncryptedDocument.ResetFileSystem();

    }




    [Test]
    public void A000_Save()
    {

        // You can create multiple instances of the class
        var user1 = new UserEncryptedDocument(); user1.count = 33;
        var user2 = new UserEncryptedDocument(); user2.count = 44;
        var user3 = new UserEncryptedDocument(); user3.count = 55;
        var user4 = new UserEncryptedDocument(); user4.count = 66;
        var user5 = new UserEncryptedDocument(); user5.count = 77;

        // And you can save all of them
        Debug.Log("Saving");
        Assert.IsTrue(user5.Save());
        Assert.IsTrue(user4.Save());
        Assert.IsTrue(user3.Save());
        Assert.IsTrue(user2.Save());
        Assert.IsTrue(user1.Save());

        // But only the las save is stored in memory, value finded will be always the last save
        Assert.IsTrue(UserEncryptedDocument.Find().Data.count == 33);

        // You can save again
        Assert.IsTrue(user4.Save());

        // And data finded will be this new save
        Assert.IsTrue(UserEncryptedDocument.Find().Data.count == 66);

    }

    [Test]
    public void A100_Find()
    {

        // You can create multiple instances of the class
        var user1 = new UserEncryptedDocument(); user1.count = 33;
        var user2 = new UserEncryptedDocument(); user2.count = 44;
        var user3 = new UserEncryptedDocument(); user3.count = 55;
        var user4 = new UserEncryptedDocument(); user4.count = 66;
        var user5 = new UserEncryptedDocument(); user5.count = 77;

        // If you dont save, the document dont exist
        Assert.IsFalse(UserEncryptedDocument.Exist());
        Assert.IsFalse(UserEncryptedDocument.Find());
        Assert.IsTrue(UserEncryptedDocument.Find().Data == null);

        // When you save
        Debug.Log("Saving");
        Assert.IsTrue(user5.Save());
        Assert.IsTrue(user4.Save());
        Assert.IsTrue(user3.Save());
        Assert.IsTrue(user2.Save());
        Assert.IsTrue(user1.Save());

        // Now the document exist
        Assert.IsTrue(UserEncryptedDocument.Exist());
        Assert.IsTrue(UserEncryptedDocument.Find());
        Assert.IsFalse(UserEncryptedDocument.Find().Data == null);

        // But only the las save is stored in memory, value finded will be always the last save
        Assert.IsTrue(UserEncryptedDocument.Find().Data.count == 33);

        // You can save again
        Assert.IsTrue(user4.Save());

        // And data finded will be this new save
        Assert.IsTrue(UserEncryptedDocument.Find().Data.count == 66);

    }

    [Test]
    public void A300_FindOrDefault()
    {

        // You can create multiple instances of the class
        var user1 = new UserEncryptedDocument(); user1.count = 33;

        var defaultDoc = new UserEncryptedDocument { count = 11, };

        // If you dont save, the document dont exist, dut findOrDefault will return the default document and a succesful operation
        Assert.IsFalse(UserEncryptedDocument.Exist());
        Assert.IsTrue(UserEncryptedDocument.FindOrDefault(defaultDoc));
        Assert.AreEqual(11, UserEncryptedDocument.FindOrDefault(defaultDoc).Data.count);

        // When you save
        Debug.Log("Saving");
        Assert.IsTrue(user1.Save());

        // Now the document exist and will be returned
        Assert.IsTrue(UserEncryptedDocument.Exist());
        Assert.IsTrue(UserEncryptedDocument.FindOrDefault(defaultDoc));
        Assert.AreEqual(33, UserEncryptedDocument.FindOrDefault(defaultDoc).Data.count);

        // You can save delete
        Assert.IsTrue(UserEncryptedDocument.Delete());

        // Now the document dont exist, dut findOrDefault will return the default document and a succesful operation
        Assert.IsFalse(UserEncryptedDocument.Exist());
        Assert.IsTrue(UserEncryptedDocument.FindOrDefault(defaultDoc));
        Assert.AreEqual(11, UserEncryptedDocument.FindOrDefault(defaultDoc).Data.count);

    }




    [Test]
    public void A400_Update_01WillFindAndUpdateAnDocumentIfExist()
    {

        // Create one document
        Debug.Log("Save");
        var newd1 = new UserEncryptedDocument { count = 66 };

        // The first one will be saved
        Debug.Log("Save");
        Assert.IsTrue(newd1.Save());

        // Only one document will be stored
        Debug.Log("Save");
        Assert.IsTrue(UserEncryptedDocument.Exist());


        // Find the document by the id
        Debug.Log("Save");
        var opp = UserEncryptedDocument.Find();
        Assert.IsTrue(opp);
        Assert.IsTrue(opp.Data.count == 66);

        // Update the document
        Debug.Log("Update");
        var upp = UserEncryptedDocument.Update(d => { d.count = 11; return d; });
        Assert.IsTrue(upp);

        // Find the document by the id
        Debug.Log("FindAgain");
        opp = UserEncryptedDocument.Find();
        Assert.IsTrue(opp);
        Assert.IsTrue(opp.Data.count == 11);




        // Try to Update an unexistent document will return false
        UserEncryptedDocument.Delete();
        Debug.Log("UpdateUnexistent");
        var opp2 = UserEncryptedDocument.Update(d => { d.count = 11; return d; });
        Assert.IsFalse(opp2);

        // Document is not stored
        Assert.IsFalse(UserEncryptedDocument.Exist());

    }

    [Test]
    public void A400_Update_IfUpdatingThrowsError_DontUpdate()
    {

        // Create one document
        Debug.Log("Save");
        var newd1 = new UserEncryptedDocument { count = 66};

        // The first one will be saved
        Debug.Log("Save");
        Assert.IsTrue(newd1.Save());

        Debug.Log("Save");
        Assert.IsTrue(UserEncryptedDocument.Exist());


        // Find the document by the id
        Debug.Log("Save");
        var opp = UserEncryptedDocument.Find();
        Assert.IsTrue(opp);
        Assert.IsTrue(opp.Data.count == 66);

        // Update the document
        Debug.Log("Update");
        var upp = UserEncryptedDocument.Update(d => { d.count = 11; throw new Exception("Expected exception"); });
        Assert.IsFalse(upp);

        // Find the same document by the id
        Debug.Log("Save");
        opp = UserEncryptedDocument.Find();
        Assert.IsTrue(opp);
        Assert.IsTrue(opp.Data.count == 66);


    }

    [Test]
    public void A400_Update_61InvalidOrNullKeyOrUpdateFunc_WillThrowError()
    {

        // Null update func
        Assert.Throws<ArgumentNullException>(() => UserEncryptedDocument.Update(null));
    }





    [Test]
    public void A200_Delete()
    {

        // You can create multiple instances of the class
        var user1 = new UserEncryptedDocument(); user1.count = 33;
        var user2 = new UserEncryptedDocument(); user2.count = 44;
        var user3 = new UserEncryptedDocument(); user3.count = 55;
        var user4 = new UserEncryptedDocument(); user4.count = 66;
        var user5 = new UserEncryptedDocument(); user5.count = 77;

        // If you dont save, the document dont exist
        Assert.IsFalse(UserEncryptedDocument.Exist());
        Assert.IsFalse(UserEncryptedDocument.Find());
        Assert.IsTrue(UserEncryptedDocument.Find().Data == null);

        // If you delete the document and dont exist, will return true
        Assert.IsTrue(UserEncryptedDocument.Delete());

        // And you can save a document
        Debug.Log("Saving");
        Assert.IsTrue(user5.Save());

        // Now the document exist
        Assert.IsTrue(UserEncryptedDocument.Exist());
        Assert.IsTrue(UserEncryptedDocument.Find());
        Assert.IsFalse(UserEncryptedDocument.Find().Data == null);
        Assert.IsTrue(UserEncryptedDocument.Find().Data.count == 77);

        // If you delete the document
        Assert.IsTrue(UserEncryptedDocument.Delete());

        // The document will be deleted
        Assert.IsFalse(UserEncryptedDocument.Exist());
        Assert.IsFalse(UserEncryptedDocument.Find());
        Assert.IsTrue(UserEncryptedDocument.Find().Data == null);

        // If you delete the document and dont exist, will return true
        Assert.IsTrue(UserEncryptedDocument.Delete());


        // And you can save a document again or many ogf them
        Debug.Log("Saving");
        Assert.IsTrue(user3.Save());
        Assert.IsTrue(user4.Save());

        // Now the document exist and is the last saved
        Assert.IsTrue(UserEncryptedDocument.Exist());
        Assert.IsTrue(UserEncryptedDocument.Find());
        Assert.IsFalse(UserEncryptedDocument.Find().Data == null);
        Assert.IsTrue(UserEncryptedDocument.Find().Data.count == 66);

        // If you delete the document
        Assert.IsTrue(UserEncryptedDocument.Delete());

        // The document will be deleted
        Assert.IsFalse(UserEncryptedDocument.Exist());
        Assert.IsFalse(UserEncryptedDocument.Find());
        Assert.IsTrue(UserEncryptedDocument.Find().Data == null);

    }



}
