using UnityEngine;
using UnityEditor;

namespace U.DalilaDB.Editor
{
    public class PersistentDataPathMenuButton : EditorWindow
    {
        [MenuItem("Universal/DalilaDB/Persistent Data Path")]
        public static void ShowWindow()
        {
            Application.OpenURL(Application.persistentDataPath);
        }
    }
}