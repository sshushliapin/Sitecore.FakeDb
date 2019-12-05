namespace Sitecore.FakeDb.Serialization.Pipelines
{
    using System.IO;
    using Sitecore.Diagnostics;
    using Sitecore.FakeDb.Pipelines;

    public class DeserializeDescendants
    {
        public void Process(DsItemLoadingArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            var dsDbItem = args.DsDbItem as DsDbItem;

            if (dsDbItem == null)
            {
                return;
            }

            // Deserialize and link descendants, if needed
            var file = args.DsDbItem.File;
            if (!dsDbItem.IncludeDescendants || file.Directory == null)
            {
                return;
            }

            var childItemsFolder = new DirectoryInfo(file.Directory.FullName + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(file.Name));
            if (!childItemsFolder.Exists)
            {
                return;
            }

            foreach (var itemFile in childItemsFolder.GetFiles("*.item", SearchOption.TopDirectoryOnly))
            {
                DbItem childItem;
                var syncItem = itemFile.Deserialize();

                if (syncItem.TemplateID == TemplateIDs.Template.ToString())
                {
                    childItem = new DsDbTemplate(dsDbItem.SerializationFolderName, syncItem, itemFile);
                }
                else
                {
                    childItem = new DsDbItem(dsDbItem.SerializationFolderName, syncItem, itemFile, true);
                }

                dsDbItem.Children.Add(childItem);
            }
        }
    }
}