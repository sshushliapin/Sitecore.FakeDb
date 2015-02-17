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

      DsDbItem dsDbItem = args.DsDbItem as DsDbItem;

      if (dsDbItem == null
          || !dsDbItem.DeserializeLinkedTemplate
          || args.DataStorage.GetFakeItem(dsDbItem.TemplateID) != null)
      {
        return;
      }

      DeserializeTemplate(args.DataStorage, dsDbItem.TemplateID, dsDbItem.SerializationFolderName);
    }

    private static void DeserializeTemplate(DataStorage dataStorage, ID templateId, string serializationFolderName)
    {
      string filePath = templateId.FindFilePath(serializationFolderName);

      if (string.IsNullOrWhiteSpace(filePath)
          || !File.Exists(filePath))
      {
        return;
      }

      DsDbTemplate dsDbTemplate = new DsDbTemplate(templateId, serializationFolderName);

      dataStorage.AddFakeTemplate(dsDbTemplate);

      // Deserialize base templates
      DbField baseTemplatesField = dsDbTemplate.Fields
                                      .FirstOrDefault(f => f.ID == FieldIDs.BaseTemplate);
      if (!string.IsNullOrWhiteSpace(baseTemplatesField.Value))
      {
        foreach (ID baseTemplateId in baseTemplatesField.Value
                    .Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(ID.IsID)
                    .Select(ID.Parse))
        {
          if (dataStorage.GetFakeItem(baseTemplateId) == null)
          {
            DeserializeTemplate(dataStorage, baseTemplateId, dsDbTemplate.SerializationFolderName);
          }
        }
      }
    }
  }
}