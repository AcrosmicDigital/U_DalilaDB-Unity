using UnityEditor;
using static U.DalilaDB.Editor.UE;

namespace U.Reactor.Editor
{
    public class CreateElementsMenuButton : EditorWindow
    {

        #region File
        private static string DefaultFolderName => "/Scripts/DalilaDocs/Models/";
        private static string DefaultFileName => "NewEE";
        private static string CustomExtension => "elements";
        static string[] File(string fileName) => new string[]
        {
            "using U.DalilaDB;",
            "using UnityEngine;",
            "",
            "public static partial class DBmodels",
            "{",
            "    public class "+fileName+" : DalilaDBElements<"+fileName+">",
            "    {",
            "",
            "        #region Config",
            "",
            "        //protected override string rootPath_ => Application.persistentDataPath;  // Change default data save path ",
            "        //protected override int cacheSize_ => 10;  // Number of elements in cache",
            "        //protected override bool _aesEncryption => true;  // Enable encryption",
            "        //protected override DalilaFS.aesValidKeySizes _aesKeySize => DalilaFS.aesValidKeySizes.aes256;  // Aes key size",
            "        //protected override string _aesFixedKey => "+quote+"TheKeyIsNew"+quote+";  // Aes Key",
            "",
            "        #endregion Config",
            "",
            "    }",
            "}",
        };
        #endregion File



        private static string FormatLog(string text) => "DalilaDB: " + text;


        [MenuItem("Universal/DalilaDB/Create/Elements")]
        public static void ShowWindow()
        {

            // Create files
            CreateFileWithSaveFilePanelAndCustomExtension(DefaultFolderName, DefaultFileName, File, FormatLog, CustomExtension);

            // Compile
            AssetDatabase.Refresh();

        }

    }
}