using System.IO;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Serialization.ObjectModel;
using Sitecore.Diagnostics;

namespace Sitecore.FakeDb.Serialization
{
    /// <summary>
    /// FakeDb template that is deserialized from the file system (uses serialized data from Sitecore or TDS).
    /// </summary>
    public class DsDbTemplate : DbTemplate, IDsDbItem
    {
        public DsDbTemplate(string path)
            : this(
                path,
                Context.Database != null ? Context.Database.Name : "master")
        {
        }

        public DsDbTemplate(ID id)
            : this(
                id,
                Context.Database != null ? Context.Database.Name : "master")
        {
        }

        public DsDbTemplate(string path, string serializationFolderName)
            : this(Deserializer.ResolveSerializationPath(path, serializationFolderName))
        {
        }

        public DsDbTemplate(ID id, string serializationFolderName)
            : this(new FileInfo(id.FindFilePath(serializationFolderName)))
        {
        }

        public DsDbTemplate(FileInfo file)
            : this(file.Deserialize(), file)
        {
        }

        private DsDbTemplate(SyncItem syncItem, FileInfo file)
            : base(syncItem.Name, ID.Parse(syncItem.ID))
        {
            Assert.IsTrue(syncItem.TemplateID == TemplateIDs.Template.ToString(),
                string.Format("File '{0}' is a correct item file, but does not represent a template; use DsDbItem instead to deserialize this", file.FullName));
            
            syncItem.CopySharedFieldsTo(this);

            syncItem.CopyVersionedFieldsTo(this);

            if (file.Directory != null)
            {
                DirectoryInfo childItemsFolder = new DirectoryInfo(
                    file.Directory.FullName + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(file.Name));
                if (childItemsFolder.Exists)
                {
                    foreach (SyncItem childItem in childItemsFolder
                        .DeserializeAll()
                        .Where(i => ID.IsID(i.TemplateID) && ID.Parse(i.TemplateID) == TemplateIDs.TemplateField))
                    {
                        SyncField isSharedField = childItem.SharedFields.FirstOrDefault(f => "Shared".Equals(f.FieldName));
                        SyncField typeField = childItem.SharedFields.FirstOrDefault(f => "Type".Equals(f.FieldName));

                        bool isShared = isSharedField != null && "1".Equals(isSharedField.FieldValue);

                        this.Fields.Add(new DbField(childItem.Name, ID.Parse(childItem.ID))
                        {
                            Shared = isShared,
                            Type = typeField != null ? typeField.FieldValue : string.Empty
                        });
                    }
                }
            }
        }
    }
}
