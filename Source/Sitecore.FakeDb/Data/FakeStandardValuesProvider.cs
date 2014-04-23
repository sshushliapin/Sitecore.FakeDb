namespace Sitecore.FakeDb.Data
{
  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Data.Engines;

  public class FakeStandardValuesProvider : StandardValuesProvider, IRequireDataStorage
  {
    private DataStorage storage;

    DataStorage IRequireDataStorage.DataStorage
    {
      get { return this.storage; }
    }

    public override string GetStandardValue(Field field)
    {
      var templateId = field.Item.TemplateID;
      var template = storage.GetFakeTemplate(templateId);

      if (template == null)
      {
        // This will be the case for things like TemplateFieldItem.Type. 
        // We don't have those templates and it's safe to just return an empty string
        return string.Empty;
      }

      if (!template.StandardValues.InnerFields.ContainsKey(field.ID))
      {
        return string.Empty;
      }

      return template.StandardValues[field.ID].Value;
    }

    void IRequireDataStorage.SetDataStorage(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "storage");

      this.storage = dataStorage;
    }
  }
}