using UnityEngine;
using UnityEditor;

namespace U.DalilaDB.Editor
{
    public class VersionMenuButton : EditorWindow
    {

        [MenuItem("Universal/DalilaDB/Version")]
        public static void PrintVersion()
        {

            Debug.Log(" U Framework: DalilaDB v1.0.0 for Unity");

        }
    }
}