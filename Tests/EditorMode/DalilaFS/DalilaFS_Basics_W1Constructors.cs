using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using U.DalilaDB;
using System.IO;


namespace DalilaFsTests
{
    public class DalilaFS_Basics_W1Constructors
    {

        [Test]
        public void A_Constructor_WhenApplicationPersistentDataPathPassed_MustInit()
        {
            // Pass a slash or not is the same
            var fileSystem1 = new DalilaFS(Application.persistentDataPath + "/");
            var fileSystem2 = new DalilaFS(Application.persistentDataPath);

            Debug.Log(fileSystem1._root);
            Debug.Log(fileSystem2._root);

            Assert.AreEqual(fileSystem1._root, fileSystem2._root);
            Assert.AreEqual(Application.persistentDataPath + "/", fileSystem2._root);

        }


        [Test]
        public void A_Constructor_WhenNoPathPassed_WillUseApplicationPersistentDatapath()
        {
            // Pass a slash or not is the same
            var fileSystem1 = new DalilaFS();

            Debug.Log(fileSystem1._root);

            Assert.AreEqual(Application.persistentDataPath + "/", fileSystem1._root);

        }


        [Test]
        public void B_Constructor_WhenOtherValidPathPassedThatExist_MustInit()
        {
            var path = Application.persistentDataPath + "/23e32d23d2332d";
            Directory.CreateDirectory(path);

            var fileSystem = new DalilaFS(path);
            Debug.Log(fileSystem._root);

            Assert.AreEqual(Application.persistentDataPath + "/23e32d23d2332d/", fileSystem._root);
        }


        [Test]
        public void C_Constructor_UnexistentRootPasses_WillBeCreatedIfPosible()
        {

            // Path must dont exists
            var unexistentPath = Application.persistentDataPath + "/23e32d23d2332d";
            if (Directory.Exists(unexistentPath))
                Directory.Delete(unexistentPath, true);

            // Try to create the fileSystem
            Assert.DoesNotThrow(() => new DalilaFS(unexistentPath));

        }


        [Test]
        public void D_Constructor_InvalidRootPasses_MustThrowException()
        {

            // Path must dont exists
            var unexistentPath = Application.persistentDataPath + "/23e32d23??()d2332d";
            if (Directory.Exists(unexistentPath))
                Directory.Delete(unexistentPath, true);

            // Try to create the fileSystem
            Assert.Throws<IOException>(() => new DalilaFS(unexistentPath));

        }
    }

}