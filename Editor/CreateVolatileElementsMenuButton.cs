using UnityEditor;
using static U.DalilaDB.Editor.UE;

namespace U.Reactor.Editor
{
    public class CreateVolatileElementsMenuButton : EditorWindow
    {

        #region File
        private static string DefaultFolderName => "/Scripts/DalilaDocs/Models/";
        private static string DefaultFileName => "NewVEE";
        private static string CustomExtension => "velements";
        static string[] File(string fileName) => new string[]
        {
            "using U.DalilaDB;",
            "",
            "public static partial class DBmodels",
            "{",
            "    public class "+fileName+" : DalilaDBVolatileElements<"+fileName+"> { }",
            "}",
        };
        #endregion File



        private static string FormatLog(string text) => "DalilaDB: " + text;


        [MenuItem("Universal/DalilaDB/Create/Volatile Elements")]
        public static void ShowWindow()
        {

            // Create files
            CreateFileWithSaveFilePanelAndCustomExtension(DefaultFolderName, DefaultFileName, File, FormatLog, CustomExtension);

            // Compile
            AssetDatabase.Refresh();

        }

    }
}