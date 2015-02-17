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
    public class DeserializeDescendants
    {
        public void Process(DsItemLoadingArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            
            DsDbItem dsDbItem = args.DsDbItem as DsDbItem;

            if (dsDbItem == null)
            {
                return;
            }

            // Deserialize and link descendants, if needed
            FileInfo file = args.DsDbItem.File;
            if (dsDbItem.IncludeDescendants && file.Directory != null)
            {
                DirectoryInfo childItemsFolder = new DirectoryInfo(
                    file.Directory.FullName + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(file.Name));
                if (childItemsFolder.Exists)
                {
                    foreach (var itemFile in childItemsFolder.GetFiles("*.item", SearchOption.TopDirectoryOnly))
                    {
                        DsDbItem childItem = new DsDbItem(dsDbItem.SerializationFolderName, itemFile, true);
                        dsDbItem.Children.Add(childItem);
                    }
                }
            }
        }
    }
}
