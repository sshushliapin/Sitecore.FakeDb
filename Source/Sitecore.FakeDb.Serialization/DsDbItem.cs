using System.IO;
using Sitecore.Data;
using Sitecore.Data.Serialization.ObjectModel;
using Sitecore.Diagnostics;

namespace Sitecore.FakeDb.Serialization
{
    /// <summary>
    /// FakeDb item that is deserialized from the file system (uses serialized data from Sitecore or TDS).
    /// </summary>
    public class DsDbItem : DbItem, IDsDbItem
    {
        public SyncItem SyncItem { get; private set; }
        public FileInfo File { get; private set; }
        public bool IncludeDescendants { get; private set; }

        public DsDbItem(string path, bool includeDescendants = false)
            : this(
                path,
                Context.Database != null ? Context.Database.Name : "master",
                includeDescendants)
        {
        }

        public DsDbItem(ID id, bool includeDescendants = false)
            : this(
                id,
                Context.Database != null ? Context.Database.Name : "master",
                includeDescendants)
        {
        }

        public DsDbItem(string path, string serializationFolderName, bool includeDescendants = false)
            : this(Deserializer.ResolveSerializationPath(path, serializationFolderName), includeDescendants)
        {
        }

        public DsDbItem(ID id, string serializationFolderName, bool includeDescendants = false)
            : this(new FileInfo(id.FindFilePath(serializationFolderName)), includeDescendants)
        {
        }

        public DsDbItem(FileInfo file, bool includeDescendants = false)
            : this(file.Deserialize(), file, includeDescendants)
        {
        }


        private DsDbItem(SyncItem syncItem, FileInfo file, bool includeDescendants)
            : base(syncItem.Name, ID.Parse(syncItem.ID), ID.Parse(syncItem.TemplateID))
        {
            this.SyncItem = syncItem;
            this.File = file;
            this.IncludeDescendants = includeDescendants;
        }
    }
}
