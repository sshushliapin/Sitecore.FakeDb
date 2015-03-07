namespace Sitecore.FakeDb.Serialization
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Serialization.ObjectModel;
  using Sitecore.Diagnostics;

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
      Assert.IsTrue(".item".Equals(file.Extension, StringComparison.InvariantCultureIgnoreCase), string.Format("File '{0}' is not a .item file", file.FullName));

      var item = SyncItem.ReadItem(new Tokenizer(file.OpenText()));

      Assert.IsTrue(ID.IsID(item.ID), string.Format("Item id '{0}' is not a valid guid", item.ID));
      Assert.IsTrue(ID.IsID(item.TemplateID), string.Format("Item template id '{0}' is not a valid guid", item.TemplateID));

      return item;
    }

    /// <summary>
    /// Deserializes all .item files below the item's children folder and all deeper directories.
    /// Also traverses shortened paths.
    /// </summary>
    /// <param name="itemFile"></param>
    /// <param name="syncItem"></param>
    /// <param name="serializationFolder"></param>
    /// <param name="maxDepth"></param>
    /// <returns></returns>
    internal static List<SyncItem> DeserializeAll(this FileInfo itemFile, SyncItem syncItem, DirectoryInfo serializationFolder, int maxDepth)
    {
      if (maxDepth <= 0)
      {
        return new List<SyncItem>();
      }

      Assert.ArgumentNotNull(itemFile, "itemFile");

      var result = new List<SyncItem>();

      // Find descendants in direct subfolder
      if (itemFile.Directory != null)
      {
        var childItemsFolder = new DirectoryInfo(itemFile.Directory.FullName + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(itemFile.Name));
        if (childItemsFolder.Exists)
        {
          foreach (var childItemFile in childItemsFolder.GetFiles("*.item", SearchOption.AllDirectories))
          {
            var childSyncItem = childItemFile.Deserialize();
            result.AddRange(childItemFile.DeserializeAll(childSyncItem, serializationFolder, maxDepth - 1));
            result.Add(childSyncItem);
          }
        }
      }

      // Find descendants in shortened paths
      var linkFiles = ShortenedPathsDictionary.GetLocationsFromLinkFiles(serializationFolder);
      if (!linkFiles.ContainsKey(syncItem.ItemPath))
      {
        return result;
      }

      var truePath = new DirectoryInfo(linkFiles[syncItem.ItemPath]);
      if (!truePath.Exists)
      {
        return result;
      }

      foreach (var childItemFile in truePath.GetFiles("*.item", SearchOption.AllDirectories))
      {
        var childSyncItem = childItemFile.Deserialize();
        result.AddRange(childItemFile.DeserializeAll(childSyncItem, serializationFolder, maxDepth - 1));
        result.Add(childSyncItem);
      }

      return result;
    }

    /// <summary>
    /// Maps shared field values from a deserialized item to the FakeDb item.
    /// </summary>
    /// <param name="item">Deserialized item</param>
    /// <param name="dsDbItem">FakeDb item to copy values to</param>
    internal static void CopySharedFieldsTo(this SyncItem item, IDsDbItem dsDbItem)
    {
      foreach (var sharedField in item.SharedFields)
      {
        Assert.IsTrue(ID.IsID(sharedField.FieldID), string.Format("Shared field id '{0}' is not a valid guid", sharedField.FieldID));

        var field = dsDbItem.Fields.FirstOrDefault(f => f.ID.ToString() == sharedField.FieldID);
        if (field != null)
        {
          field.Value = sharedField.FieldValue;
        }
        else
        {
          dsDbItem.Add(new DbField(sharedField.FieldName, ID.Parse(sharedField.FieldID)) { Value = sharedField.FieldValue, Shared = true });
        }
      }
    }

    /// <summary>
    /// Maps versioned field values from a deserialized item to the FakeDb item.
    /// </summary>
    /// <param name="item">Deserialized item</param>
    /// <param name="dsDbItem">FakeDb item to copy values to</param>
    internal static void CopyVersionedFieldsTo(this SyncItem item, IDsDbItem dsDbItem)
    {
      var fields = new List<DbField>();
      foreach (var version in item.Versions)
      {
        foreach (var field in version.Fields)
        {
          Assert.IsTrue(ID.IsID(field.FieldID), string.Format("Field id '{0}' is not a valid guid", field.FieldID));
          var fieldId = ID.Parse(field.FieldID);
          var dbField = fields.FirstOrDefault(f => f.ID == fieldId);

          if (dbField == null)
          {
            dbField = new DbField(field.FieldName, fieldId) { Shared = false };
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
    /// </summary>
    /// <param name="path">A valid sitecore item path</param>
    /// <param name="serializationFolderName">Name of serialization folder as configured in app.config</param>
    /// <returns></returns>
    internal static FileInfo ResolveSerializationPath(string path, string serializationFolderName)
    {
      var serializationFolder = GetSerializationFolder(serializationFolderName);
      return ResolveSerializationPath(path, serializationFolder);
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
    /// <param name="serializationFolder">Folder in which serialized items are available</param>
    /// <returns></returns>
    internal static FileInfo ResolveSerializationPath(string path, DirectoryInfo serializationFolder)
    {
      var truePath = ShortenedPathsDictionary.FindTruePath(serializationFolder, path);

      var itemLocation =
        new FileInfo(
          string.Format(
            "{0}.item", 
            Path.Combine(
              serializationFolder.FullName.Trim(new[] { Path.DirectorySeparatorChar }), 
              truePath.Replace('/', Path.DirectorySeparatorChar).Trim(new[] { Path.DirectorySeparatorChar }))));

      Assert.IsTrue(
        itemLocation.Exists, 
        string.Format("Serialized item '{0}' could not be found in the path '{1}'; please check the path and if the item is serialized correctly", truePath, itemLocation.FullName));

      return itemLocation;
    }

    public static DirectoryInfo GetSerializationFolder(string serializationFolderName)
    {
      Assert.IsNotNullOrEmpty(serializationFolderName, "Please specify a serialization folder when you instantiate a FakeDb or individual DsDbItem/DsDbTemplate");

      var folderNode = Factory.GetConfigNode(string.Format("szfolders/folder[@name='{0}']", serializationFolderName));

      Assert.IsNotNull(
        folderNode, 
        string.Format(
          "Configuration for serialization folder name '{0}' could not be found; please check the <szfolders /> configuration in the app.config", 
          serializationFolderName));

      var serializationFolder = new DirectoryInfo(folderNode.Attributes["value"].Value);
      Assert.IsTrue(
        serializationFolder.Exists, 
        string.Format(
          "Path '{0}', as configured in the app.config could not be found; please check the <szfolders /> configuration in the app.config", 
          serializationFolder.FullName));
      return serializationFolder;
    }
  }
}