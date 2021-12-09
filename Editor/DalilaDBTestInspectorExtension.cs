using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(DalilaDBTest))]
internal class DalilaDBTestInspectorExtension : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Label("Only for tests purpose");
        GUILayout.Label("Remove in final release");
    }
}

#endif