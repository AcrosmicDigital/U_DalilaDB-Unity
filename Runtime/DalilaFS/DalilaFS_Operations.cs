using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using System.Data;

namespace U.DalilaDB
{
    public partial class DalilaFS
    {


    // InvalidOperationException
    // DuplicateNameException
    // DirectoryNotFoundException
    // FileNotFoundException

    /// <summary>
    /// Operation.Succesfull if a location exist
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public DataOperation ExistLocation(string location)
    {
        var operation = new DataOperation(); // Proccess
        string path; // full path

        // Validate the Name
        try
        {
            path = LocationToSystemPath(location);
        }
        catch (Exception e)
        {
            return operation.Fails(e);
        }

        // Check if folder exist
        try
        {
            if (Directory.Exists(path))
                return operation.Successful("1");
            else
                return operation.Fails(new DirectoryNotFoundException());
        }
        catch (Exception e)
        {
            return operation.Fails(e);
        }

    }

    /// <summary>
    /// Operation.Succesfull when a resource exist
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    public DataOperation ExistResource(string resource)
    {
        var operation = new DataOperation(); // Proccess
        string path; // full path

        // Validate the Name
        try
        {
            path = ResourceToSystemPath(resource);
        }
        catch (Exception e)
        {
            return operation.Fails(e);
        }

        // Check if resource exist
        try
        {
            if (File.Exists(path))
                return operation.Successful();
            else
                return operation.Fails(new FileNotFoundException());
        }
        catch (Exception e)
        {
            return operation.Fails(e);
        }

    }

    /// <summary>
    /// Create a location and all the sublocations and return true if is created or already exist
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public DataOperation CreateLocation(string location)
    {
        var operation = new DataOperation(); // Proccess
        string path; // full path

        // Validate the Name
        try
        {
            path = LocationToSystemPath(location);

        }
        catch (Exception e)
        {
            return operation.Fails(e);
        }


        // Check if a file exist with same name
        if (location != "/" && ExistResource(location.TrimEnd('/')))
            return operation.Fails(new DuplicateNameException("A file with same name already exist"));

        // If altern resource exist function will return true but int = 0 created
        if (location == "/" || ExistLocation(location))
            return operation.Successful("0");

        // Create the folder
        try
        {
            Directory.CreateDirectory(path);
            return operation.Successful("1");
        }
        catch (Exception e)
        {
            return operation.Fails(e);
        }


    }

    /// <summary>
    /// Return true if a directory is empty
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public DataOperation IsEmptyLocation(string location)
    {
        var operation = new DataOperation(); // Proccess
        string path; // full path

        // Validate the Name
        try
        {
            path = LocationToSystemPath(location);
        }
        catch (Exception e)
        {
            return operation.Fails(e);
        }

        // Check if empty
        try
        {
            // If dont exist
            if (!Directory.Exists(path))
                return operation.Fails(new DirectoryNotFoundException());

            var files = Directory.EnumerateFileSystemEntries(path);

            if (files.Count() == 0)
                return operation.Successful();
            else
                return operation.Fails(new FileNotFoundException("Any files in directory"));
        }
        catch (Exception e)
        {
            return operation.Fails(e);
        }

    }

    /// <summary>
    /// Delete a location and all sublocations empty or not and return true if deleted or not exist
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public DataOperation DeleteLocation(string location)
    {
        var operation = new DataOperation(); // Proccess
        string path; // full path

        // Validate the Name
        try
        {
            path = LocationToSystemPath(location);
        }
        catch (Exception e)
        {
            return operation.Fails(e);
        }

        // Check if a file exist with same name
        if (ExistResource(location.TrimEnd('/')))
            return operation.Fails(new DuplicateNameException("A file with same name already exist"));

        // Return true, but 0 deleted
        if (!ExistLocation(location))
            return operation.Successful("0");

        // Delete the folder if ExistsLocation
        try
        {
            // If is the root location
            if (path == _root)
                return operation.Fails( new InvalidOperationException("Cant delete Root location"));

            if (Directory.Exists(path))
                Directory.Delete(path, true);

            return operation.Successful("1");
        }
        catch (Exception e)
        {
            return operation.Fails(e);
        }


    }

    /// <summary>
    /// Delete a resource or file and return 0 or 1 if deleted or not
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    public DataOperation DeleteResource(string resource)
    {
        var operation = new DataOperation(); // Proccess
        string path; // full path

        // Validate the Name
        try
        {
            path = ResourceToSystemPath(resource);
        }
        catch (Exception e)
        {
            return operation.Fails(e);
        }

        // Check if a location exist with same name
        if (ExistLocation(resource + "/"))
            return operation.Fails(new DuplicateNameException("A file with same name already exist"));

        // Check if resource Exists
        if (!ExistResource(resource))
            return operation.Successful("0");

        // Delete the resource if Exists
        try
        {

            if (File.Exists(path))
                File.Delete(path);

            return operation.Successful("1");
        }
        catch (Exception e)
        {
            return operation.Fails(e);
        }


    }

