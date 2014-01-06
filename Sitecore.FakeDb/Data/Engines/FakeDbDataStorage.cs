namespace Sitecore.FakeDb.Data.Engines
{
  using System.Collections.Generic;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Globalization;

  public class FakeDbDataStorage
  {
    private static readonly ID RootTemplateId = new ID("{C6576836-910C-4A3D-BA03-C277DBD3B827}");

    private readonly Database database;

    private readonly IDictionary<ID, Item> items = new Dictionary<ID, Item>();

    public FakeDbDataStorage(Database database)
    {
      this.database = database;
      this.FillStandardItemDefinitions();
    }

    public IDictionary<ID, Item> Items
    {
      get { return this.items; }
    }

    public Database Database
    {
      get { return this.database; }
    }

    public void Reset()
    {
      this.Items.Clear();
      this.FillStandardItemDefinitions();
    }

    private void FillStandardItemDefinitions()
    {
      this.items.Add(ItemIDs.RootID, new Item(ItemIDs.RootID, new ItemData(new ItemDefinition(ItemIDs.RootID, "sitecore", RootTemplateId, ID.Null), Language.Invariant, Version.First, new FieldList()), this.Database));
      this.items.Add(ItemIDs.ContentRoot, new Item(ItemIDs.ContentRoot, new ItemData(new ItemDefinition(ItemIDs.ContentRoot, "content", TemplateIDs.MainSection, ID.Null), Language.Invariant, Version.First, new FieldList()), this.Database));
      this.items.Add(ItemIDs.TemplateRoot, new Item(ItemIDs.TemplateRoot, new ItemData(new ItemDefinition(ItemIDs.TemplateRoot, "templates", TemplateIDs.MainSection, ID.Null), Language.Invariant, Version.First, new FieldList()), this.Database));
    }
  }
}