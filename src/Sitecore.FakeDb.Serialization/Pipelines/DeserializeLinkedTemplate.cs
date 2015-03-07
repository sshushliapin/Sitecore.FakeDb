namespace Sitecore.FakeDb.Serialization.Pipelines
{
  using System;
  using System.IO;
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Pipelines;

  public class DeserializeLinkedTemplate
  {
    public void Process(DsItemLoadingArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      var dsDbItem = args.DsDbItem as DsDbItem;

      if (dsDbItem == null || !dsDbItem.DeserializeLinkedTemplate || args.DataStorage.GetFakeItem(dsDbItem.TemplateID) != null)
      {
        return;
      }

      DeserializeTemplate(args.DataStorage, dsDbItem.TemplateID, dsDbItem.SerializationFolderName);
    }

    private static void DeserializeTemplate(DataStorage dataStorage, ID templateId, string serializationFolderName)
    {
      var filePath = templateId.FindFilePath(serializationFolderName);

      if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
      {
        return;
      }

      var dsDbTemplate = new DsDbTemplate(templateId, serializationFolderName);

      dataStorage.AddFakeItem(dsDbTemplate);

      // Deserialize base templates
      var baseTemplatesField = dsDbTemplate.Fields.FirstOrDefault(f => f.ID == FieldIDs.BaseTemplate);
      if (string.IsNullOrWhiteSpace(baseTemplatesField.Value))
      {
        return;
      }

      foreach (var baseTemplateId in baseTemplatesField.Value.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
        .Where(ID.IsID)
        .Select(ID.Parse)
        .Where(baseTemplateId => dataStorage.GetFakeItem(baseTemplateId) == null))
      {
        DeserializeTemplate(dataStorage, baseTemplateId, dsDbTemplate.SerializationFolderName);
      }
    }
  }
}