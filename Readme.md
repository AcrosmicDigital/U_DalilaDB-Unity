<img src="https://images4public4ccess.s3.amazonaws.com/DalilaDB/DalilaDB-Logob.png" width="100">

# DalilaDB(Beta) for Unity

A document based NoSql database for use in local applications that can store data in disk or in memory.

- Compatible with PC, Mac, Linux, Windows Universal, iOS, tvOS, Android, WebGL
- Serialize to .xml documentd using DataContractSerializer 
- Save any serializable type
- Sync and Async functions to store in background thread and dont frezee app
- Relations between collections(Tables)
- Easy query system to Save, Find, Update and Delete
- You can save data to hard disk or to RAM memory
- Serialize public and private properties
- Three kinds of storage models
- Optional AES128 or AES256 encryption
- Optional SHA256 to hash passwords or text

## Instalation

1. Create or open a Unity proyect
2. Download and unzip the repository
3. Inside Unity proyect in Assets Folder, create a new Folder named DalilaDB and copy
4. From unziped folder copy only
    - "U.DalilaDB.asmdef" file
    - "LICENSE" file
    - "/Editor" folder
    - "/Runtime" folder

![DalilaDBInstallation1](https://images4public4ccess.s3.amazonaws.com/DalilaDB/DalilaDBInstallation1b.JPG)

5. If ok if this log appear in Unity console "Assembly for Assembly Definition File 'Assets/DalilaDB/U.DalilaDB.asmdef' will not be compiled, because it has no scripts associated with it."

![DalilaDBInstallation2](https://images4public4ccess.s3.amazonaws.com/DalilaDB/DalilaDBInstallation2b.JPG)

6. Wait while Unity compiles the files 
7. Delete all logs in console
8. When finish "U" button must appear in Menu Bar

![DalilaDBInstallation3](https://images4public4ccess.s3.amazonaws.com/DalilaDB/DalilaDBInstallation3b.JPG)

14. If something went wrong, please create a new issue with logs and Unity version

## Store persistent data

### Elements

DalilaDB elements are the simpliest way to store data, you can store any serializable type in a key - value mode.

#### Create a storage class

1. A Elements storage class is nedded, so inside /Assets/Scripts/ folder crete a new one called /DdbModels/ to create inside all data models. You can create many classes, each class is a diferent storage, so you can save in each storage a item with the same key but diferent type or value.
2. Create a new C# script named <name>DdbElements with the following code, replace "MainDdbElements" with your script name
3. "rootPath_" is the folder where the data will be stored, you can use Application.persistentDataPath or any other folder with read/write permissions

```C#

using UnityEngine;
using U.DalilaDB;

public class MainDdbElements : DalilaDBElements<MainDdbElements>
{
    protected override string rootPath_ => Application.persistentDataPath;
}

```

#### Save

1. To save an object use Save function
2. If you save two objects with the same key, the second will overwrite the first one
3. You can use enums or any object whith a valid ToString() function as key
4. The save function return a DataOperation object that contains the state of the operation
5. Keys only can containt letters and numbers

```C#

// Save the lives with "Lives" key
var saveOpp = MainDdbElements.Save(lives, "Lives");

// If save fail, print a log
if (!saveOpp)
    Debug.LogError("Cant save data, Error: " + saveOpp.Error);


// Save the possition
MainDdbElements.Save(transform.position, "Position");

```

#### Find/Read

1. To find an object use Find function thit the type of the object and the Key
3. The Find function return a DataOperation with Data
2. If the object exist DataOperation will be true and the object will be in .Data propertie
3. If cant foud the object, Dataoperation will be false 

```C#

// Find an int object with key "Lives"
var readOpp = MainDdbElements.Find<int>("Lives");

// If Lives key was find assign the Data to lives propertie 
if (readOpp)
    lives = readOpp.Data;
// If not found lives will be a default value
else
    lives = 10;

```

#### FindOrDefault/ReadOrDefault

1. If object cant be founded, then default value will be returned

```C#

// Find a Vector 3 with the position, if cant find will return a Vector3.Zero
var readOppPoss = MainDdbElements.FindOrDefault<Vector3>("Position", Vector3.zero);

// Assign the position to GameObject transform
transform.position = readOppPoss.Data;

```

#### Update

1. Updates an object, if object doesnt exist wont be created.

```C#

// Update Lives
var upp = MainDdbElements.Update<int>("Lives", (v) => { v = v + newLives; return v; });

// Update position
var uppPos = MainDdbElements.Update<Vector3>("Position", v => newPosition);

```

#### Delete

1. Delete an object
2. Operation will be true if object was deleted or doesnt exist

```C#

// Delete the key
MainDdbElements.Delete("Position");
MainDdbElements.Delete("Lives");

```

#### Example

1. This is an componet that store data, when the object is created the lives value and the position will be searched OnAwake()
2. OnDisable the values will be saved

```C#

using UnityEngine;

public class SampleScript : MonoBehaviour
{

    public int lives;

    private void Awake()
    {
        // Find an int object with key "Lives"
        var readOpp = MainDdbElements.Find<int>("Lives");

        // If Lives key was find assign the Data to lives propertie 
        if (readOpp)
            lives = readOpp.Data;
        // If not found lives will be a default value
        else
            lives = 10;


        // Find a Vector 3 with the position, if cant find will return a Vector3.Zero
        var readOppPoss = MainDdbElements.FindOrDefault<Vector3>("Position", Vector3.zero);

        // Assign the position to GameObject transform
        transform.position = readOppPoss.Data;
    }

    private void OnDisable()
    {
        // Save the lives with "Lives" key
        var saveOpp = MainDdbElements.Save(lives, "Lives");

        // If save fail, print a log
        if (!saveOpp)
            Debug.LogError("Cant save data, Error: " + saveOpp.Error);


        // Save the possition
        MainDdbElements.Save(transform.position, "Position");
    }

    public void DeleteSavedData()
    {
        // Delete the key
        MainDdbElements.Delete("Position");
        MainDdbElements.Delete("Lives");
    }

    public void UpdateSavedData(int newLives, Vector3 newPosition)
    {
        // Update Lives
        var upp = MainDdbElements.Update<int>("Lives", (v) => { v = v + newLives; return v; });

        // Update position
        var uppPos = MainDdbElements.Update<Vector3>("Position", v => newPosition);
    }

}


```

### Document

DalilaDBDocuments store all data from a class but only one copy of data will be stored, so if you crate many instances of the same document and save all of them, only the last saved will be stored. 

#### Create a storage class

1. A Document storage class is nedded, so inside /Assets/Scripts/ folder crete a new one called /DdbModels/ to create inside all data models. You can create many classes, each class is a diferent storage, so you can save many documents with diferent name.
2. Create a new C# script named <name>DdbDocument with the following code, replace "GameDdbDocument" with your script name
3. "rootPath_" is the folder where the data will be stored, you can use Application.persistentDataPath or any other folder with read/write permissions
4. All properties with [DataMember()] attribute will be stored, you can apply to public or private properties

```C#

using UnityEngine;
using U.DalilaDB;
using System.Runtime.Serialization;

[KnownType(typeof(GameDdbDocument))]
[DataContract()]
public class GameDdbDocument : DalilaDBDocument<GameDdbDocument>
{
    protected override string rootPath_ => Application.persistentDataPath;

    [DataMember()]
    public int lives;

    [DataMember()]
    public Vector3 position;

    [DataMember()]
    public float Time;
}

```

#### Save

1. To save data a instance of the class is needed, you can change data of the instance but it wont be saved.
2. To save the changes use Save() function of the instance
3. You can create many instances of the class, but saves will overwrite data, so the data stored will be the data of the last instance saved.

```C#

// DalilaDBDocument instance
GameDdbDocument match = new GameDdbDocument();

// Change data
match.Time = 23.4f;
match.lives -= 1;

// Save all data
match?.Save();


```

#### Find/Read

1. To find a saved instance use Find() function, if the instance donesnt exist the operation will be false, if exist the data will be in .Data propertie

```C#

// Instance of the document
GameDdbDocument match;

// Find the document
var opp = GameDdbDocument.Find();

// If cant find
if (!opp)
{
    Debug.Log("Cant find a saved match");
    match = new GameDdbDocument();
}
// If document exist assign to the instance
else
{
    match = opp.Data;
}

```

#### FindOrDefault/ReadOrDefault

1. If object cant be founded, then default value will be returned

```C#

// Find a saved document or return a new one and assign to match
var opp = GameDdbDocument.FindOrDefault(new GameDdbDocument());
GameDdbDocument match = opp.Data;

// If cant find a document just print a log
if (!opp)
    Debug.Log("Cant find a saved match");

```

#### Update

1. Update the saved document if exist

```C#

// Update
var opp = GameDdbDocument.Update(d => { d.lives++; return d; });

```

#### Delete

1. Delete the document

```C#

// Delete
GameDdbDocument.Delete();

```

#### Example

1. This is a sample use of the document to save the Time that the game is Played

```C#

using UnityEngine;

public class SampleScriptTwo : MonoBehaviour
{

    // Properties
    private float time = 0;

    // DalilaDBDocument instance
    private GameDdbDocument match;

    private void Awake()
    {
        // Find a saved document or return a new one and assign to match
        var opp = GameDdbDocument.FindOrDefault(new GameDdbDocument());
        match = opp.Data;

        // If cant find a document just print a log
        if (!opp)
            Debug.Log("Cant find a saved match");

    }

    private void Update()
    {
        // Add the elapsed time
        time += Time.deltaTime;
    }

    public void ChangeLives(int v)
    {
        // Change the lives value, but dont save it
        match.lives -= v;
    }

    public void DeleteSavedData()
    {
        // Delete the document
        GameDdbDocument.Delete();
    }


    private void OnDisable()
    {
        // Assing  the seconds of the match to the document
        match.Time += time;

        // Save all data
        match?.Save();
    }

}


```

### Collection

DalilaDBCollections are like MongoDB collections, store many copy of the class with a unique id, so you can save many Users or items and query the collection to find all with a certain properti or one with a specific propertie.

#### Create a storage class

1. A Collection storage class is nedded, so inside /Assets/Scripts/ folder crete a new one called /DdbModels/ to create inside all data models. You can create many classes, each class is a diferent storage, so you can save many collections with diferent name.
2. Create a new C# script named <name>DdbCollection with the following code, replace "UserDdbCollection" with your script name
3. "rootPath_" is the folder where the data will be stored, you can use Application.persistentDataPath or any other folder with read/write permissions
4. All properties with [DataMember()] attribute will be stored, you can apply to public or private properties


```C#

using UnityEngine;
using U.DalilaDB;
using System.Runtime.Serialization;

[KnownType(typeof(UserDdbCollection))]
[DataContract()]
public class UserDdbCollection : DalilaDBCollection<UserDdbCollection>
{
    protected override string rootPath_ => throw new System.NotImplementedException();

    [DataMember()]
    public string username;

    [DataMember()]
    public int maxScore;

    [DataMember()]
    public float minTime;

    [DataMember()]
    public Vector3 lastPosition;

}

```

#### Save

- You can create a new object creating a new instance of the class
- You can set or not the _id, if is not set a new one will be generated
- _id is a string with 19 numbers, only numbers are allowed
- To save a instance use Save() method
- If instance is modified you need to use Save() each time
- If you save two documents with the same key, the second will overwrite the first use SaveNew() to crate a copy with diferent _id

```C#

// Save a element with id
var newd1 = new User { _id = "4961429416443432069", count = 66, name = "Pedro" };
newd1.Save();

// Save a element without id, a random id will be generated
var newd2 = new User { count = 77, name = "Paco" };
newd2.Save();

// Save an IEnumerable of elements
var userList = new User[]
{
    new User { count = 66, name = "Pedro" },
    new User { count = 66, name = "Pedro" },
    new User { count = 66, name = "Pedro" },
    new User { count = 66, name = "Pedro" },
    new User { count = 66, name = "Pedro" },
    new User { count = 66, name = "Pedro" },
};
var opp = userList.Save();

// If SaveNew is used a new key is generated each time for he document, four documents will be created
newd1.SaveNew();
newd1.SaveNew();
newd1.SaveNew();
newd1.SaveNew();

```

#### FindById

You can find a document by id

```C#

// Create one document
var newd1 = new User { _id = "4961429416443432069", count = 66, name = "Pedro" };

// Save
Assert.IsTrue(newd1.Save());

// Only one document will be stored
Assert.IsTrue(User.Count() == 1);

// Find the document by id
var opp = User.FindById("4961429416443432069");
Assert.IsTrue(opp);
Assert.IsTrue(opp.Data.count == 66);

// If you try to find an unexistent document will return null
var opp2 = User.FindById("4961429416443432010");
Assert.IsFalse(opp2);
Assert.IsTrue(opp2.Data == null);

```

#### FindByIdOrDefault

```C#

var defaultDoc = new User { count = 11, };

// Create one document
Debug.Log("Save");
var newd1 = new User { _id = "4961429416443432069", count = 66, name = "Pedro" };

// The first one will be saved
Assert.IsTrue(newd1.Save());

// Only one document will be stored
Assert.IsTrue(User.Count() == 1);
Assert.IsTrue(User.Exist("4961429416443432069"));

// Find the document by the id
var opp = User.FindByIdOrDefault("4961429416443432069", defaultDoc);
Assert.IsTrue(opp);
Assert.IsTrue(opp.Data.count == 66);

// If you try to find an unexistent document will return default
var opp2 = User.FindByIdOrDefault("4961429416443432010", defaultDoc);
Assert.IsTrue(opp2);
Assert.IsTrue(opp2.Data.count == 11);

```

#### FindOne

You can find a document with specified value, similar to LINQ

```C#

// Create five documents
Debug.Log("Save");
var newd1 = new User { count = 66, name = "Pedro" };
var newd2 = new User { count = 66, name = "Paco" };
var newd3 = new User { count = 77, name = "Benito" };
var newd4 = new User { count = 88, name = "Juan" };
var newd5 = new User { count = 99, name = "Ale" };

// Save
Assert.IsTrue(newd1.Save());
Assert.IsTrue(newd2.Save());
Assert.IsTrue(newd3.Save());
Assert.IsTrue(newd4.Save());
Assert.IsTrue(newd5.Save());

// Check
Assert.IsTrue(User.Count() == 5);

// Find a document
var opp = User.FindOne(d => d.count == 66);
Assert.IsTrue(opp);
Assert.IsTrue(opp.Data.count == 66);
// Find a document
opp = User.FindOne(d => d.count == 77);
Assert.IsTrue(opp);
Assert.IsTrue(opp.Data.count == 77);
// Find a document
opp = User.FindOne(d => d.name == "Ale");
Assert.IsTrue(opp);
Assert.IsTrue(opp.Data.name == "Ale");
// Find a document
opp = User.FindOne(d => d.name == "Juan");
Assert.IsTrue(opp);
Assert.IsTrue(opp.Data.count == 88);

// If you try to find an unexistent document will return null
opp = User.FindOne(d => d.name == "Ema");
Assert.IsFalse(opp);
Assert.IsTrue(opp.Data == null);
// If you try to find an unexistent document will return null
opp = User.FindOne(d => d.count == 100);
Assert.IsFalse(opp);
Assert.IsTrue(opp.Data == null);

```

#### FindMany

```C#

// Create six documents
Debug.Log("Save");
var newd1 = new User { count = 66, name = "Pedro" };
var newd2 = new User { count = 66, name = "Paco" };
var newd3 = new User { count = 77, name = "Benito" };
var newd4 = new User { count = 88, name = "Juan" };
var newd5 = new User { count = 33, name = "Juan" };
var newd6 = new User { count = 99, name = "Ale" };


// Save
Assert.IsTrue(newd1.Save());
Assert.IsTrue(newd2.Save());
Assert.IsTrue(newd3.Save());
Assert.IsTrue(newd4.Save());
Assert.IsTrue(newd5.Save());
Assert.IsTrue(newd6.Save());

// Check
Assert.IsTrue(User.Count() == 6);

// Find documents
var opp = User.FindMany(d => d.count == 66);
Assert.IsTrue(opp);
Assert.IsTrue(opp.Data.Length == 2);
foreach(var d in opp.Data)
{
    Assert.IsTrue(d.count == 66);
}
// Find documents
opp = User.FindMany(d => d.name == "Juan");
Assert.IsTrue(opp);
Assert.IsTrue(opp.Data.Length == 2);
foreach (var d in opp.Data)
{
    Assert.IsTrue(d.name == "Juan");
}
// Find documents
opp = User.FindMany(d => d.name == "Benito");
Assert.IsTrue(opp);
Assert.IsTrue(opp.Data.Length == 1);
foreach (var d in opp.Data)
{
    Assert.IsTrue(d.name == "Benito");
}
// If you try to find an unexistent document will return empty array and false
opp = User.FindMany(d => d.name == "Ema");
Assert.IsFalse(opp);
Assert.IsTrue(opp.Data.Length == 0);

// If you try to find an unexistent document will return empty array and false
opp = User.FindMany(d => d.count == 11);
Assert.IsFalse(opp);
Assert.IsTrue(opp.Data.Length == 0);


```

#### FindAll

```C#

// Create six documents
Debug.Log("Save");
var newd1 = new User { count = 66, name = "Pedro" };
var newd2 = new User { count = 66, name = "Paco" };
var newd3 = new User { count = 77, name = "Benito" };
var newd4 = new User { count = 88, name = "Juan" };
var newd5 = new User { count = 33, name = "Juan" };
var newd6 = new User { count = 99, name = "Ale" };


// Find all document when no documents are stored will return empyty array
var opp = User.FindAll();
Assert.IsFalse(opp);
Assert.IsTrue(opp.Data.Length == 0);


// Save
Assert.IsTrue(newd1.Save());
Assert.IsTrue(newd2.Save());
Assert.IsTrue(newd3.Save());
Assert.IsTrue(newd4.Save());
Assert.IsTrue(newd5.Save());
Assert.IsTrue(newd6.Save());

// Check
Assert.IsTrue(User.Count() == 6);

// Find all documents
opp = User.FindAll();
Assert.IsTrue(opp);
Assert.IsTrue(opp.Data.Length == 6);


```

#### Update

```C#

public static DataOperation UpdateById(Func<TCollection, TCollection> updating, SID id);
public static DataOperation UpdateAll(Func<TCollection, TCollection> updating);
public static DataOperation UpdateOne(Func<TCollection, TCollection> updating, Func<TCollection, bool> predicate);
public static DataOperation UpdateMany(Func<TCollection, TCollection> updating, Func<TCollection, bool> predicate);

```

#### Delete

```C#

public static DataOperation DeleteById(SID id);
public static DataOperation DeleteAll();
public static DataOperation DeleteOne(Func<TCollection, bool> predicate);
public static DataOperation DeleteMany(Func<TCollection, bool> predicate);

```

## Store Volatile Data

Volatile storage save data to RAM memory, the behaviour is the same as perstent stores, tha main diference is that all data will lose if application is closed.

#### Volatile Elements

1. Inherits from "DalilaDBVolatileElements" intead of "DalilaDBElements"
2. "rootPath_" is not needed because data wont be store in hard disk
3. "cacheSize_" is the max number of elements that can be stored

```C#

using U.DalilaDB;

public class MainDdbVolatileElements : DalilaDBVolatileElements<MainDdbVolatileElements>
{
    protected override int cacheSize_ => 100;
}

```

#### Volatile Document

1. Inherits from "DalilaDBVolatileDocument" intead of "DalilaDBDocument"
2. "rootPath_" is not needed because data wont be store in hard disk

```C#

using UnityEngine;
using U.DalilaDB;
using System.Runtime.Serialization;

[KnownType(typeof(GameDdbVolatileDocument))]
[DataContract()]
public class GameDdbVolatileDocument : DalilaDBVolatileDocument<GameDdbVolatileDocument>
{
    [DataMember()]
    public int lives;

    [DataMember()]
    public Vector3 position;

    [DataMember()]
    public float Time;
}

```

#### Volatile Collection

1. Inherits from "UserDdbVolatileCollection" intead of "UserDdbCollection"
2. "rootPath_" is not needed because data wont be store in hard disk
3. "cacheSize_" is the max number of elements that can be stored

```C#

using UnityEngine;
using U.DalilaDB;
using System.Runtime.Serialization;

[KnownType(typeof(UserDdbVolatileCollection))]
[DataContract()]
public class UserDdbVolatileCollection : DalilaDBVolatileCollection<UserDdbVolatileCollection>
{
    protected override int cacheSize_ => 100;

    [DataMember()]
    public string username;

    [DataMember()]
    public int maxScore;

    [DataMember()]
    public float minTime;

    [DataMember()]
    public Vector3 lastPosition;

}

```


```C#



```

## Encryption

DalilaDB implements AES128 or AES256 encryption for persistent data stores, and SHA256 to hash passwords or text, both are disabled by default 

### Encrypt data stores

All DalilaDB data stores can be encrypted, to enable encryption override _aesEncryption method, you can change some parameters of encryption like the key size and the key used to encrypt data.

You can harcode the key n you proyect or use an external service to store it, try not to store in a document

```C#

    // Enable encryption
    protected override bool _aesEncryption => true;

    // Optional parameters
    protected override DalilaFS.aesValidKeySizes _aesKeySize => DalilaFS.aesValidKeySizes.aes256;
    protected override string _aesFixedKey => "ThisIsTheKey";

```

An unencrypted document can be easily readed and modified

![DalilaDBInstallation1](https://images4public4ccess.s3.amazonaws.com/DalilaDB/DalilaDBEncryption1.JPG)

An encrypted document cant be readed or modified without the key

![DalilaDBInstallation1](https://images4public4ccess.s3.amazonaws.com/DalilaDB/DalilaDBEncryption2.JPG)

The following example shows how to use encryption in Elements, Documets and Collections

```C#

    class SimpleEncryptedElements : DalilaDBElements<SimpleEncryptedElements>
    {

        // Enable encryption
        protected override bool _aesEncryption => true;

        // Optional parameters
        protected override DalilaFS.aesValidKeySizes _aesKeySize => DalilaFS.aesValidKeySizes.aes256;
        protected override string _aesFixedKey => "ThisIsTheKey";

    }


    [KnownType(typeof(UserEncryptedDocument))]
    [DataContract()]
    class UserEncryptedDocument : DalilaDBDocument<UserEncryptedDocument>
    {

        [DataMember()]
        public int count;

        // Enable encryption
        protected override bool _aesEncryption => true;

        // Optional parameters
        protected override DalilaFS.aesValidKeySizes _aesKeySize => DalilaFS.aesValidKeySizes.aes256;
        protected override string _aesFixedKey => "ThisIsTheKey";

    }


    [KnownType(typeof(UserEncrypted))]
    [DataContract()]
    class UserEncrypted : DalilaDBCollection<UserEncrypted>
    {

        [DataMember()]
        public int count;

        [DataMember()]
        public string name;

        // Enable encryption
        protected override bool _aesEncryption => true;

        // Optional parameters
        protected override DalilaFS.aesValidKeySizes _aesKeySize => DalilaFS.aesValidKeySizes.aes256;
        protected override string _aesFixedKey => "ThisIsTheKey";

    }


```

### Hash Passwords

Is a bad idea store passwords in plain text, because can be easily readed

![DalilaDBInstallation3](https://images4public4ccess.s3.amazonaws.com/DalilaDB/DalilaDBEncryption3.JPG)

You can use a object of type secret to store this data so it cant be readed

![DalilaDBInstallation3](https://images4public4ccess.s3.amazonaws.com/DalilaDB/DalilaDBEncryption4.JPG)

The secret object hash a string ans store the hashed value

```C#

    // Hash a string
    password = new Secret("PasswordInPlainText")

    // Compare Secrets
    Assert.IsTrue(password.Compare("PasswordInPlainText"));
    Assert.IsFalse(password.Compare("ThisIs not the pasword"));

```

In this example a password is saved and readed in DalilaDBElements

```C#
    
    using U.DalilaDB;

    class HashedElementsStore : DalilaDBElements<HashedElementsStore> { }

    // Save the password
    var opp = HashedElementsStore.Save("Password", new Secret("PasswordInPlainText"));
    Debug.Log("Saved as Int: " + opp);
    Assert.IsTrue(opp);

    // Now exist
    Debug.Log("Now Exist: " + HashedElementsStore.Exist("Password"));
    Assert.IsTrue(HashedElementsStore.Exist("Password"));
    Assert.IsTrue(HashedElementsStore.Count() == 1);

    // Read the hash
    var readOpp = HashedElementsStore.Find<Secret>("Password");
    Assert.IsTrue(readOpp);

    // Compare Secrets
    Assert.IsTrue(readOpp.Data.Compare("PasswordInPlainText"));
    Assert.IsFalse(readOpp.Data.Compare("ThisIs not the pasword"));

```

In this example a password is saved and readed in DalilaDBDocument

```C#

    using U.DalilaDB;

    [KnownType(typeof(HashedDocument))]
    [DataContract()]
    class HashedDocument : DalilaDBDocument<HashedDocument>
    {

        [DataMember()]
        public Secret password;

    }

    // Create a instance with a password
    var user1 = new HashedDocument(); user1.password = new Secret("PasswordInPlainText");

    // save
    Debug.Log("Saving");
    Assert.IsTrue(user1.Save());

    // Now find the document
    var readOpp = HashedDocument.Find();
    Assert.IsTrue(readOpp);

    // Compare Secrets
    Assert.IsTrue(readOpp.Data.password.Compare("PasswordInPlainText"));
    Assert.IsFalse(readOpp.Data.password.Compare("ThisIs not the pasword"));

```

In this example a password is saved and readed in DalilaDBCollection

```C#

    using U.DalilaDB;

    [KnownType(typeof(HashedCollection))]
    [DataContract()]
    class HashedCollection : DalilaDBCollection<HashedCollection>
    {

        [DataMember()]
        public Secret password;

    }

    // You can create multiple instances of the class with diferent passwords
    var user1 = new HashedCollection(); user1.password = new Secret("PasswordInPlainTextOne");
    var user2 = new HashedCollection(); user2.password = new Secret("PasswordInPlainTextTwo");

    // save
    Debug.Log("Saving");
    Assert.IsTrue(user1.Save());
    Assert.IsTrue(user2.Save());

    // Now find the data
    var readOpp1 = HashedCollection.FindById(user1._id);
    var readOpp2 = HashedCollection.FindById(user2._id);
    Assert.IsTrue(readOpp1);
    Assert.IsTrue(readOpp2);

    // Compare Secrets of User One
    Assert.IsTrue(readOpp1.Data.password.Compare("PasswordInPlainTextOne"));
    Assert.IsFalse(readOpp1.Data.password.Compare("PasswordInPlainTextTwo"));
    Assert.IsFalse(readOpp1.Data.password.Compare("ThisIs not the pasword"));

    // Compare Secrets of User Two
    Assert.IsFalse(readOpp2.Data.password.Compare("PasswordInPlainTextOne"));
    Assert.IsTrue(readOpp2.Data.password.Compare("PasswordInPlainTextTwo"));
    Assert.IsFalse(readOpp2.Data.password.Compare("ThisIs not the pasword"));

```


## 


```C#



```