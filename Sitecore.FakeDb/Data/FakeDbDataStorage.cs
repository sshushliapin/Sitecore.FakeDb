namespace Sitecore.FakeDb.Data
{
  using System.Collections.Generic;
  using Sitecore.Data;

  public class FakeDbDataStorage
  {
    private static readonly ID RootTemplateId = new ID("{C6576836-910C-4A3D-BA03-C277DBD3B827}");

    private readonly IDictionary<ID, ItemDefinition> itemDefinitions = new Dictionary<ID, ItemDefinition>();

    public FakeDbDataStorage()
    {
      this.FillStandardItemDefinitions();
    }

    public IDictionary<ID, ItemDefinition> ItemDefinitions
    {
      get { return this.itemDefinitions; }
    }

    public void Reset()
    {
      this.ItemDefinitions.Clear();
      this.FillStandardItemDefinitions();
    }

    private void FillStandardItemDefinitions()
    {
      this.itemDefinitions.Add(ItemIDs.RootID, new ItemDefinition(ItemIDs.RootID, "sitecore", RootTemplateId, ID.Null));
      this.itemDefinitions.Add(ItemIDs.ContentRoot, new ItemDefinition(ItemIDs.ContentRoot, "content", TemplateIDs.MainSection, ID.Null));
      this.itemDefinitions.Add(ItemIDs.TemplateRoot, new ItemDefinition(ItemIDs.TemplateRoot, "templates", TemplateIDs.MainSection, ID.Null));
    }
  }
}