using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            foreach (SyncVersion version in item.Versions)
            {
                foreach (SyncField field in version.Fields)
                {
                    Assert.IsTrue(ID.IsID(field.FieldID), string.Format("Field id '{0}' is not a valid guid", field.FieldID));
                    ID fieldId = ID.Parse(field.FieldID);
                    DbField dbField = dsDbItem.Fields.FirstOrDefault(f => f.ID == fieldId);

                    if (dbField == null)
                    {
                        dbField = new DbField(field.FieldName, fieldId)
                        {
                            Shared = false
                        };
                        dsDbItem.Add(dbField);
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
        /// For example, path=/sitecore/content/item1 and dbName=master 
        /// Will be resolved to c:\site\data\serialization\master\sitecore\content\item1.item
        /// A fallback is used if the database name is not used, for example:
        /// c:\site\data\serialization\sitecore\content\item1.item
        /// 
        /// You need to define a setting SerializationFolder in the app.config. For example:
        /// <setting name="SerializationFolder" value="c:\site\data\serialization\" />
        /// </summary>
        /// <param name="path">A valid sitecore item path</param>
        /// <param name="dbName">The name of the database</param>
        /// <returns></returns>
        internal static FileInfo ResolveSerializationPath(string path, string dbName)
        {
            DirectoryInfo serializationFolder = new DirectoryInfo(Sitecore.Configuration.Settings.GetSetting("SerializationFolder", "."));
            Assert.IsTrue(
                serializationFolder.Exists,
                string.Format("Path '{0}', as configured in the app.config could not be found; please check if the setting SerializationFolder is available and correctly set.", serializationFolder.FullName));

            FileInfo itemLocation = new FileInfo(
                string.Format(
                    "{0}.item",
                    Path.Combine(
                        serializationFolder.FullName,
                        dbName ?? string.Empty,
                        path.Replace('/', Path.DirectorySeparatorChar).Trim(new[] { Path.DirectorySeparatorChar }))));
            if (! itemLocation.Exists)
            {
                // fallback to path without database name
                itemLocation = new FileInfo(
                string.Format(
                    "{0}.item",
                    Path.Combine(
                        serializationFolder.FullName,
                        path.Replace('/', Path.DirectorySeparatorChar).Trim(new[] { Path.DirectorySeparatorChar }))));
            }

            Assert.IsTrue(itemLocation.Exists,
                string.Format("Serialized item '{0}' could not be found in the path '{1}' (and neither in the location with database {2} included); please check the path and if the item is serialized correctly",
                path,
                itemLocation.FullName,
                dbName));

            return itemLocation;
        }
    }
}
