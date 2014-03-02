namespace Sitecore.FakeDb.Data.Engines
{
  using System.Collections.Generic;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.Globalization;

  public class DataStorage
  {
    private static readonly ID RootTemplateId = new ID("{C6576836-910C-4A3D-BA03-C277DBD3B827}");

    private const string SitecoreItemName = "sitecore";

    private const string ContentItemName = "content";

    private const string TemplatesItemName = "templates";

    public const string TemplateItemName = "Template";

    public const string TemplateSectionItemName = "Template section";

    public const string TemplateFieldItemName = "Template field";

    public const string BranchItemName = "Branch";

    public DataStorage()
    {
      this.FakeItems = new Dictionary<ID, DbItem>();
      this.FakeTemplates = new Dictionary<ID, DbTemplate>();

      this.Reset();
    }

    // TODO:[Mid] Should be removed.
    public Database Database { get; private set; }

    /// <summary>
    /// Gets the fake items.
    /// </summary>
    public IDictionary<ID, DbItem> FakeItems { get; private set; }

    public IDictionary<ID, DbTemplate> FakeTemplates { get; private set; }

    public virtual DbItem GetFakeItem(ID itemId)
    {
      Assert.ArgumentCondition(!ID.IsNullOrEmpty(itemId), "itemId", "Value cannot be null.");

      return this.FakeItems.ContainsKey(itemId) ? this.FakeItems[itemId] : null;
    }

    public virtual DbTemplate GetFakeTemplate(ID templateId)
    {
      Assert.ArgumentCondition(!ID.IsNullOrEmpty(templateId), "templateId", "Value cannot be null.");

      return this.FakeTemplates.ContainsKey(templateId) ? this.FakeTemplates[templateId] : null;
    }

    public virtual FieldList GetFieldList(ID templateId)
    {
      Assert.ArgumentCondition(!ID.IsNullOrEmpty(templateId), "templateId", "Value cannot be null.");

      var templates = this.GetFakeTemplate(templateId);
      Assert.IsNotNull(templates, "Template '{0}' not found.", templateId);

      var fields = new FieldList();
      foreach (var field in this.FakeTemplates[templateId].Fields)
      {
        fields.Add(field.ID, string.Empty);
      }

      return fields;
    }

    public virtual Item GetSitecoreItem(ID itemId, Language language)
    {
      if (!this.FakeItems.ContainsKey(itemId))
      {
        return null;
      }

      var fakeItem = this.FakeItems[itemId];

      var fields = new FieldList();
      if (this.FakeTemplates.ContainsKey(fakeItem.TemplateID))
      {
        fields = this.GetFieldList(fakeItem.TemplateID);

        foreach (var field in fakeItem.Fields)
        {
          var value = field.LocalizableValues.ContainsKey(language.Name) ? field.LocalizableValues[language.Name] : field.Value;
          fields.Add(field.ID, value);
        }
      }

      var item = ItemHelper.CreateInstance(fakeItem.Name, fakeItem.ID, fakeItem.TemplateID, fields, this.Database, language);

      return item;
    }

    public void SetDatabase(Database db)
    {
      this.Database = db;
      this.Reset();
    }

    public void Reset()
    {
      this.FakeTemplates.Clear();
      this.FakeItems.Clear();

      this.FillDefaultFakeTemplates();
      this.FillDefaultFakeItems();
    }

    protected virtual void FillDefaultFakeTemplates()
    {
      this.FakeTemplates.Add(TemplateIDs.Template, new DbTemplate(TemplateItemName, TemplateIDs.Template));
    }

    protected virtual void FillDefaultFakeItems()
    {
      this.FakeItems.Add(ItemIDs.RootID, new DbItem(SitecoreItemName, ItemIDs.RootID, RootTemplateId) { ParentID = ID.Null, FullPath = "/sitecore" });
      this.FakeItems.Add(ItemIDs.ContentRoot, new DbItem(ContentItemName, ItemIDs.ContentRoot, TemplateIDs.MainSection) { ParentID = ItemIDs.RootID, FullPath = "/sitecore/content" });
      this.FakeItems.Add(ItemIDs.TemplateRoot, new DbItem(TemplatesItemName, ItemIDs.TemplateRoot, TemplateIDs.MainSection) { ParentID = ItemIDs.RootID, FullPath = "/sitecore/templates" });

      // TODO: Move 'Template' item to proper directory to correspond Sitecore structure.
      this.FakeItems.Add(TemplateIDs.Template, new DbItem(TemplateItemName, TemplateIDs.Template, TemplateIDs.Template) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/template" });
      this.FakeItems.Add(TemplateIDs.TemplateSection, new DbItem(TemplateSectionItemName, TemplateIDs.TemplateSection, TemplateIDs.Template) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/template section" });
      this.FakeItems.Add(TemplateIDs.TemplateField, new DbItem(TemplateFieldItemName, TemplateIDs.TemplateField, TemplateIDs.Template) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/template field" });
      this.FakeItems.Add(TemplateIDs.BranchTemplate, new DbItem(BranchItemName, TemplateIDs.BranchTemplate, TemplateIDs.Template) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/branch" });
    }
  }
}