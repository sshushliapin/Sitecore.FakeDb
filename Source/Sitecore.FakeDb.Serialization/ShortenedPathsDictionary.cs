using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.FakeDb.Serialization
{
    public static class ShortenedPathsDictionary
    {
        /// <summary>
        /// Maps serialization folders to a mapping of shortened paths to their real locations.
        /// </summary>
        private static readonly Dictionary<string, Dictionary<string, string>> _shortenedPaths
            = new Dictionary<string, Dictionary<string, string>>();

        public static string FindTruePath(DirectoryInfo serializationFolder, string path)
        {
            lock (_shortenedPaths)
            {
                Dictionary<string, string> folderPaths;
                if (_shortenedPaths.ContainsKey(serializationFolder.FullName))
                {
                    folderPaths = _shortenedPaths[serializationFolder.FullName];
                }
                else
                {
                    folderPaths = new Dictionary<string, string>();

                    // populate the shortened paths for this serialization folder
                    var locationsFromLinkFiles = GetLocationsFromLinkFiles(serializationFolder);

                    foreach (var fromLinkFile in locationsFromLinkFiles)
                    {
                        folderPaths.Add(fromLinkFile.Key, fromLinkFile.Value);
                    }

                    _shortenedPaths.Add(serializationFolder.FullName, folderPaths);
                }

                string parentPath = path.TrimEnd(new [] { '/' });
                parentPath = parentPath.Substring(0, parentPath.LastIndexOf('/'));
                if (folderPaths.ContainsKey(parentPath))
                {
                    return string.Format("/../{0}{1}", Path.GetFileName(folderPaths[parentPath]), path.Substring(parentPath.Length));
                }
                return path;
            }
        }

        internal static Dictionary<string, string> GetLocationsFromLinkFiles(DirectoryInfo serializationFolder)
        {
            var linkFiles = serializationFolder.Parent.GetDirectories()
                                               .SelectMany(d => d.GetFiles("link"));

            Dictionary<string, string> locationsFromLinkFiles = new Dictionary<string, string>();

            foreach (FileInfo linkFile in linkFiles)
            {
                string fullPath = File.ReadAllText(linkFile.FullName);
                string dbName = fullPath.Substring(0, fullPath.IndexOf('\\'));
                fullPath = fullPath.Substring(fullPath.IndexOf('\\')).Replace('\\', '/');
                if (dbName.Equals(serializationFolder.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    locationsFromLinkFiles.Add(fullPath, linkFile.Directory.FullName);
                }
            }
            return locationsFromLinkFiles;
        }
    }
}