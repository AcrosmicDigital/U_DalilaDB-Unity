using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace U.DalilaDB
{
    [Serializable]
    public class GameObjectData
    {

        private GameObjectData() { }  // Private Constructor


        #region Properties - This values will be serialized

        public string name;
        public TransformData transformData;

        #endregion Properties

        #region Store - Functions to copy data from the object


        public static GameObjectData Store(GameObject gameObject)
        {
            var d = new GameObjectData();

            d.name = gameObject.name;
            d.transformData = TransformData.Store(gameObject);

            return d;
        }


        #endregion Store

        #region Set - Functions to set stored data to an existent object


        public GameObject Set(GameObject g)
        {
            g.name = name;
            transformData.Set(g.transform);

            return g;
        }


        #endregion Set

        #region Create - Functions to create a new instance of the object with the stored data


        public GameObject Create()
        {
            return Set(new GameObject()); ;
        }


        #endregion Create

    }
}
