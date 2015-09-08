namespace Sitecore.FakeDb.Data
{
  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Exceptions;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.StringExtensions;

  public class FakeStandardValuesProvider : StandardValuesProvider
  {
    public virtual DataStorage DataStorage
    {
      get { return DataStorageSwitcher.CurrentValue; }
    }

    public override string GetStandardValue(Field field)
    {
      var templateId = field.Item.TemplateID;

      Assert.IsNotNull(this.DataStorage, "DataStorage cannot be null.");

      var template = this.DataStorage.GetFakeTemplate(templateId);

      if (template == null)
      {
        // This will be the case for things like TemplateFieldItem.Type. 
        // We don't have those templates and it's safe to just return an empty string
        return string.Empty;
      }

      var standardValue = this.FindStandardValueInTheTemplate(template, field.ID) ?? string.Empty;

      return this.ReplaceTokens(standardValue, field.Item);
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
        if (ID.IsNullOrEmpty(baseId))
        {
          continue;
        }

        var baseTemplate = this.DataStorage.GetFakeTemplate(baseId);
        if (baseTemplate == null)
        {
          throw new TemplateNotFoundException("The template \"{0}\" was not found.".FormatWith(baseId.ToString()));
        }

        var value = this.FindStandardValueInTheTemplate(baseTemplate, fieldId);
        if (value != null)
        {
          return value;
        }
      }

      return null;
    }
  }
}