    /// <summary>
    /// Delete a location only if is empty and return true if deleted or not exist
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public DataOperation DeleteEmptyLocation(string location)
    {
        var operation = new DataOperation(); // Proccess
        string path; // full path

        // Validate the Name
        try
        {
            path = LocationToSystemPath(location);
        }
        catch (Exception e)
        {
            return operation.Fails(e);
        }

        // If ExistsLocation
        if (ExistLocation(location))
        {
            if (IsEmptyLocation(location))
            {
                return DeleteLocation(location);
            }
            else
            {
                return operation.Fails(new InvalidOperationException("Directory is not empty"));
            }
        }
        else
        {
            // Check if a file exist with same name
            if (ExistResource(location.TrimEnd('/')))
                return operation.Fails(new DuplicateNameException("A file with same name already exist"));

            return operation.Successful();
        }


    }

    /// <summary>
    /// Delete all the locations and prevlocations in a location string that are empty
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public DataOperation DeleteEmptyLocations(string location)
    {

        var operation = new DataOperation(); // Proccess

        // Validate the Name
        try
        {
            LocationToSystemPath(location);
        }
        catch (Exception e)
        {
            return operation.Fails(e);
        }

        // Delete the empty folders
        var s = 0;
        var toDeleteLocation = location;
        while (s < 50 && location != "/")
        {

            // Check if the from folder is now empty
            var deleteoperation = DeleteEmptyLocation(toDeleteLocation);
            if (!deleteoperation)
                break;

            toDeleteLocation = GetPrevLocation(toDeleteLocation);

        }

        return operation.Successful();
    }

    /// <summary>
    /// Rename a location and delete the locationFrom if empty or move it
    /// </summary>
    /// <param name="locationFrom"></param>
    /// <param name="locationTo"></param>
    /// <returns></returns>
    public DataOperation RenameLocation(string locationFrom, string locationTo)
    {

        var operation = new DataOperation(); // Proccess
        string pathFrom; // full path
        string pathTo; // full path

        // Validate the Name
        try
        {
            pathFrom = LocationToSystemPath(locationFrom);
            pathTo = LocationToSystemPath(locationTo);
        }
        catch (Exception e)
        {
            return operation.Fails(e);
        }

        // If source dont ExistsLocation return
        if (!ExistLocation(locationFrom))
            return operation.Fails(new DirectoryNotFoundException());

        // If destinacion folder already exist or there are a resource with same name
        if (ExistLocation(locationTo) || ExistResource(locationTo.TrimEnd('/')))
        {
            return operation.Fails(new DuplicateNameException("A location or Resource with the same name, already exist"));
        }

        // Check if previus destination ExistsLocation or create it
        var prevLocationTo = GetPrevLocation(locationTo);
        if (!ExistLocation(prevLocationTo))
        {
            var createOperation = CreateLocation(prevLocationTo);
            if (!createOperation)
                return createOperation;
        }


        // Move the folder
        try
        {
            Directory.Move(pathFrom, pathTo);
        }
        catch (Exception e)
        {
            return operation.Fails(e);
        }

        // Delete the empty folders
        DeleteEmptyLocations(GetPrevLocation(locationFrom));

        return operation.Successful("1");

    }

    /// <summary>
    /// Rename a resource and or move it
    /// </summary>
    /// <param name="resourceFrom"></param>
    /// <param name="resourceTo"></param>
    /// <returns></returns>
    public DataOperation RenameResource(string resourceFrom, string resourceTo)
    {

        var operation = new DataOperation(); // Proccess
        string pathFrom; // full path
        string pathTo; // full path

        // Validate the Name
        try
        {
            pathFrom = ResourceToSystemPath(resourceFrom);
            pathTo = ResourceToSystemPath(resourceTo);
        }
        catch (Exception e)
        {
            return operation.Fails(e);
        }

        // If source dont ExistsLocation return
        if (!ExistResource(resourceFrom))
            return operation.Fails(new FileNotFoundException());

        // If destinacion folder already exist or there are a location with same name
        if (ExistLocation(resourceTo + "/") || ExistResource(resourceTo))
        {
            return operation.Fails(new DuplicateNameException("A location or Resource with the same name, already exist"));
        }


        // Check if destiny location Exists or create it
        var prevLocationTo = GetResourceLocation(resourceTo);
        if (!ExistLocation(prevLocationTo))
        {
            var createOperation = CreateLocation(prevLocationTo);
            if (!createOperation)
                return createOperation;
        }


        // Move the file
        try
        {
            File.Move(pathFrom, pathTo);
        }
        catch (Exception e)
        {
            return operation.Fails(e);
        }

        return operation.Successful("1");

    }

    /// <summary>
    /// Get all the sublocations in a location with a string pattern
    /// </summary>
    /// <param name="location"></param>
    /// <param name="searchPattern"></param>
    /// <returns></returns>
    public string[] GetLocations(string location, string searchPattern = "*")
    {
        string path; // full path

        // Validate the Name
        try
        {
            path = LocationToSystemPath(location);
        }
        catch (Exception)
        {
            return new string[0];
        }

        // Get locations
        try
        {
            return Directory.EnumerateDirectories(path, searchPattern, SearchOption.AllDirectories).Select(l => LocationFromSystemPath(l)).ToArray();
        }
        catch (Exception)
        {
            return new string[0];
        }

    }

