namespace Sitecore.FakeDb.Data.Engines
{
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.FakeDb.Templates;

  // TODO: Find better name.
  public class DataStorage
  {
    private static readonly ID RootTemplateId = new ID("{C6576836-910C-4A3D-BA03-C277DBD3B827}");

    private readonly IDictionary<ID, FItem> fakeItems;

    private readonly IDictionary<ID, Item> items;

    private readonly IDictionary<ID, FTemplate> fakeTemplates;

    private const string SitecoreItemName = "sitecore";

    private const string ContentItemName = "content";

    private const string TemplatesItemName = "templates";

    public const string TemplateItemName = "Template";

    public const string TemplateSectionItemName = "Template section";

    public const string TemplateFieldItemName = "Template field";

    private Database database;

    public DataStorage()
    {
      this.fakeItems = new Dictionary<ID, FItem>();
      this.items = new Dictionary<ID, Item>();
      this.fakeTemplates = new Dictionary<ID, FTemplate>();
    }

    public Database Database
    {
      get { return this.database; }
    }

    public IDictionary<ID, FItem> FakeItems
    {
      get { return this.fakeItems; }
    }

    public IDictionary<ID, Item> Items
    {
      get { return this.items; }
    }

    public IDictionary<ID, FTemplate> FakeTemplates
    {
      get { return this.fakeTemplates; }
    }

    public virtual FItem GetFakeItem(ID itemId)
    {
      return this.FakeItems.ContainsKey(itemId) ? this.FakeItems[itemId] : null;
    }

    public virtual Item GetSitecoreItem(ID itemId)
    {
      return this.FakeItems.ContainsKey(itemId) ? this.Items[itemId] : null;
    }

    public void SetDatabase(Database db)
    {
      this.database = db;
      this.Reset();
    }

    public void Reset()
    {
      this.FakeItems.Clear();
      this.Items.Clear();
      this.FakeTemplates.Clear();

      this.FillDefaultFakeItems();
      this.FillDefaultSitecoreItems();
    }

    private void FillDefaultFakeItems()
    {
      this.fakeItems.Add(ItemIDs.RootID, new FItem(SitecoreItemName, ItemIDs.RootID, RootTemplateId) { ParentID = ID.Null, FullPath = "/sitecore" });
      this.fakeItems.Add(ItemIDs.ContentRoot, new FItem(ContentItemName, ItemIDs.ContentRoot, TemplateIDs.MainSection) { ParentID = ItemIDs.RootID, FullPath = "/sitecore/content" });
      this.fakeItems.Add(ItemIDs.TemplateRoot, new FItem(TemplatesItemName, ItemIDs.TemplateRoot, TemplateIDs.MainSection) { ParentID = ItemIDs.RootID, FullPath = "/sitecore/templates" });

      // TODO: Move 'Template' item to proper directory to correspond Sitecore structure.
      this.fakeItems.Add(TemplateIDs.Template, new FItem(TemplateItemName, TemplateIDs.Template, TemplateIDs.Template) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/template" });
      this.fakeItems.Add(TemplateIDs.TemplateSection, new FItem(TemplateSectionItemName, TemplateIDs.TemplateSection, TemplateIDs.Template) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/template section" });
      this.fakeItems.Add(TemplateIDs.TemplateField, new FItem(TemplateFieldItemName, TemplateIDs.TemplateField, TemplateIDs.Template) { ParentID = ItemIDs.TemplateRoot, FullPath = "/sitecore/templates/template field" });
    }

    private void FillDefaultSitecoreItems()
    {
      this.items.Add(ItemIDs.RootID, ItemHelper.CreateInstance(SitecoreItemName, ItemIDs.RootID, RootTemplateId, new FieldList(), this.Database));
      this.items.Add(ItemIDs.ContentRoot, ItemHelper.CreateInstance(ContentItemName, ItemIDs.ContentRoot, TemplateIDs.MainSection, new FieldList(), this.Database));
      this.items.Add(ItemIDs.TemplateRoot, ItemHelper.CreateInstance(TemplatesItemName, ItemIDs.TemplateRoot, TemplateIDs.MainSection, new FieldList(), this.Database));
      this.items.Add(TemplateIDs.Template, ItemHelper.CreateInstance(TemplateItemName, TemplateIDs.Template, TemplateIDs.Template, new FieldList(), this.Database));
      this.items.Add(TemplateIDs.TemplateSection, ItemHelper.CreateInstance(TemplateSectionItemName, TemplateIDs.TemplateSection, TemplateIDs.Template, new FieldList(), this.Database));
      this.items.Add(TemplateIDs.TemplateField, ItemHelper.CreateInstance(TemplateFieldItemName, TemplateIDs.TemplateField, TemplateIDs.Template, new FieldList(), this.Database));
    }

    public virtual FieldList GetFieldList(ID templateId)
    {
      Assert.IsTrue(this.FakeTemplates.ContainsKey(templateId), "Template '{0}' not found.", templateId);

      var fields = new FieldList();
      foreach (var field in this.FakeTemplates[templateId].Fields)
      {













        fields.Add(field.Value, string.Empty);
      }

      return fields;
    }
  }
}