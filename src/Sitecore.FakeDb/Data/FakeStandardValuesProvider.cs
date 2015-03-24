namespace Sitecore.FakeDb.Data
{
  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
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

      Assert.IsNotNull(this.storage, "DataStorage cannot be null.");

      var template = this.storage.GetFakeTemplate(templateId);

      if (template == null)
      {
        // This will be the case for things like TemplateFieldItem.Type. 
        // We don't have those templates and it's safe to just return an empty string
        return string.Empty;
      }

      var standardValue = FindStandardValueInTheTemplate(template, field.ID) ?? string.Empty;

      return ReplaceTokens(standardValue, field.Item);
    }

    protected string ReplaceTokens(string standardValue, Item item)
    {
      Assert.IsNotNull(standardValue, "standardValue");
      Assert.IsNotNull(item, "item");

      return standardValue.Replace("$name", item.Name);
    }

    protected string FindStandardValueInTheTemplate(DbTemplate template, ID fieldId)
    {
      if (template.StandardValues.ContainsKey(fieldId))
      {
        return template.StandardValues[fieldId].Value;
      }

      if (template.BaseIDs == null || template.BaseIDs.Length <= 0)
      {
        return null;
      }

      foreach (var baseId in template.BaseIDs)
      {
        var baseTemplate = this.storage.GetFakeTemplate(baseId);
        var value = this.FindStandardValueInTheTemplate(baseTemplate, fieldId);

        if (value != null)
        {
          return value;
        }
      }

      return null;
    }

    void IRequireDataStorage.SetDataStorage(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.storage = dataStorage;
    }
  }
}