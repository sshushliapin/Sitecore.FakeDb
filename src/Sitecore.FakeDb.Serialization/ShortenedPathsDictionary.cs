namespace Sitecore.FakeDb.Serialization
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;

  public static class ShortenedPathsDictionary
  {
    /// <summary>
    /// Maps serialization folders to a mapping of shortened paths to their real locations.
    /// </summary>
    private static readonly Dictionary<string, Dictionary<string, string>> ShortenedPaths = new Dictionary<string, Dictionary<string, string>>();

    public static string FindTruePath(DirectoryInfo serializationFolder, string path)
    {
      var folderPaths = GetLocationsFromLinkFiles(serializationFolder);

      var parentPath = path.TrimEnd(new[] { '/' });
      parentPath = parentPath.Substring(0, parentPath.LastIndexOf('/'));
      if (folderPaths.ContainsKey(parentPath))
      {
        return string.Format("/../{0}{1}", Path.GetFileName(folderPaths[parentPath]), path.Substring(parentPath.Length));
      }

      return path;
    }

    internal static Dictionary<string, string> GetLocationsFromLinkFiles(DirectoryInfo serializationFolder)
    {
      lock (ShortenedPaths)
      {
        if (ShortenedPaths.ContainsKey(serializationFolder.FullName))
        {
          return ShortenedPaths[serializationFolder.FullName];
        }

        // populate the shortened paths for this serialization folder
        var linkFiles = serializationFolder.Parent.GetDirectories().SelectMany(d => d.GetFiles("link"));

        var locationsFromLinkFiles = new Dictionary<string, string>();

        foreach (var linkFile in linkFiles)
        {
          var fullPath = File.ReadAllText(linkFile.FullName);
          var dbName = fullPath.Substring(0, fullPath.IndexOf('\\'));
          fullPath = fullPath.Substring(fullPath.IndexOf('\\')).Replace('\\', '/');
          if (dbName.Equals(serializationFolder.Name, StringComparison.InvariantCultureIgnoreCase))
          {
            locationsFromLinkFiles.Add(fullPath, linkFile.Directory.FullName);
          }
        }

        ShortenedPaths.Add(serializationFolder.FullName, locationsFromLinkFiles);

        return locationsFromLinkFiles;
      }
    }
  }
}