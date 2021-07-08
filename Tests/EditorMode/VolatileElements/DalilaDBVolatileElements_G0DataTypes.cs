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

public class DalilaVolatileElements_G0DataTypes
{

    #region Example classes

    class SimpleElements : DalilaDBVolatileElements<SimpleElements>
    {

    }

    #endregion Example classes


    [SetUp]
    public void SetUp()
    {
        // Delete the directory to clear it
        SimpleElements.DeleteAll();

    }




    [Test]
    public void DalilaVolatileElements_A001CanSave_Int()
    {
        // Save a element
        int originalElement = 32;
        var saveOperation = SimpleElements.Save("File", originalElement);

        // Check the result
        Debug.Log("Save Operation: " + saveOperation);
        Assert.IsTrue(saveOperation);


        // Read the saved element
        var readOperation = SimpleElements.Find<int>("File");

        // Check the result
        Debug.Log("Read Operation: " + readOperation);
        Assert.IsTrue(readOperation);
        int readedElement = readOperation.Data;


        // Compare the two values
        Debug.Log("Original: " + originalElement + " Readed: " + readedElement);
        Assert.AreEqual(originalElement, readedElement);

    }


    [Test]
    public void DalilaVolatileElements_A002CanSave_Bool()
    {
        // Save a element
        bool originalElement = true;
        var saveOperation = SimpleElements.Save("File", originalElement);

        // Check the result
        Debug.Log("Save Operation: " + saveOperation);
        Assert.IsTrue(saveOperation);


        // Read the saved element
        var readOperation = SimpleElements.Find<bool>("File");

        // Check the result
        Debug.Log("Read Operation: " + readOperation);
        Assert.IsTrue(readOperation);
        bool readedElement = readOperation.Data;


        // Compare the two values
        Debug.Log("Original: " + originalElement + " Readed: " + readedElement);
        Assert.AreEqual(originalElement, readedElement);

    }



    [Test]
    public void DalilaVolatileElements_A003CanSave_Vector2()
    {
        // Save a element
        Vector2 originalElement = new Vector2(1.1f,2.2f);
        var saveOperation = SimpleElements.Save("File", originalElement);

        // Check the result
        Debug.Log("Save Operation: " + saveOperation);
        Assert.IsTrue(saveOperation);


        // Read the saved element
        var readOperation = SimpleElements.Find<Vector2>("File");

        // Check the result
        Debug.Log("Read Operation: " + readOperation);
        Assert.IsTrue(readOperation);
        Vector2 readedElement = readOperation.Data;


        // Compare the two values
        Debug.Log("Original: " + originalElement + " Readed: " + readedElement);
        Assert.AreEqual(originalElement, readedElement);

    }

    [Test]
    public void DalilaVolatileElements_A004CanSave_Vector3()
    {
        // Save a element
        Vector3 originalElement = new Vector3(1.1f,2.2f,3.3f);
        var saveOperation = SimpleElements.Save("File", originalElement);

        // Check the result
        Debug.Log("Save Operation: " + saveOperation);
        Assert.IsTrue(saveOperation);


        // Read the saved element
        var readOperation = SimpleElements.Find<Vector3>("File");

        // Check the result
        Debug.Log("Read Operation: " + readOperation);
        Assert.IsTrue(readOperation);
        var readedElement = readOperation.Data;


        // Compare the two values
        Debug.Log("Original: " + originalElement + " Readed: " + readedElement);
        Assert.AreEqual(originalElement, readedElement);

    }

    [Test]
    public void DalilaVolatileElements_A005CanSave_Quaternion()
    {
        // Save a element
        Quaternion originalElement = new Quaternion(2,3,4,5);
        var saveOperation = SimpleElements.Save("File", originalElement);

        // Check the result
        Debug.Log("Save Operation: " + saveOperation);
        Assert.IsTrue(saveOperation);


        // Read the saved element
        var readOperation = SimpleElements.Find<Quaternion>("File");

        // Check the result
        Debug.Log("Read Operation: " + readOperation);
        Assert.IsTrue(readOperation);
        var readedElement = readOperation.Data;


        // Compare the two values
        Debug.Log("Original: " + originalElement + " Readed: " + readedElement);
        Assert.AreEqual(originalElement, readedElement);

    }

