using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            string filePath = dsDbItem.TemplateID.FindFilePath(dsDbItem.SerializationFolderName);
            if (string.IsNullOrWhiteSpace(filePath)
                || ! File.Exists(filePath))
            {
                return;
            }

            args.Db.Add(new DsDbTemplate(dsDbItem.TemplateID, dsDbItem.SerializationFolderName));
        }
    }
}
