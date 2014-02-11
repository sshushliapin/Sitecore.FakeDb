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

    private readonly IDictionary<ID, DbItem> fakeItems;

    private readonly IDictionary<ID, DbTemplate> fakeTemplates;

    private const string SitecoreItemName = "sitecore";

    private const string ContentItemName = "content";

    private const string TemplatesItemName = "templates";

    public const string TemplateItemName = "Template";

    public const string TemplateSectionItemName = "Template section";

    public const string TemplateFieldItemName = "Template field";

    public const string BranchItemName = "Branch";

    private Database database;

    public DataStorage()
    {
      this.fakeItems = new Dictionary<ID, DbItem>();
      this.fakeTemplates = new Dictionary<ID, DbTemplate>();
    }

    public Database Database
    {
      get { return this.database; }
    }

    public IDictionary<ID, DbItem> FakeItems
    {
      get { return this.fakeItems; }
    }

    public IDictionary<ID, DbTemplate> FakeTemplates
    {
      get { return this.fakeTemplates; }
    }

    public virtual DbItem GetFakeItem(ID itemId)
    {
      return this.FakeItems.ContainsKey(itemId) ? this.FakeItems[itemId] : null;
    }

    public virtual DbTemplate GetFakeTemplate(ID templateId)
    {
      return this.FakeTemplates.ContainsKey(templateId) ? this.FakeTemplates[templateId] : null;
    }

    public virtual FieldList GetFieldList(ID templateId)
    {
      Assert.IsTrue(this.FakeTemplates.ContainsKey(templateId), "Template '{0}' not found.", templateId);

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

        using (new LanguageSwitcher(language))
        {
          foreach (var field in fakeItem.Fields)
          {
            fields.Add(field.ID, field.Value);
          }
        }
      }

      var item = ItemHelper.CreateInstance(fakeItem.Name, fakeItem.ID, fakeItem.TemplateID, fields, this.Database);

      return item;
    }

    public void SetDatabase(Database db)
    {
      this.database = db;
      this.Reset();
    }

    public void Reset()
    {
      this.FakeTemplates.Clear();
      this.FakeItems.Clear();

      this.FillDefaultFakeTemplates();
      this.FillDefaultFakeItems();
    }

    private void FillDefaultFakeTemplates()
    {
      this.FakeTemplates.Add(TemplateIDs.Template, new DbTemplate(TemplateItemName, TemplateIDs.Template));
    }

    private void FillDefaultFakeItems()
    {
      this.fakeItems.Add(ItemIDs.RootID, new DbItem(SitecoreItemName, ItemIDs.RootID, RootTemplateId) { ParentID = ID.Null, FullPath = "/sitecore" });
      this.fakeItems.Add(ItemIDs.ContentRoot, new DbItem(ContentItemName, ItemIDs.ContentRoot, TemplateIDs.MainSection) { ParentID = ItemIDs.RootID, FullPath = "/sitecore/content" });
      this.fakeItems.Add(ItemIDs.TemplateRoot, new DbItem(TemplatesItemName, ItemIDs.TemplateRoot, TemplateIDs.MainSection) { ParentID = ItemIDs.RootID, FullPath = "/sitecore/templates" });

      // TODO: Move 'Template' item to proper directory to correspond Sitecore structure.
      this.fakeItems.Add(TemplateIDs.Template, new DbItem(TemplateItemName, TemplateIDs.Template, TemplateIDs.Template) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/template" });
      this.fakeItems.Add(TemplateIDs.TemplateSection, new DbItem(TemplateSectionItemName, TemplateIDs.TemplateSection, TemplateIDs.Template) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/template section" });
      this.fakeItems.Add(TemplateIDs.TemplateField, new DbItem(TemplateFieldItemName, TemplateIDs.TemplateField, TemplateIDs.Template) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/template field" });
      this.fakeItems.Add(TemplateIDs.BranchTemplate, new DbItem(BranchItemName, TemplateIDs.BranchTemplate, TemplateIDs.Template) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/branch" });
    }
  }
}