    [Test]
    public void DalilaVolatileElements_A005CanSave_Color()
    {
        // Save a element
        Color originalElement = Color.red;
        var saveOperation = SimpleElements.Save("File", originalElement);

        // Check the result
        Debug.Log("Save Operation: " + saveOperation);
        Assert.IsTrue(saveOperation);


        // Read the saved element
        var readOperation = SimpleElements.Find<Color>("File");

        // Check the result
        Debug.Log("Read Operation: " + readOperation);
        Assert.IsTrue(readOperation);
        var readedElement = readOperation.Data;


        // Compare the two values
        Debug.Log("Original: " + originalElement + " Readed: " + readedElement);
        Assert.AreEqual(originalElement, readedElement);

    }

    [Test]
    public void DalilaVolatileElements_A006CanSave_String()
    {
        // Save a element
        string originalElement = "Saved String";
        var saveOperation = SimpleElements.Save("File", originalElement);

        // Check the result
        Debug.Log("Save Operation: " + saveOperation);
        Assert.IsTrue(saveOperation);


        // Read the saved element
        var readOperation = SimpleElements.Find<string>("File");

        // Check the result
        Debug.Log("Read Operation: " + readOperation);
        Assert.IsTrue(readOperation);
        string readedElement = readOperation.Data;


        // Compare the two values
        Debug.Log("Original: " + originalElement + " Readed: " + readedElement);
        Assert.AreEqual(originalElement, readedElement);

    }

    // Custom Serializable class
    [Serializable]
    class CustomSerializableClass
    {
        public int lives;
        public bool sool;
        public Vector3 position;
    }

    [Test]
    public void DalilaVolatileElements_B001CanSave_CustomSerializableClassSaveAllParameters()
    {
        // Save a element
        CustomSerializableClass originalElement = new CustomSerializableClass { lives = 2, position = new Vector3(1, 2, 3), sool = true};
        var saveOperation = SimpleElements.Save("File", originalElement);

        // Check the result
        Debug.Log("Save Operation: " + saveOperation);
        Assert.IsTrue(saveOperation);


        // Read the saved element
        var readOperation = SimpleElements.Find<CustomSerializableClass>("File");

        // Check the result
        Debug.Log("Read Operation: " + readOperation);
        Assert.IsTrue(readOperation);
        CustomSerializableClass readedElement = readOperation.Data;


        // Compare the two values
        Debug.Log("Original: " + originalElement + " Readed: " + readedElement);
        Assert.AreEqual(originalElement.lives, readedElement.lives);
        Assert.AreEqual(originalElement.position, readedElement.position);
        Assert.AreEqual(originalElement.sool, readedElement.sool);

    }

    // Custom Serializable class
    [DataContract()]
    class CustomDataContractClass
    {
        public int lives = 10;
        public bool sool = false;
        [DataMember()]
        public Vector3 position;
    }

    [Test]
    public void DalilaVolatileElements_B001CanSave_CustomDataContractClassSaveSomeParameters()
    {
        // Save a element
        CustomDataContractClass originalElement = new CustomDataContractClass { lives = 2, position = new Vector3(1, 2, 3), sool = true };
        var saveOperation = SimpleElements.Save("File", originalElement);

        // Check the result
        Debug.Log("Save Operation: " + saveOperation);
        Assert.IsTrue(saveOperation);


        // Read the saved element
        var readOperation = SimpleElements.Find<CustomDataContractClass>("File");

        // Check the result
        Debug.Log("Read Operation: " + readOperation);
        Assert.IsTrue(readOperation);
        CustomDataContractClass readedElement = readOperation.Data;


        // Compare the two values
        Debug.Log("Original: " + originalElement + " Readed: " + readedElement);
        Assert.AreNotEqual(originalElement.lives, readedElement.lives);
        Assert.AreNotEqual(originalElement.sool, readedElement.sool);
        Assert.AreEqual(originalElement.position, readedElement.position);

    }


    // Custom Serializable class
    [Serializable]
    struct CustomSerializableStruct
    {
        public int lives;
        public bool sool;
        public Vector3 position;

    }

