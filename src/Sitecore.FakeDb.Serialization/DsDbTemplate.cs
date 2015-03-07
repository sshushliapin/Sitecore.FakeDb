namespace Sitecore.FakeDb.Serialization
{
  using System.IO;
  using Sitecore.Data;
  using Sitecore.Data.Serialization.ObjectModel;
  using Sitecore.Diagnostics;

  /// <summary>
  /// FakeDb template that is deserialized from the file system (uses serialized data from Sitecore or TDS).
  /// </summary>
  public class DsDbTemplate : DbTemplate, IDsDbItem
  {
    public string SerializationFolderName { get; private set; }

    public SyncItem SyncItem { get; private set; }

    public FileInfo File { get; private set; }

    public DsDbTemplate(string path)
      : this(path, Context.Database != null ? Context.Database.Name : "master")
    {
    }

    public DsDbTemplate(ID id)
      : this(id, Context.Database != null ? Context.Database.Name : "master")
    {
    }

    public DsDbTemplate(string path, string serializationFolderName)
      : this(Deserializer.ResolveSerializationPath(path, serializationFolderName), serializationFolderName)
    {
    }

    public DsDbTemplate(ID id, string serializationFolderName)
      : this(new FileInfo(id.FindFilePath(serializationFolderName)), serializationFolderName)
    {
    }

    private DsDbTemplate(FileInfo file, string serializationFolderName)
      : this(serializationFolderName, file.Deserialize(), file)
    {
    }

    private DsDbTemplate(string serializationFolderName, SyncItem syncItem, FileInfo file)
      : base(syncItem.Name, ID.Parse(syncItem.ID))
    {
      Assert.IsTrue(
        syncItem.TemplateID == TemplateIDs.Template.ToString(), 
        string.Format("File '{0}' is a correct item file, but does not represent a template; use DsDbItem instead to deserialize this", file.FullName));

      this.SerializationFolderName = serializationFolderName;
      this.SyncItem = syncItem;
      this.File = file;
    }
  }
}