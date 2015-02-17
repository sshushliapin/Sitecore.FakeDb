using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;

namespace Sitecore.FakeDb.Serialization
{
    public static class SerializedIdToPathResolver
    {
        private static readonly Dictionary<string, SerializedIdToPathSet> _pathSets
            = new Dictionary<string, SerializedIdToPathSet>();

        public static string FindFilePath(this ID id, string serializationFolderName)
        {
            lock (_pathSets)
            {
                // find or create the set of paths for the serialization folder
                SerializedIdToPathSet pathSet;
                if (_pathSets.ContainsKey(serializationFolderName))
                {
                    pathSet = _pathSets[serializationFolderName];
                }
                else
                {
                    pathSet = new SerializedIdToPathSet();
                    DirectoryInfo serializationFolder
                        = Deserializer.GetSerializationFolder(serializationFolderName);

                    // Add filepaths for shortened paths
                    foreach (string shortenedItemsFolder in ShortenedPathsDictionary
                        .GetLocationsFromLinkFiles(serializationFolder).Values)
                    {
                        pathSet.FilePaths.Push(shortenedItemsFolder);
                    }

                    // Add filepath for root of regular content
                    pathSet.FilePaths.Push(serializationFolder.FullName);
                    
                    _pathSets.Add(serializationFolderName, pathSet);
                }

                // get the individual item if already found
                if (pathSet.Paths.ContainsKey(id))
                {
                    return pathSet.Paths[id];
                }

                while (pathSet.FilePaths.Any())
                {
                    string filePath = pathSet.FilePaths.Pop();
                    foreach (string subdirectory in Directory.GetDirectories(filePath))
                    {
                        pathSet.FilePaths.Push(subdirectory);
                    }

                    string foundFile = null;
                    foreach (string file in Directory.GetFiles(filePath, "*.item"))
                    {
                        using (StreamReader sr = new StreamReader(file))
                        {
                            sr.ReadLine();
                            sr.ReadLine();
                            string itemIdStr = sr.ReadLine().Substring(4);
                            if (ID.IsID(itemIdStr))
                            {
                                ID itemId = ID.Parse(itemIdStr);
                                pathSet.Paths.Add(itemId, file);
                                if (itemId == id)
                                {
                                    foundFile = file;
                                }
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