    [Test]
    public void DalilaVolatileElements_B002CanSave_CustomSerializableStruct()
    {
        // Save a element
        CustomSerializableStruct originalElement = new CustomSerializableStruct { lives = 2, position = new Vector3(1, 2, 3), sool = true };
        var saveOperation = SimpleElements.Save("File", originalElement);

        // Check the result
        Debug.Log("Save Operation: " + saveOperation);
        Assert.IsTrue(saveOperation);


        // Read the saved element
        var readOperation = SimpleElements.Find<CustomSerializableStruct>("File");

        // Check the result
        Debug.Log("Read Operation: " + readOperation);
        Assert.IsTrue(readOperation);
        CustomSerializableStruct readedElement = readOperation.Data;


        // Compare the two values
        Debug.Log("Original: " + originalElement + " Readed: " + readedElement);
        Assert.AreEqual(originalElement.lives, readedElement.lives);
        Assert.AreEqual(originalElement.position, readedElement.position);
        Assert.AreEqual(originalElement.sool, readedElement.sool);

    }

    
    [Test]
    public void DalilaVolatileElements_B001CanSave_TransformData()
    {
        // Create the unsavable element
        var gObject = new GameObject();
        gObject.transform.position = new Vector3(1, 2, 3);
        gObject.transform.rotation = Quaternion.Euler(13, 44, 54);
        gObject.transform.localScale = new Vector3(2, 2, 2);

        // Save a element
        U.DalilaDB.TransformData originalElement = U.DalilaDB.TransformData.Store(gObject);
        var saveOperation = SimpleElements.Save("File", originalElement);

        // Check the result
        Debug.Log("Save Operation: " + saveOperation);
        Assert.IsTrue(saveOperation);


        // Read the saved element
        var readOperation = SimpleElements.Find<TransformData>("File");

        // Check the result
        Debug.Log("Read Operation: " + readOperation);
        Assert.IsTrue(readOperation);
        TransformData readedTransformData = readOperation.Data;


        // Create a clone of the unsabable element
        var cObject = new GameObject();
        readedTransformData.Set(cObject);

        // Compare the two values
        Debug.Log("Original: " + originalElement + " Readed: " + readedTransformData);
        Assert.AreEqual(gObject.transform.position, cObject.transform.position);
        Assert.AreEqual(gObject.transform.rotation, cObject.transform.rotation);
        Assert.AreEqual(gObject.transform.localScale, cObject.transform.localScale);

    }


    [Test]
    public void DalilaVolatileElements_B001CanSave_GameObjectData()
    {
        // Create the unsavable element
        var gObject = new GameObject();
        gObject.name = "testing";
        gObject.transform.position = new Vector3(1, 2, 3);
        gObject.transform.rotation = Quaternion.Euler(13,44,54);
        gObject.transform.localScale = new Vector3(2, 2, 2);

        // Save a element
        GameObjectData originalElement = GameObjectData.Store(gObject);
        var saveOperation = SimpleElements.Save("File", originalElement);

        // Check the result
        Debug.Log("Save Operation: " + saveOperation);
        Assert.IsTrue(saveOperation);


        // Read the saved element
        var readOperation = SimpleElements.Find<GameObjectData>("File");

        // Check the result
        Debug.Log("Read Operation: " + readOperation);
        Assert.IsTrue(readOperation);
        GameObjectData readedGameObjectData = readOperation.Data;


        // Create a clone of the unsabable element
        var cObject = readedGameObjectData.Create();

        // Compare the two values
        Debug.Log("Original: " + originalElement + " Readed: " + readedGameObjectData);
        Assert.AreEqual(gObject.name, cObject.name);
        Assert.AreEqual(gObject.transform.position, cObject.transform.position);
        Assert.AreEqual(gObject.transform.rotation, cObject.transform.rotation);
        Assert.AreEqual(gObject.transform.localScale, cObject.transform.localScale);


        // Create a clone of the unsabable element setting values
        var dObject = new GameObject();
        readedGameObjectData.Set(dObject);

        // Compare the two values
        Debug.Log("Original: " + originalElement + " Readed: " + readedGameObjectData);
        Assert.AreEqual(gObject.name, dObject.name);
        Assert.AreEqual(gObject.transform.position, dObject.transform.position);
        Assert.AreEqual(gObject.transform.rotation, dObject.transform.rotation);
        Assert.AreEqual(gObject.transform.localScale, dObject.transform.localScale);


    }



