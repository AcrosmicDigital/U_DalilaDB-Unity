using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using U.DalilaDB;

namespace DalilaFsTests
{
    public class DalilaFS_Basics_W2Validations
    {


        [Test]
        public void A1_IsValidResource_WhenValidResourcePassed_MustBeTrue()
        {

            foreach (var resource in DataSource_SacrumFileSystem.validResources)
            {
                Debug.Log("Valid Resource: " + resource);
                Assert.IsTrue(DalilaFS.IsValidResource(resource));
            }

        }


        [Test]
        public void A2_IsValidResource_WhenInvalidResourcePassed_MustBeFalse()
        {

            foreach (var resource in DataSource_SacrumFileSystem.invalidResources)
            {
                Debug.Log("Invalid Resource: " + resource);
                Assert.IsFalse(DalilaFS.IsValidResource(resource));
            }

        }




        [Test]
        public void B1_IsValidLocation_WhenValidLocationPassed_MustBeTrue()
        {

            foreach (var location in DataSource_SacrumFileSystem.validLocations)
            {
                Debug.Log("Valid Location: " + location);
                Assert.IsTrue(DalilaFS.IsValidLocation(location));
            }

        }


        [Test]
        public void B2_IsValidLocation_WhenInvalidLocationPassed_MustBeFalse()
        {

            foreach (var location in DataSource_SacrumFileSystem.invalidlocations)
            {
                Debug.Log("Invalid Location: " + location);
                Assert.IsFalse(DalilaFS.IsValidLocation(location));
            }

        }


    }
}
