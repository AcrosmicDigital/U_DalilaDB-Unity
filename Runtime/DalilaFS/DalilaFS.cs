using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

namespace U.DalilaDB
{
    internal partial class DalilaFS
    {
       
        protected string root_; // Root Path for the fileSystem

        public string _root { get => root_; private set => root_ = value; }  // Public Root Path

        /// <summary>
        /// Private constructor to create the fileSystem in a Path
        /// </summary>
        /// <param name="root">Root path</param>
        public DalilaFS(string root)
        {

            if (String.IsNullOrEmpty(root) || String.IsNullOrWhiteSpace(root))
                throw new DirectoryNotFoundException();

            // Set the root
            _root = root.Replace('\\', '/').TrimEnd('/', '\\') + "/";

            // Validate the root or try to create it
            Directory.CreateDirectory(_root);
            if (!Directory.Exists(_root))
                throw new DirectoryNotFoundException("UDalila: Cant find or create root directory");

            // Try to create a directory to check
            var rDir = _root + "FileSystemRandomDir907242394727984" + "/";
            Directory.CreateDirectory(rDir);

            // Try to create a file to check
            var rFile = rDir + "FileSystemRandomFile89723498237498";
            using (FileStream fs = File.Create(rFile))
            {
                byte[] info = new UTF8Encoding(true).GetBytes("Random text.");
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }

            // Open the file and read it back.
            using (StreamReader sr = File.OpenText(rFile))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }

            // Delete the directory and the file
            Directory.Delete(rDir, true);
        }

        /// <summary>
        /// Sets of characters for the names of locations ans resources
        /// </summary>
        public static char[] validCharacters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '_' };
        public static char[] validCharactersAndDot = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '_', '.' };
        public static char[] validCharactersAndDotAndSlash = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '_', '.', '/' };

        /// <summary>
        /// Check if a string is a valid resource name
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static bool IsValidResource(string resource)
        {

            // Cant be null or empty
            if (String.IsNullOrEmpty(resource))
                return false;

            // Must start with '/'
            if (!resource.StartsWith("/"))
                return false;

            // Cant end with '/'
            if (resource.EndsWith("/"))
                return false;

            // Cant end with '.'
            if (resource.EndsWith("."))
                return false;

            // Only can contain numbers, lowecase letters, and '/' and '.'
            if (resource.TrimEnd(DalilaFS.validCharactersAndDotAndSlash) != "")
                return false;

            // Cant have more than one '/' or '.' together
            for (int i = 0; i < resource.Length; i++)
            {
                if (i + 1 >= resource.Length)
                    break;

                if (resource[i] == resource[i + 1] && resource[i] == '/')
                    return false;

                if (resource[i] == resource[i + 1] && resource[i] == '.')
                    return false;

                if (resource[i] == '.' && resource[i + 1] == '/')
                    return false;
            }

            return true;

        }

        /// <summary>
        /// Check if a string is a valid location name
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static bool IsValidLocation(string location)
        {

            // Cant be null or empty
            if (String.IsNullOrEmpty(location))
                return false;

            // Must start with '/'
            if (!location.StartsWith("/"))
                return false;

            // Must end with '/'
            if (!location.EndsWith("/"))
                return false;

            // Only can contain numbers, lowecase letters, and '/' and '.'
            if (location.TrimEnd(DalilaFS.validCharactersAndDotAndSlash) != "")
                return false;

            // Cant have more than one '/' or '.' together or a './'
            for (int i = 0; i < location.Length; i++)
            {
                if (i + 1 >= location.Length)
                    break;

                if (location[i] == location[i + 1] && location[i] == '/')
                    return false;

                if (location[i] == location[i + 1] && location[i] == '.')
                    return false;

                if (location[i] == '.' && location[i + 1] == '/')
                    return false;
            }

            return true;

        }

        /// <summary>
        /// Transform a Altern Location in a complete Path
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public string LocationToSystemPath(string location)
        {

            if (!IsValidLocation(location))
                throw new FormatException("Invalid Location, Only can contain numbers, letters and '_', Must start and end with '/', Can contain '.', but cant end with it, Cant have more than one '.' or '/' together");

            return String.Concat(_root, location.TrimStart('/'));

        }

        /// <summary>
        /// Transform a Altern Resource in a complete Path
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public string ResourceToSystemPath(string resource)
        {
            if (!IsValidResource(resource))
                throw new FormatException("Invalid Resource, Only can contain numbers, letters and '_', Must start with '/', Can contain '.', but cant end with it or with '/', Cant have more than one '.' or '/' together");

            return String.Concat(_root, resource.TrimStart('/'));
        }

        /// <summary>
        /// Creates a altern location from a system path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string LocationFromSystemPath(string path)
        {
            // Check that the root is contained
            if(!path.Replace('\\', '/').StartsWith(_root))
                throw new FormatException("The path must start with Root location of the filesystem");

            // Get the location
            var location = ("/" + path.Remove(0, _root.Length).Replace('\\', '/').Replace(@"\+", " ")).TrimEnd('/') + "/";

            // Check if valid
            if (!IsValidLocation(location))
            {
                throw new FormatException("Invalid Location, Only can contain numbers, letters and '_', Must start and end with '/', Can contain '.', but cant end with it, Cant have more than one '.' or '/' together");
            }

            return location;
        }

        /// <summary>
        /// Create a altern resource from a system path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ResourceFromSystemPath(string path)
        {
            // Check that the root is contained and dont end with slash
            if (!path.Replace('\\', '/').StartsWith(_root) || path.EndsWith("/"))
                throw new FormatException("The path must start with Root location of the filesystem");

            // Get the resource
            var resource = "/" + path.Remove(0, _root.Length).Replace('\\', '/').Replace(@"\+", " ");

            // Check if valid
            if (!IsValidResource(resource))
                throw new FormatException("Invalid Resource, Only can contain numbers, letters and '_', Must start with '/', Can contain '.', but cant end with it or with '/', Cant have more than one '.' or '/' together");

            return resource;
        }

        /// <summary>
        /// Get the prev location from a location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static string GetPrevLocation(string location)
        {
            // Check if is valid location
            if (!IsValidLocation(location))
                throw new FormatException("Invalid Location, Only can contain numbers, letters and '_', Must start and end with '/', Can contain '.', but cant end with it, Cant have more than one '.' or '/' together");

            // If is root location
            if (location == "/")
                return location;

            // Trim the '/' and alphanumerics
            return location.TrimEnd('/').TrimEnd(validCharactersAndDot);

        }

        /// <summary>
        /// Get the location of a resource
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static string GetResourceLocation(string resource)
        {
            // Check if is valid resource
            if (!IsValidResource(resource))
                throw new FormatException("Invalid Resource, Only can contain numbers, letters and '_', Must start with '/', Can contain '.', but cant end with it or with '/', Cant have more than one '.' or '/' together");

            // Trim the resource
            var trim = resource.TrimEnd(validCharactersAndDot);

            return trim;

        }

        public static string GetOnlyResourceName(string resource, string extension)
        {
            try
            {
                var pr = resource.Split('/');
                var pos = pr.Length - 1;

                if (!pr[pos].EndsWith(extension)) return pr[pos];
                return pr[pos].Remove(pr[pos].Length-extension.Length);

            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GetOnlyResourceName(string resource)
        {
            return GetOnlyResourceName(resource, "");
        }


    }
}
