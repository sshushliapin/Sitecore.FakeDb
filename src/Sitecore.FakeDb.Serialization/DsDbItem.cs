namespace Sitecore.FakeDb.Serialization
{
  using System.IO;
  using Sitecore.Data;
  using Sitecore.Data.Serialization.ObjectModel;

  /// <summary>
  /// FakeDb item that is deserialized from the file system (uses serialized data from Sitecore or TDS).
  /// </summary>
  public class DsDbItem : DbItem, IDsDbItem
  {
    public string SerializationFolderName { get; private set; }

    public SyncItem SyncItem { get; private set; }

    public FileInfo File { get; private set; }

    public bool IncludeDescendants { get; private set; }

    public bool DeserializeLinkedTemplate { get; private set; }

    public DsDbItem(string path, bool includeDescendants = false, bool deserializeLinkedTemplate = true)
      : this(path, Context.Database != null ? Context.Database.Name : "master", includeDescendants, deserializeLinkedTemplate)
    {
    }

    public DsDbItem(ID id, bool includeDescendants = false, bool deserializeLinkedTemplate = true)
      : this(id, Context.Database != null ? Context.Database.Name : "master", includeDescendants, deserializeLinkedTemplate)
    {
    }

    public DsDbItem(string path, string serializationFolderName, bool includeDescendants = false, bool deserializeLinkedTemplate = true)
      : this(serializationFolderName, Deserializer.ResolveSerializationPath(path, serializationFolderName), includeDescendants, deserializeLinkedTemplate)
    {
    }

    public DsDbItem(ID id, string serializationFolderName, bool includeDescendants = false, bool deserializeLinkedTemplate = true)
      : this(serializationFolderName, new FileInfo(id.FindFilePath(serializationFolderName)), includeDescendants, deserializeLinkedTemplate)
    {
    }

    internal DsDbItem(string serializationFolderName, FileInfo file, bool includeDescendants = false, bool deserializeLinkedTemplate = true)
      : this(serializationFolderName, file.Deserialize(), file, includeDescendants, deserializeLinkedTemplate)
    {
    }

    private DsDbItem(string serializationFolderName, SyncItem syncItem, FileInfo file, bool includeDescendants, bool deserializeLinkedTemplate = true)
      : base(syncItem.Name, ID.Parse(syncItem.ID), ID.Parse(syncItem.TemplateID))
    {
      this.SerializationFolderName = serializationFolderName;
      this.SyncItem = syncItem;
      this.File = file;
      this.IncludeDescendants = includeDescendants;
      this.DeserializeLinkedTemplate = deserializeLinkedTemplate;
    }
  }
}