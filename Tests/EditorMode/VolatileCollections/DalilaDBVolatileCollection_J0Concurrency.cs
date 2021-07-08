using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using NUnit.Framework;
using U.DalilaDB;
using UnityEngine;
using UnityEngine.TestTools;

public class DalilaVolatileCollection_J0Concurrency
{


    #region Example classes

    [KnownType(typeof(CollectionInDefaultPath))]
    [DataContract()]
    class CollectionInDefaultPath : DalilaDBVolatileCollection<CollectionInDefaultPath>
    {

        [DataMember()]
        public int count;

        protected override int cacheSize_ => 1000;

    }

    [KnownType(typeof(CollectionInChangePath))]
    [DataContract()]
    class CollectionInChangePath : DalilaDBVolatileCollection<CollectionInChangePath>
    {

        [DataMember()]
        public int count;

        protected override int cacheSize_ => 1000;

    }





    #endregion Example classes



    [UnityTest]
    public IEnumerator DalilaVolatileCollection_1ConcurrencyWithEnumeratorPasses()
    {

        // Delete the directory to clear it
        CollectionInDefaultPath.DeleteAll();

        // Create 500 new doocuments
        Debug.Log("Creating");
        var elementsList = new CollectionInDefaultPath[] {
            new CollectionInDefaultPath {
                count = 55,
            },
            new CollectionInDefaultPath {
                count = 66,
            },
            new CollectionInDefaultPath {
                count = 77,
            },
            new CollectionInDefaultPath {
                count = 88,
            },
            new CollectionInDefaultPath {
                count = 99,
            },
        };

        for (int i = 0; i < 100; i++)
        {
            elementsList.SaveNew();
        }

        // Check if Data is Saved
        Debug.Log("Saved: " + CollectionInDefaultPath.Count());
        Assert.IsTrue(CollectionInDefaultPath.Count() == 500);

        // Create 10 concurrent FindTasks
        var tasks = new List<Task<CollectionInDefaultPath[]>>();

        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                var ee = CollectionInDefaultPath.FindAll().Data;
                return ee;
            }));
        }

        // Wait for the findTasks to complete
        var waitTask = Task.WhenAll(tasks);
        while (!waitTask.IsCompleted)
        {
            yield return null;
        }

        if (waitTask.IsFaulted)
        {
            throw waitTask.Exception;
        }

        // Confim the number of files readed on each find
        foreach (var task in waitTask.Result)
        {
            Debug.Log("Readed: " + task.Length);
            Assert.IsTrue(task.Length == 500);
        }


        //Final Number of files
        Debug.Log("Exist FINAL are: " + CollectionInDefaultPath.Count());
        Assert.IsTrue(CollectionInDefaultPath.Count() == 500);

        // Files of number to seeee
        Assert.IsTrue(CollectionInDefaultPath.FindMany(d => d.count == 55).Data.Length == 100);
        Assert.IsTrue(CollectionInDefaultPath.FindMany(d => d.count == 66).Data.Length == 100);
        Assert.IsTrue(CollectionInDefaultPath.FindMany(d => d.count == 77).Data.Length == 100);
        Assert.IsTrue(CollectionInDefaultPath.FindMany(d => d.count == 88).Data.Length == 100);
        Assert.IsTrue(CollectionInDefaultPath.FindMany(d => d.count == 99).Data.Length == 100);

    }


    [UnityTest]
    public IEnumerator DalilaVolatileCollection_2ConcurrencyWithEnumeratorPasses()
    {

        // Delete the directory to clear it
        CollectionInDefaultPath.DeleteAll();


        // Find Tasks Create 100 concurrent Tasks groups
        var saveTasks = new List<Task<DataOperation>>();

        for (int i = 0; i < 100; i++)
        {
            var ii = i;
            saveTasks.Add(Task.Run(() =>
            {
                var newd1 = new CollectionInDefaultPath { count = ii, };
                return newd1.Save();
            }));
        }

        var waitSaveTask = Task.WhenAll(saveTasks);
        while (!waitSaveTask.IsCompleted)
        {
            yield return null;
        }

        if (waitSaveTask.IsFaulted)
        {
            throw waitSaveTask.Exception;
        }


        // Find Tasks Create 50 concurrent Tasks groups
        var findTasks = new List<Task<CollectionInDefaultPath[]>>();

        for (int i = 0; i < 50; i++)
        {
            findTasks.Add(Task.Run(() =>
            {
                var ee = CollectionInDefaultPath.FindAll().Data;
                return ee;
            }));
        }

        var waitFindTask = Task.WhenAll(findTasks);
        while (!waitFindTask.IsCompleted)
        {
            yield return null;
        }

        if (waitFindTask.IsFaulted)
        {
            throw waitFindTask.Exception;
        }

        // Confim the number of reads
        foreach (var task in waitFindTask.Result)
        {
            Debug.Log("Readed: " + task.Length);
            Assert.IsTrue(task.Length == 100);
        }



        // Update Tasks Create 50 concurrent Tasks groups
        var updateTasks = new List<Task<DataOperation>>();

        for (int i = 0; i < 10; i++)
        {
            updateTasks.Add(Task.Run(() =>
            {
                var ee = CollectionInDefaultPath.UpdateAll(d => { d.count = 10; return d; });
                Debug.Log("Up: " + ee.Message);
                return ee;
            }));
        }

        var waitUpdateTask = Task.WhenAll(updateTasks);
        while (!waitUpdateTask.IsCompleted)
        {
            yield return null;
        }

        if (waitUpdateTask.IsFaulted)
        {
            throw waitUpdateTask.Exception;
        }



        // Find Tasks Create 50 concurrent Tasks groups if updated
        findTasks = new List<Task<CollectionInDefaultPath[]>>();

        for (int i = 0; i < 50; i++)
        {
            findTasks.Add(Task.Run(() =>
            {
                var ee = CollectionInDefaultPath.FindMany(d => d.count == 10).Data;
                Debug.Log("Find Updated: " + ee.Length);
                Assert.IsTrue(ee.Length == 100);
                return ee;
            }));
        }

        waitFindTask = Task.WhenAll(findTasks);
        while (!waitFindTask.IsCompleted)
        {
            yield return null;
        }

        if (waitFindTask.IsFaulted)
        {
            throw waitFindTask.Exception;
        }





        //Files in path
        Debug.Log("Exist FINAL are: " + CollectionInDefaultPath.Count());
        Assert.IsTrue(CollectionInDefaultPath.Count() == 100);

    }


}
