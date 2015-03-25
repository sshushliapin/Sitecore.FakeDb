namespace Sitecore.FakeDb.Serialization.Pipelines
{
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Pipelines;

  public class LoadTemplateFields
  {
    public void Process(DsItemLoadingArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      var template = args.DsDbItem as DsDbTemplate;
      Assert.ArgumentNotNull(template, "Item was not a DsDbTemplate, which is required here");

      foreach (var descendantItem in template.File
        .DeserializeAll(template.SyncItem, Deserializer.GetSerializationFolder(template.SerializationFolderName), 3)
        .Where(i => ID.IsID(i.TemplateID) && ID.Parse(i.TemplateID) == TemplateIDs.TemplateField))
      {
        var isSharedField = descendantItem.SharedFields.FirstOrDefault(f => "Shared".Equals(f.FieldName));
        var typeField = descendantItem.SharedFields.FirstOrDefault(f => "Type".Equals(f.FieldName));

        var isShared = isSharedField != null && "1".Equals(isSharedField.FieldValue);

        template.Fields.Add(new DbField(descendantItem.Name, ID.Parse(descendantItem.ID))
          {
            Shared = isShared,
            Type = typeField != null ? typeField.FieldValue : string.Empty
          });
      }
    }
  }
}