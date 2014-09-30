using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;
using Sitecore.Data.Serialization.ObjectModel;
using Sitecore.Diagnostics;
using Sitecore.FakeDb.Pipelines;

namespace Sitecore.FakeDb.Serialization.Pipelines
{
    public class LoadTemplateFields
    {
        public void Process(DsItemLoadingArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            DsDbTemplate template = args.DsDbItem as DsDbTemplate;
            Assert.ArgumentNotNull(template, "Item was not a DsDbTemplate, which is required here");

            if (template.File.Directory != null)
            {
                DirectoryInfo childItemsFolder = new DirectoryInfo(
                    template.File.Directory.FullName
                    + Path.DirectorySeparatorChar
                    + Path.GetFileNameWithoutExtension(template.File.Name));
                if (childItemsFolder.Exists)
                {
                    foreach (SyncItem childItem in childItemsFolder
                        .DeserializeAll()
                        .Where(i => ID.IsID(i.TemplateID) && ID.Parse(i.TemplateID) == TemplateIDs.TemplateField))
                    {
                        SyncField isSharedField = childItem.SharedFields.FirstOrDefault(f => "Shared".Equals(f.FieldName));
                        SyncField typeField = childItem.SharedFields.FirstOrDefault(f => "Type".Equals(f.FieldName));

                        bool isShared = isSharedField != null && "1".Equals(isSharedField.FieldValue);

                        template.Fields.Add(new DbField(childItem.Name, ID.Parse(childItem.ID))
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