    class AMonoBehaviour : MonoBehaviour
    {
        public int live;
        public bool isJumping = false;
        public float time;
    }

    [Serializable]
    class AMonoBehaviourData
    {
        // Select only properties that should be saved
        public int live;
        // This is not selected
        //public bool isJumping;
        public float time;

        // Private Constructor
        private AMonoBehaviourData() { }

        // Copy the data to save from the monobehaviour
        public static AMonoBehaviourData Store(AMonoBehaviour t)
        {
            var d = new AMonoBehaviourData();

            d.live = t.live;
            d.time = t.time;


            return d;
        }

        // Get the component and copy the data
        public static AMonoBehaviourData Store(GameObject g)
        {
            return Store(g.GetComponent<AMonoBehaviour>());
        }

        // Set the data to a instance of the monobehaviour
        public AMonoBehaviour Set(AMonoBehaviour t)
        {
            t.live = live;
            t.time = time; 

            return t;
        }

        // Add the component and set the data
        public AMonoBehaviour Set(GameObject g)
        {
            var c = g.AddComponent<AMonoBehaviour>();
            Set(c);
            return c;
        }
    }

    [Test]
    public void DalilaVolatileElements_B001CanSave_MonoBehaviours()
    {
        // Create the unsavable element
        var gObject = new GameObject();
        var c = gObject.AddComponent<AMonoBehaviour>();
        c.live = 23;
        c.time = 12.2f;
        c.isJumping = true;

        // Save a element
        AMonoBehaviourData originalElement = AMonoBehaviourData.Store(gObject);
        var saveOperation = SimpleElements.Save("File", originalElement);

        // Check the result
        Debug.Log("Save Operation: " + saveOperation);
        Assert.IsTrue(saveOperation);


        // Read the saved element
        var readOperation = SimpleElements.Find<AMonoBehaviourData>("File");

        // Check the result
        Debug.Log("Read Operation: " + readOperation);
        Assert.IsTrue(readOperation);
        AMonoBehaviourData readedMonobehaviourData = readOperation.Data;


        // Create a clone of the unsabable element
        var cObject = new GameObject();
        readedMonobehaviourData.Set(cObject);

        // Compare the two values
        Debug.Log("Original: " + originalElement + " Readed: " + readedMonobehaviourData);
        Assert.AreEqual(gObject.GetComponent<AMonoBehaviour>().live, cObject.GetComponent<AMonoBehaviour>().live);
        Assert.AreEqual(gObject.GetComponent<AMonoBehaviour>().time, cObject.GetComponent<AMonoBehaviour>().time);

        // Vars not saved
        Assert.IsFalse(cObject.GetComponent<AMonoBehaviour>().isJumping);



        // Set values to existing component
        var dObject = new GameObject().AddComponent<AMonoBehaviour>();
        readedMonobehaviourData.Set(dObject.GetComponent<AMonoBehaviour>());

        // Compare the two values
        Debug.Log("Original: " + originalElement + " Readed: " + readedMonobehaviourData);
        Assert.AreEqual(gObject.GetComponent<AMonoBehaviour>().live, dObject.GetComponent<AMonoBehaviour>().live);
        Assert.AreEqual(gObject.GetComponent<AMonoBehaviour>().time, dObject.GetComponent<AMonoBehaviour>().time);

        // Vars not saved
        Assert.IsFalse(dObject.GetComponent<AMonoBehaviour>().isJumping);


    }



    // Custom Serializable class
    class CustomNoSerializableClass
    {
        public int lives;
        public bool sool;
        public Vector3 position;
    }

