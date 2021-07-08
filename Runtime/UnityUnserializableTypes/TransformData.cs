using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace U.DalilaDB
{
    [Serializable]
    public class TransformData
    {

        private TransformData() { }  // Private Constructor


        #region Properties - This values will be serialized

        public Vector3 position;
        public Quaternion rotation;
        public Vector3 localScale;

        #endregion Properties

        #region Store - Functions to copy data from the object


        public static TransformData Store(Transform t)
        {
            var d = new TransformData();

            d.position = t.position;
            d.rotation = t.rotation;
            d.localScale = t.localScale;


            return d;
        }

        public static TransformData Store(GameObject g)
        {
            return Store(g.transform);
        }


        #endregion Store

        #region Set - Functions to set stored data to an existent object


        public Transform Set(Transform t)
        {
            t.position = position;
            t.rotation = rotation;
            t.localScale = localScale;

            return t;
        }

        public Transform Set(GameObject g)
        {
            Set(g.transform);
            return g.transform;
        }


        #endregion Set

        #region Create - Functions to create a new instance of the object with the stored data

        //... Cant create a transform without a gameobject

        #endregion Create

    }
}
