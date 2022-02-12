using UnityEditor;
using static U.DalilaDB.Editor.UE;

namespace U.Reactor.Editor
{
    public class CreateAPIControllerMenuButton : EditorWindow
    {

        #region File
        private static string DefaultFolderName => "/Scripts/Controllers/";
        private static string DefaultFileName => "NewAPI";
        private static string CustomExtension => "api.controller";
        static string[] File(string fileName) => new string[]
        {
            "using UnityEngine;",
            "using System.Linq;",
            "using System.Collections.Generic;",
            "using U.DalilaDB;",
            "",
            "public static partial class Control",
            "{",
            "    public static partial class API",
            "    {",
            "        public static partial class "+fileName+"",
            "        {",
            "",
            "            // Write functions here",
            "            // ...",
            "",
            "        }",
            "    }",
            "}",
        };
        #endregion File



        private static string FormatLog(string text) => "DalilaDB: " + text;


        [MenuItem("Universal/DalilaDB/Create/API Controller")]
        public static void ShowWindow()
        {

            // Create files
            CreateFileWithSaveFilePanelAndCustomExtension(DefaultFolderName, DefaultFileName, File, FormatLog, CustomExtension);

            // Compile
            AssetDatabase.Refresh();

        }

    }
}