    [Test]
    public void DalilaVolatileElements_H001CantSave_NoSerializablesOrDataContractClasses()
    {
        // Save a element
        CustomNoSerializableClass originalElement = new CustomNoSerializableClass { lives = 2, position = new Vector3(1, 2, 3), sool = true };
        var saveOperation = SimpleElements.Save("File", originalElement);

        // Check the result
        Debug.Log("Save Operation: " + saveOperation);
        Assert.IsFalse(saveOperation);
        Assert.Throws<InvalidDataContractException>(() => throw saveOperation.Error);


        // Read the saved element
        var readOperation = SimpleElements.Find<CustomNoSerializableClass>("File");

        // Check the result
        Debug.Log("Read Operation: " + readOperation);
        Assert.IsFalse(readOperation);
        Assert.Throws<FileNotFoundException>(() => throw readOperation.Error);

    }

    // Custom Serializable class
    struct CustomNoSerializableStruct
    {
        public int lives;
        public bool sool;
        public Vector3 position;
    }

    [Test]
    public void DalilaVolatileElements_H002CantSave_NoSerializablesStructs()
    {
        // Save a element
        CustomNoSerializableStruct originalElement = new CustomNoSerializableStruct { lives = 2, position = new Vector3(1, 2, 3), sool = true };
        var saveOperation = SimpleElements.Save("File", originalElement);

        // Check the result
        Debug.Log("Save Operation: " + saveOperation);
        Assert.IsFalse(saveOperation);
        Assert.Throws<InvalidDataContractException>(() => throw saveOperation.Error);


        // Read the saved element
        var readOperation = SimpleElements.Find<CustomNoSerializableStruct>("File");

        // Check the result
        Debug.Log("Read Operation: " + readOperation);
        Assert.IsFalse(readOperation);
        Assert.Throws<FileNotFoundException>(() => throw readOperation.Error);

    }


    [Test]
    public void DalilaVolatileElements_H003CantSave_RawGameObjects()
    {
        // Save a element
        GameObject originalElement = new GameObject("Only");
        var saveOperation = SimpleElements.Save("File", originalElement);

        // Check the result
        Debug.Log("Save Operation: " + saveOperation);
        Assert.IsFalse(saveOperation);
        Assert.Throws<System.Reflection.TargetInvocationException>(() => throw saveOperation.Error);


        // Read the saved element
        var readOperation = SimpleElements.Find<GameObject>("File");

        // Check the result
        Debug.Log("Read Operation: " + readOperation);
        Assert.IsFalse(readOperation);
        Assert.Throws<FileNotFoundException>(() => throw readOperation.Error);

    }

    [Test]
    public void DalilaVolatileElements_H004CantSave_RawTransforms()
    {
        // Save a element
        Transform originalElement = new GameObject("Only").transform;
        var saveOperation = SimpleElements.Save("File", originalElement);

        // Check the result
        Debug.Log("Save Operation: " + saveOperation);
        Assert.IsFalse(saveOperation);
        Assert.Throws<System.Runtime.Serialization.InvalidDataContractException>(() => throw saveOperation.Error);


        // Read the saved element
        var readOperation = SimpleElements.Find<Transform>("File");

        // Check the result
        Debug.Log("Read Operation: " + readOperation);
        Assert.IsFalse(readOperation);
        Assert.Throws<FileNotFoundException>(() => throw readOperation.Error);

    }

    class CustomMonoBehaviour : MonoBehaviour
    {
        public int lives;
        public bool sool;
        public Vector3 position;
    }
    [Test]
    public void DalilaVolatileElements_H004CantSave_RawMonoBehaviours()
    {
        // Save a element
        CustomMonoBehaviour originalElement = new GameObject("Only").AddComponent<CustomMonoBehaviour>();
        var saveOperation = SimpleElements.Save("File", originalElement);

        // Check the result
        Debug.Log("Save Operation: " + saveOperation);
        Assert.IsFalse(saveOperation);
        Assert.Throws<System.Runtime.Serialization.InvalidDataContractException>(() => throw saveOperation.Error);


        // Read the saved element
        var readOperation = SimpleElements.Find<CustomMonoBehaviour>("File");

        // Check the result
        Debug.Log("Read Operation: " + readOperation);
        Assert.IsFalse(readOperation);
        Assert.Throws<FileNotFoundException>(() => throw readOperation.Error);

    }


}
