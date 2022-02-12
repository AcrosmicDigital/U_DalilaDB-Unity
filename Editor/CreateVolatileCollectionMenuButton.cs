using UnityEditor;
using static U.DalilaDB.Editor.UE;

namespace U.Reactor.Editor
{
    public class CreateVolatileCollectionMenuButton : EditorWindow
    {

        #region File
        private static string DefaultFolderName => "/Scripts/DalilaDocs/Models/";
        private static string DefaultFileName => "NewVCC";
        private static string CustomExtension => "vcollection";
        static string[] File(string fileName) => new string[]
        {
            "using System.Runtime.Serialization;",
            "using U.DalilaDB;",
            "using UnityEngine;",
            "using System.Collections.Generic;",
            "",
            "public static partial class DBmodels",
            "{",
            "    [KnownType(typeof("+fileName+"))]",
            "    [DataContract()]",
            "    public class "+fileName+" : DalilaDBVolatileCollection<"+fileName+">",
            "    {",
            "",
            "        #region Config",
            "",
            "        //protected override int cacheSize_ => 10;  // Number of elements",
            "",
            "        #endregion Config",
            "",
            "",
            "        // Write members here",
            "",
            "        //[DataMember()]",
            "        //public string playerName { get; set; }",
            "",
            "        //[DataMember()]",
            "        //public int maxScore { get; set; }",
            "",
            "        //...",
            "",
            "    }",
            "}",
        };
        #endregion File



        private static string FormatLog(string text) => "DalilaDB: " + text;


        [MenuItem("Universal/DalilaDB/Create/Volatile Collection")]
        public static void ShowWindow()
        {

            // Create files
            CreateFileWithSaveFilePanelAndCustomExtension(DefaultFolderName, DefaultFileName, File, FormatLog, CustomExtension);

            // Compile
            AssetDatabase.Refresh();

        }

    }
}