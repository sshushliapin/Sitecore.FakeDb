using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.FakeDb.Pipelines;

namespace Sitecore.FakeDb.Serialization.Pipelines
{
    public class DeserializeLinkedTemplate
    {
        public void Process(DsItemLoadingArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            DsDbItem dsDbItem = args.DsDbItem as DsDbItem;

            if (dsDbItem == null
                || ! dsDbItem.DeserializeLinkedTemplate
                || args.Db.GetItem(dsDbItem.TemplateID) != null)
            {
                return;
            }

            DeserializeTemplate(args.Db, dsDbItem.TemplateID, dsDbItem.SerializationFolderName);
        }

        private static void DeserializeTemplate(Db db, ID templateId, string serializationFolderName)
        {
            string filePath = templateId.FindFilePath(serializationFolderName);
            if (string.IsNullOrWhiteSpace(filePath)
                || ! File.Exists(filePath))
            {
                return;
            }

            DsDbTemplate dsDbTemplate = new DsDbTemplate(templateId, serializationFolderName);

            db.Add(dsDbTemplate);

            // Deserialize base templates
            DbField baseTemplatesField = dsDbTemplate.Fields
                                            .FirstOrDefault(f => f.ID == FieldIDs.BaseTemplate);
            if (! string.IsNullOrWhiteSpace(baseTemplatesField.Value))
            {
                foreach (ID baseTemplateId in baseTemplatesField.Value
                            .Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries)
                            .Where(ID.IsID)
                            .Select(ID.Parse))
                {
                    if (db.GetItem(baseTemplateId) == null)
                    {
                        DeserializeTemplate(db, baseTemplateId, dsDbTemplate.SerializationFolderName);
                    }
                }
            }
        }
    }
}