    /// <summary>
    /// Get all the resources in a location with a string pattern
    /// </summary>
    /// <param name="location"></param>
    /// <param name="searchPattern"></param>
    /// <returns></returns>
    public string[] GetResources(string location, string searchPattern = "*")
    {

        string path; // full path
                     // Validate the Name
        try
        {
            path = LocationToSystemPath(location);
        }
        catch (Exception)
        {
            return new string[0];
        }

        // Get locations
        try
        {
            return Directory.EnumerateFiles(path, searchPattern, SearchOption.AllDirectories).Select(l => ResourceFromSystemPath(l)).ToArray();
        }
        catch (Exception)
        {
            return new string[0];
        }

    }

    /// <summary>
    /// Get a key value pair with the locations and the resources.
    /// </summary>
    /// <param name="location"></param>
    /// <param name="searchPattern"></param>
    /// <returns></returns>
    public KeyValuePair<string, string[]>[] GetFileSystem(string location, string searchPattern = "*")
    {

        string path; // full path

        // Validate the Name
        try
        {
            path = LocationToSystemPath(location);
        }
        catch (Exception)
        {
            return new KeyValuePair<string, string[]>[0];
        }

        return new KeyValuePair<string, string[]>[]
        {
                new KeyValuePair<string, string[]>("locations", GetLocations(location, searchPattern)),
                new KeyValuePair<string, string[]>("resources", GetResources(location, searchPattern)),
        };

    }

    /// <summary>
    /// Copy a resource
    /// </summary>
    /// <param name="resourceFrom"></param>
    /// <param name="resourceTo"></param>
    /// <returns></returns>
    public DataOperation CopyResource(string resourceFrom, string resourceTo)
    {

        var operation = new DataOperation(); // Proccess
        string pathFrom; // full path
        string pathTo; // full path

        // Validate the Name
        try
        {
            pathFrom = ResourceToSystemPath(resourceFrom);
            pathTo = ResourceToSystemPath(resourceTo);
        }
        catch (Exception e)
        {
            return operation.Fails(e);
        }

        // If source dont ExistsLocation return
        if (!ExistResource(resourceFrom))
            return operation.Fails(new FileNotFoundException());

        // If destinacion folder already exist or there are a resource with same name
        if (ExistLocation(resourceTo + "/") || ExistResource(resourceTo))
            return operation.Fails(new DuplicateNameException("A location or Resource with the same name, already exist"));

        // Check if destiny location Exists or create it
        var prevLocationTo = GetResourceLocation(resourceTo);
        if (!ExistLocation(prevLocationTo))
        {
            var createOperation = CreateLocation(prevLocationTo);
            if (!createOperation)
                return createOperation;
        }


        // Copy the file
        try
        {
            File.Copy(pathFrom, pathTo);
            return operation.Successful("1");
        }
        catch (Exception e)
        {
            return operation.Fails(e);
        }

    }

    /// <summary>
    /// Copy a location and all the sublocations and subresources
    /// </summary>
    /// <param name="locationFrom"></param>
    /// <param name="locationTo"></param>
    /// <returns></returns>
    public DataOperation CopyLocation(string locationFrom, string locationTo)
    {
        var operation = new DataOperation(); // Proccess
        string pathFrom; // full path
        string pathTo; // full path
        int copied = 0;

        // Validate the Name
        try
        {
            pathFrom = LocationToSystemPath(locationFrom);
            pathTo = LocationToSystemPath(locationTo);
        }
        catch (Exception e)
        {
            return operation.Fails(e);
        }

        // If source dont ExistsLocation return
        if (!ExistLocation(locationFrom))
            return operation.Fails(new DirectoryNotFoundException());

        // If destinacion folder already exist or there are a resource with same name
        if (ExistLocation(locationTo) || ExistResource(locationTo.TrimEnd('/')))
        {
            return operation.Fails(new DuplicateNameException("A location or Resource with the same name, already exist"));
        }

        // Check if previus destination ExistsLocation or create it
        var prevLocationTo = GetPrevLocation(locationTo);
        if (!ExistLocation(prevLocationTo))
        {
            var createOperation = CreateLocation(prevLocationTo);
            if (!createOperation)
                return createOperation;
        }

        // Get the subdirectories for the specified location end create them in destiny
        foreach (var location in GetLocations(locationFrom))
        {
            if (CreateLocation(locationTo + location.Remove(0, locationFrom.Length)))
                copied++;
        }

        // Get the files in the directory and copy them to the new location.
        foreach (var resource in GetResources(locationFrom))
        {
            if (CopyResource(resource, locationTo + resource.Remove(0, locationFrom.Length)))
                copied++;
        }

        return operation.Successful(copied + "");

    }

}

}
