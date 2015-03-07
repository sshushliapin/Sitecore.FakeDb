namespace Sitecore.FakeDb.Serialization
{
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using Sitecore.Data;

  public static class SerializedIdToPathResolver
  {
    private static readonly Dictionary<string, SerializedIdToPathSet> PathSets = new Dictionary<string, SerializedIdToPathSet>();

    public static string FindFilePath(this ID id, string serializationFolderName)
    {
      lock (PathSets)
      {
        // find or create the set of paths for the serialization folder
        SerializedIdToPathSet pathSet;
        if (PathSets.ContainsKey(serializationFolderName))
        {
          pathSet = PathSets[serializationFolderName];
        }
        else
        {
          pathSet = new SerializedIdToPathSet();
          var serializationFolder = Deserializer.GetSerializationFolder(serializationFolderName);

          // Add filepaths for shortened paths
          foreach (
            var shortenedItemsFolder in ShortenedPathsDictionary.GetLocationsFromLinkFiles(serializationFolder).Values)
          {
            pathSet.FilePaths.Push(shortenedItemsFolder);
          }

          // Add filepath for root of regular content
          pathSet.FilePaths.Push(serializationFolder.FullName);

          PathSets.Add(serializationFolderName, pathSet);
        }

        // get the individual item if already found
        if (pathSet.Paths.ContainsKey(id))
        {
          return pathSet.Paths[id];
        }

        while (pathSet.FilePaths.Any())
        {
          var filePath = pathSet.FilePaths.Pop();
          foreach (var subdirectory in Directory.GetDirectories(filePath))
          {
            pathSet.FilePaths.Push(subdirectory);
          }

          string foundFile = null;
          foreach (var file in Directory.GetFiles(filePath, "*.item"))
          {
            using (var sr = new StreamReader(file))
            {
              sr.ReadLine();
              sr.ReadLine();
              var itemIdStr = sr.ReadLine().Substring(4);
              if (!ID.IsID(itemIdStr))
              {
                continue;
              }

              var itemId = ID.Parse(itemIdStr);
              pathSet.Paths.Add(itemId, file);
              if (itemId == id)
              {
                foundFile = file;
              }
            }
          }

          if (foundFile != null)
          {
            return foundFile;
          }
        }

        return null;
      }
    }
  }
}