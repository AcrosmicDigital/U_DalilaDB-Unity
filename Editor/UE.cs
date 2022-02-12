using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace U.DalilaDB.Editor
{
    internal static class UE
    {
        public static void CreateFile(string folderName, string fileName, string[] file, Func<string, string> FormatLog)
        {
            // Check if Env folder exist or create it
            if (!Directory.Exists(Application.dataPath + folderName))
            {
                Debug.Log(FormatLog("Creating Assets" + folderName + " directory"));
                Directory.CreateDirectory(Application.dataPath + folderName);
            }
            else
            {
                Debug.Log(FormatLog("Assets" + folderName + " already exist"));
            }

            // check if scenes file exist or create it
            if (!File.Exists(Application.dataPath + folderName + fileName))
            {
                Debug.Log(FormatLog("Creating Assets" + folderName + fileName + " file"));

                // Write the file
                File.WriteAllLines(Application.dataPath + folderName + fileName, file); // This should be async when is available

            }
            else
            {
                Debug.LogError(FormatLog("Assets" + folderName + fileName + " already exist"));
            }

        }

        public static void CreateFileWithSaveFilePanelForceLocation(string folderName, string defaultFileName, Func<string, string[]> file, Func<string, string> FormatLog)
        {

            // Check if Env folder exist or create it
            if (!Directory.Exists(Application.dataPath + folderName))
            {
                Debug.Log(FormatLog("Creating Assets" + folderName + " directory"));
                Directory.CreateDirectory(Application.dataPath + folderName);
            }
            else
            {
                Debug.Log(FormatLog("Assets" + folderName + " already exist"));
            }

            // Read the File Name from the save file Panel
            var path = EditorUtility.SaveFilePanel(title: "Create " + defaultFileName, directory: Application.dataPath + folderName, defaultName: defaultFileName, extension: "cs");

            // Get only the filename
            var fileName = Path.GetFileName(path);
            var fileNameNoExtension = Path.GetFileNameWithoutExtension(path);

            // check if scenes file exist or create it
            if (!File.Exists(Application.dataPath + folderName + fileName))
            {
                Debug.Log(FormatLog("Creating Assets" + folderName + fileName + " file"));

                // Write the file
                File.WriteAllLines(Application.dataPath + folderName + fileName, file(fileNameNoExtension)); // This should be async when is available

            }
            else
            {
                Debug.LogError(FormatLog("Assets" + folderName + fileName + " already exist"));
            }

        }

        public static void CreateFileWithSaveFilePanel(string defaultFolderName, string defaultFileName, Func<string, string[]> file, Func<string, string> FormatLog)
        {
            // Check if Env folder exist or create it
            if (!Directory.Exists(Application.dataPath + defaultFolderName))
            {
                Debug.Log(FormatLog("Creating Assets" + defaultFolderName + " directory"));
                Directory.CreateDirectory(Application.dataPath + defaultFolderName);
            }
            else
            {
                Debug.Log(FormatLog("Assets" + defaultFolderName + " already exist"));
            }

            // Read the File Name from the save file Panel
            var path = EditorUtility.SaveFilePanel(title: "Create " + defaultFileName, directory: Application.dataPath + defaultFolderName, defaultName: defaultFileName, extension: "cs");

            // check if scenes file exist or create it
            if (!File.Exists(path))
            {
                Debug.Log(FormatLog("Creating " + path + " file"));

                // Write the file
                File.WriteAllLines(path, file(Path.GetFileNameWithoutExtension(path))); // This should be async when is available

            }
            else
            {
                Debug.LogError(FormatLog(path + " already exist"));
            }

        }

        public static void CreateFileWithSaveFilePanelAndCustomExtension(string defaultFolderName, string defaultFileName, Func<string, string[]> file, Func<string, string> FormatLog, string extension)
        {
            // Check if Env folder exist or create it
            if (!Directory.Exists(Application.dataPath + defaultFolderName))
            {
                Debug.Log(FormatLog("Creating Assets" + defaultFolderName + " directory"));
                Directory.CreateDirectory(Application.dataPath + defaultFolderName);
            }
            else
            {
                Debug.Log(FormatLog("Assets" + defaultFolderName + " already exist"));
            }

            // Read the File Name from the save file Panel
            var path = EditorUtility.SaveFilePanel(title: "Create " + defaultFileName, directory: Application.dataPath + defaultFolderName, defaultName: defaultFileName, extension: "cs");

            // Get only the filename
            var fileNameNoExtension = Path.GetFileNameWithoutExtension(path);
            var fileDirectory = Path.GetDirectoryName(path);

            var FilePath = fileDirectory + "/" + fileNameNoExtension + "." + extension + ".cs";

            // Create Files
            if (!File.Exists(FilePath))
            {
                Debug.Log(FormatLog("Creating " + FilePath + " file"));

                // Write the files
                File.WriteAllLines(FilePath, file(fileNameNoExtension)); // This should be async when is available

            }
            else
            {
                Debug.LogError(FormatLog(FilePath + " already exist"));
            }

        }

        public static void CreateViewFiles(string defaultFolderName, string defaultFileName, Func<string, string[]> fileRender, Func<string, string[]> fileScripts, Func<string, string> FormatLog)
        {
            // Check if Env folder exist or create it
            if (!Directory.Exists(Application.dataPath + defaultFolderName))
            {
                Debug.Log(FormatLog("Creating Assets" + defaultFolderName + " directory"));
                Directory.CreateDirectory(Application.dataPath + defaultFolderName);
            }
            else
            {
                Debug.Log(FormatLog("Assets" + defaultFolderName + " already exist"));
            }

            // Read the File Name from the save file Panel
            var path = EditorUtility.SaveFilePanel(title: "Create " + defaultFileName, directory: Application.dataPath + defaultFolderName, defaultName: defaultFileName, extension: "cs");

            // Get only the filename
            var fileNameNoExtension = Path.GetFileNameWithoutExtension(path);
            var fileDirectory = Path.GetDirectoryName(path);

            var renderFilePath = fileDirectory + "/" + fileNameNoExtension + ".render.cs";
            var scriptsFilePath = fileDirectory + "/" + fileNameNoExtension + ".scripts.cs";

            // Create Files
            if (!File.Exists(renderFilePath) && !File.Exists(scriptsFilePath))
            {
                Debug.Log(FormatLog("Creating " + renderFilePath + " and " + scriptsFilePath + " files"));

                // Write the files
                File.WriteAllLines(renderFilePath, fileRender(fileNameNoExtension)); // This should be async when is available
                File.WriteAllLines(scriptsFilePath, fileScripts(fileNameNoExtension)); // This should be async when is available

            }
            else
            {
                Debug.LogError(FormatLog(renderFilePath + " or " + scriptsFilePath + " already exist"));
            }

        }

        public const string quote = "\"";

    }
}