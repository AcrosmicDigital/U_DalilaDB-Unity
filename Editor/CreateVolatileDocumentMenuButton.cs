using UnityEditor;
using static U.DalilaDB.Editor.UE;

namespace U.Reactor.Editor
{
    public class CreateVolatileDocumentMenuButton : EditorWindow
    {

        #region File
        private static string DefaultFolderName => "/Scripts/DalilaDocs/Models/";
        private static string DefaultFileName => "NewVDD";
        private static string CustomExtension => "vdocument";
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
            "    public class "+fileName+" : DalilaDBVolatileDocument<"+fileName+">",
            "    {",
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


        [MenuItem("Universal/DalilaDB/Create/Volatile Document")]
        public static void ShowWindow()
        {

            // Create files
            CreateFileWithSaveFilePanelAndCustomExtension(DefaultFolderName, DefaultFileName, File, FormatLog, CustomExtension);

            // Compile
            AssetDatabase.Refresh();

        }

    }
}