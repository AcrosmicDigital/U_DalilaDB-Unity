using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR

public class VersionMenuButton : EditorWindow
{

    [MenuItem("U/DalilaDB/Version")]
    public static void PrintVersion()
    {
        
        Debug.Log(" U Framework: DalilaDB v1.0.0 for Unity");
         
    }
}


#endif
