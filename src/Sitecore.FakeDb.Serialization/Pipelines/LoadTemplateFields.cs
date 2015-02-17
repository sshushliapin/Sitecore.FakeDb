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

            foreach (SyncItem descendantItem in template.File
                .DeserializeAll(
                    template.SyncItem,
                    Deserializer.GetSerializationFolder(template.SerializationFolderName),
                    3)
                .Where(i => ID.IsID(i.TemplateID) && ID.Parse(i.TemplateID) == TemplateIDs.TemplateField))
            {
                SyncField isSharedField = descendantItem.SharedFields.FirstOrDefault(f => "Shared".Equals(f.FieldName));
                SyncField typeField = descendantItem.SharedFields.FirstOrDefault(f => "Type".Equals(f.FieldName));

                bool isShared = isSharedField != null && "1".Equals(isSharedField.FieldValue);

                template.Fields.Add(new DbField(descendantItem.Name, ID.Parse(descendantItem.ID))
                {
                    Shared = isShared,
                    Type = typeField != null ? typeField.FieldValue : string.Empty
                });
            }
        }
    }
}
