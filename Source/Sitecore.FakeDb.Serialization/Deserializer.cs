using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Serialization.ObjectModel;
using Sitecore.Diagnostics;

namespace Sitecore.FakeDb.Serialization
{
    /// <summary>
    /// Utility methods that help with deserializing data for use within unit tests.
    /// </summary>
    internal static class Deserializer
    {
        /// <summary>
        /// Deserializes an item file that was serialized with normal Sitecore serialization or TDS.
        /// </summary>
        /// <param name="file">.item file</param>
        /// <returns></returns>
        internal static SyncItem Deserialize(this FileInfo file)
        {
            Assert.ArgumentNotNull(file, "file");
            Assert.IsTrue(file.Exists, string.Format("File '{0}' can not be found or cannot be accessed", file.FullName));
            Assert.IsTrue(
                ".item".Equals(file.Extension, StringComparison.InvariantCultureIgnoreCase),
                string.Format("File '{0}' is not a .item file", file.FullName));

            SyncItem item = SyncItem.ReadItem(new Tokenizer(file.OpenText()));

            Assert.IsTrue(ID.IsID(item.ID), string.Format("Item id '{0}' is not a valid guid", item.ID));
            Assert.IsTrue(ID.IsID(item.TemplateID), string.Format("Item template id '{0}' is not a valid guid", item.TemplateID));

            return item;
        }

        /// <summary>
        /// The same as deserializes all .item files within a directory and all deeper directories.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        internal static List<SyncItem> DeserializeAll(this DirectoryInfo directory)
        {
            Assert.ArgumentNotNull(directory, "directory");
            return directory.GetFiles("*.item", SearchOption.AllDirectories).Select(i => i.Deserialize()).ToList();
        }

        /// <summary>
        /// Maps shared field values from a deserialized item to the FakeDb item.
        /// </summary>
        /// <param name="item">Deserialized item</param>
        /// <param name="dsDbItem">FakeDb item to copy values to</param>
        internal static void CopySharedFieldsTo(this SyncItem item, IDsDbItem dsDbItem)
        {
            foreach (SyncField sharedField in item.SharedFields)
            {
                Assert.IsTrue(ID.IsID(sharedField.FieldID), string.Format("Shared field id '{0}' is not a valid guid", sharedField.FieldID));
                dsDbItem.Add(new DbField(sharedField.FieldName, ID.Parse(sharedField.FieldID))
                {
                    Value = sharedField.FieldValue,
                    Shared = true
                });
            }
        }

        /// <summary>
        /// Maps versioned field values from a deserialized item to the FakeDb item.
        /// </summary>
        /// <param name="item">Deserialized item</param>
        /// <param name="dsDbItem">FakeDb item to copy values to</param>
        internal static void CopyVersionedFieldsTo(this SyncItem item, IDsDbItem dsDbItem)
        {
            List<DbField> fields = new List<DbField>();
            foreach (SyncVersion version in item.Versions)
            {
                foreach (SyncField field in version.Fields)
                {
                    Assert.IsTrue(ID.IsID(field.FieldID), string.Format("Field id '{0}' is not a valid guid", field.FieldID));
                    ID fieldId = ID.Parse(field.FieldID);
                    DbField dbField = fields.FirstOrDefault(f => f.ID == fieldId);

                    if (dbField == null)
                    {
                        dbField = new DbField(field.FieldName, fieldId)
                        {
                            Shared = false
                        };
                        dsDbItem.Add(dbField);
                        fields.Add(dbField);
                    }
                    int versionNumber;
                    if (int.TryParse(version.Version, out versionNumber))
                    {
                        dbField.Add(version.Language, versionNumber, field.FieldValue);
                    }
                    else
                    {
                        dbField.Add(version.Language, field.FieldValue);
                    }
                }
            }
        }

        /// <summary>
        /// Resolves a sitecore path to the filesystem where it is serialized.
        /// For example, path=/sitecore/content/item1 and serializationFolderName=master 
        /// Will be resolved to c:\site\data\serialization\master\sitecore\content\item1.item
        /// 
        /// You need to define the serialization folders in the app.config. For example:
        /// <szfolders>
        ///   <folder name="core" value="c:\site\data\serialization\core\" />
        ///   <folder name="master" value="c:\site\data\serialization\master\" />
        ///   <folder name="web" value="c:\site\data\serialization\web\" />
        /// </szfolders>
        /// </summary>
        /// <param name="path">A valid sitecore item path</param>
        /// <param name="serializationFolderName">Name of serialization folder as configured in app.config</param>
        /// <returns></returns>
        internal static FileInfo ResolveSerializationPath(string path, string serializationFolderName)
        {
            DirectoryInfo serializationFolder = GetSerializationFolder(serializationFolderName);

            FileInfo itemLocation = new FileInfo(
                string.Format(
                    "{0}.item",
                    Path.Combine(
                        serializationFolder.FullName.Trim(new[] { Path.DirectorySeparatorChar }),
                        path.Replace('/', Path.DirectorySeparatorChar).Trim(new[] { Path.DirectorySeparatorChar }))));
            
            Assert.IsTrue(itemLocation.Exists,
                string.Format("Serialized item '{0}' could not be found in the path '{1}'; please check the path and if the item is serialized correctly",
                path,
                itemLocation.FullName));

            return itemLocation;
        }

        public static DirectoryInfo GetSerializationFolder(string serializationFolderName)
        {
            Assert.IsNotNullOrEmpty(
                serializationFolderName,
                "Please specify a serialization folder when you instantiate a FakeDb or individual DsDbItem/DsDbTemplate");

            XmlNode folderNode = Factory.GetConfigNode(
                string.Format("szfolders/folder[@name='{0}']", serializationFolderName));

            Assert.IsNotNull(
                folderNode,
                string.Format(
                    "Configuration for serialization folder name '{0}' could not be found; please check the <szfolders /> configuration in the app.config",
                    serializationFolderName));

            DirectoryInfo serializationFolder = new DirectoryInfo(folderNode.Attributes["value"].Value);
            Assert.IsTrue(
                serializationFolder.Exists,
                string.Format(
                    "Path '{0}', as configured in the app.config could not be found; please check the <szfolders /> configuration in the app.config",
                    serializationFolder.FullName));
            return serializationFolder;
        }
    }
}
