using Sitecore.Abstractions;

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
        public FakeStandardValuesProvider(
            BaseItemManager itemManager,
            BaseTemplateManager templateManager,
            BaseFactory factory)
            : base(itemManager, templateManager, factory)
        {
        }

        public virtual DataStorage DataStorage(Database database)
        {
            Assert.IsNotNull(database, "database");

            return DataStorageSwitcher.CurrentValue(database.Name);
        }

        public override string GetStandardValue(Field field)
        {
            var templateId = field.Item.TemplateID;

            Assert.IsNotNull(this.DataStorage(field.Database), "DataStorage cannot be null.");

            var template = this.DataStorage(field.Database).GetFakeTemplate(templateId);

            if (template == null)
            {
                // This will be the case for things like TemplateFieldItem.Type. 
                // We don't have those templates and it's safe to just return an empty string
                return string.Empty;
            }

            var standardValue = this.FindStandardValueInTheTemplate(template, field) ?? string.Empty;

            return this.ReplaceTokens(standardValue, field.Item);
        }

        protected string ReplaceTokens(string standardValue, Item item)
        {
            Assert.IsNotNull(standardValue, "standardValue");
            Assert.IsNotNull(item, "item");

            return standardValue.Replace("$name", item.Name);
        }

        protected string FindStandardValueInTheTemplate(DbTemplate template, Field field)
        {
            if (template.StandardValues.ContainsKey(field.ID))
            {
                return template.StandardValues[field.ID].Value;
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

                var baseTemplate = this.DataStorage(field.Database).GetFakeTemplate(baseId);
                if (baseTemplate == null)
                {
                    throw new TemplateNotFoundException("The template \"{0}\" was not found.".FormatWith(baseId.ToString()));
                }

                var value = this.FindStandardValueInTheTemplate(baseTemplate, field);
                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }
    }
}
