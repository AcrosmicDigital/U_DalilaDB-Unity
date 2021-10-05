using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using U.DalilaDB;
using System;

namespace DalilaFsTests
{
    public class DalilaFS_Basics_W3GetSubLocationsOrResourceName
    {


        // A Test behaves as an ordinary method
        [Test]
        public void A1_GetPrevLocation_WhenValidLocationsPasses_WillReturnThePrevLocation()
        {

            for (int i = 0; i < DataSource_SacrumFileSystem.validLocations.Length; i++)
            {
                Debug.Log("Location: " + DataSource_SacrumFileSystem.validLocations[i] + " Prev: " + DalilaFS.GetPrevLocation(DataSource_SacrumFileSystem.validLocations[i]));
                Assert.IsTrue(DalilaFS.GetPrevLocation(DataSource_SacrumFileSystem.validLocations[i]) == DataSource_SacrumFileSystem.validLocationsPrev[i]);
            }

        }


        // A Test behaves as an ordinary method
        [Test]
        public void A2_GetPrevLocation_WhenInvalidLocationsPasses_WillThrowError()
        {

            foreach (var location in DataSource_SacrumFileSystem.invalidlocations)
            {
                Assert.Throws<FormatException>(() => DalilaFS.GetPrevLocation(location));
            }

        }


        // A Test behaves as an ordinary method
        [Test]
        public void A3_GetPrevLocation_CanBeChained_AndWillReturnThePrevPrevLocation()
        {

            for (int i = 0; i < DataSource_SacrumFileSystem.validLocations.Length; i++)
            {
                Debug.Log("Location: " + DataSource_SacrumFileSystem.validLocations[i] + " PrevPrev: " + DalilaFS.GetPrevLocation(DalilaFS.GetPrevLocation(DataSource_SacrumFileSystem.validLocations[i])));
                Assert.IsTrue(DalilaFS.GetPrevLocation(DalilaFS.GetPrevLocation(DataSource_SacrumFileSystem.validLocations[i])) == DataSource_SacrumFileSystem.validLocationsPrevPrev[i]);
            }

        }




        // A Test behaves as an ordinary method
        [Test]
        public void B1_GetResourceLocation_WhenValidResourcePasses_WillReturnTheLocation()
        {

            for (int i = 0; i < DataSource_SacrumFileSystem.validResources.Length; i++)
            {
                Debug.Log("Resource: " + DataSource_SacrumFileSystem.validResources[i] + " Location: " + DalilaFS.GetResourceLocation(DataSource_SacrumFileSystem.validResources[i]));
                Assert.IsTrue(DalilaFS.GetResourceLocation(DataSource_SacrumFileSystem.validResources[i]) == DataSource_SacrumFileSystem.validResourcesLocation[i]);
            }

        }


        // A Test behaves as an ordinary method
        [Test]
        public void B2_GetResourceLocation_WhenInvalidResourcePasses_WillThrowError()
        {

            foreach (var resource in DataSource_SacrumFileSystem.invalidResources)
            {
                Assert.Throws<FormatException>(() => DalilaFS.GetResourceLocation(resource));
            }

        }




        // A Test behaves as an ordinary method
        [Test]
        public void B3_GetPrevLocation_CanbeChainedWithGetAlternPrevLocation_AndWillReturnThePrevPrevLocation()
        {

            for (int i = 0; i < DataSource_SacrumFileSystem.validResources.Length; i++)
            {
                Debug.Log("Resource: " + DataSource_SacrumFileSystem.validResources[i] + " LocationPrev: " + DalilaFS.GetPrevLocation(DalilaFS.GetResourceLocation(DataSource_SacrumFileSystem.validResources[i])));
                Assert.IsTrue(DalilaFS.GetPrevLocation(DalilaFS.GetResourceLocation(DataSource_SacrumFileSystem.validResources[i])) == DataSource_SacrumFileSystem.validResourcesLocationPrev[i]);
            }

        }


    }
}
