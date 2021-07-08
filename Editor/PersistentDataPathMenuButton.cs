using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Linq;
using U.DalilaDB;


#if UNITY_EDITOR

public class PersistentDataPathMenuButton : EditorWindow
{
    [MenuItem("U/DalilaDB/Persistent Data Path")]
    public static void ShowWindow()
    {
        Application.OpenURL(Application.persistentDataPath);
    }
}

#endif