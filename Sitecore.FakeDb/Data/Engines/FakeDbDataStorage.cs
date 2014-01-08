namespace Sitecore.FakeDb.Data.Engines
{
  using System.Collections.Generic;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Items;

  // TODO: Find better name.
  public class FakeDbDataStorage
  {
    private static readonly ID RootTemplateId = new ID("{C6576836-910C-4A3D-BA03-C277DBD3B827}");

    private readonly Database database;

    private readonly IDictionary<ID, FItem> fakeItems;

    private readonly IDictionary<ID, Item> items;

    private const string SitecoreItemName = "sitecore";

    private const string ContentItemName = "content";

    private const string TemplatesItemName = "templates";

    public FakeDbDataStorage(Database database)
    {
      this.database = database;
      this.fakeItems = new Dictionary<ID, FItem>();
      this.items = new Dictionary<ID, Item>();

      this.FillDefaultFakeItems();
      this.FillDefaultSitecoreItems();
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

    public void Reset()
    {
      this.FakeItems.Clear();
      this.Items.Clear();

      this.FillDefaultFakeItems();
      this.FillDefaultSitecoreItems();
    }

    private void FillDefaultFakeItems()
    {
      this.fakeItems.Add(ItemIDs.RootID, new FItem(SitecoreItemName, ItemIDs.RootID, RootTemplateId) { ParentID = ID.Null, FullPath = "/sitecore" });
      this.fakeItems.Add(ItemIDs.ContentRoot, new FItem(ContentItemName, ItemIDs.ContentRoot, TemplateIDs.MainSection) { ParentID = ItemIDs.RootID, FullPath = "/sitecore/content" });
      this.fakeItems.Add(ItemIDs.TemplateRoot, new FItem(TemplatesItemName, ItemIDs.TemplateRoot, TemplateIDs.MainSection) { ParentID = ItemIDs.RootID, FullPath = "/sitecore/templates" });
    }

    private void FillDefaultSitecoreItems()
    {
      this.items.Add(ItemIDs.RootID, ItemHelper.CreateInstance(SitecoreItemName, ItemIDs.RootID, RootTemplateId, new FieldList(), this.Database));
      this.items.Add(ItemIDs.ContentRoot, ItemHelper.CreateInstance(ContentItemName, ItemIDs.ContentRoot, TemplateIDs.MainSection, new FieldList(), this.Database));
      this.items.Add(ItemIDs.TemplateRoot, ItemHelper.CreateInstance(TemplatesItemName, ItemIDs.TemplateRoot, TemplateIDs.MainSection, new FieldList(), this.Database));
    }
  }
}