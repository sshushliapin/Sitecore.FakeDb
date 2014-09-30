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

            if (! (args.DsDbItem is DsDbItem))
            {
                return;
            }

            // Deserialize and link descendants, if needed
            FileInfo file = args.DsDbItem.File;
            if (((DsDbItem)args.DsDbItem).IncludeDescendants && file.Directory != null)
            {
                DirectoryInfo childItemsFolder = new DirectoryInfo(
                    file.Directory.FullName + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(file.Name));
                if (childItemsFolder.Exists)
                {
                    foreach (var itemFile in childItemsFolder.GetFiles("*.item", SearchOption.TopDirectoryOnly))
                    {
                        DsDbItem childItem = new DsDbItem(itemFile, true);
                        ((DsDbItem)args.DsDbItem).Children.Add(childItem);
                    }
                }
            }
        }
    }
}
