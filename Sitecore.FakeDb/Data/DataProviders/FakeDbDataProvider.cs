namespace Sitecore.FakeDb.Data.DataProviders
{
  using System.Collections.Generic;
  using Sitecore.Data;
  using Sitecore.Data.DataProviders;

  public class FakeDbDataProvider : DataProvider
  {
    private readonly IDictionary<ID, ItemDefinition> itemDefinitions = new Dictionary<ID, ItemDefinition>();

    public IDictionary<ID, ItemDefinition> ItemDefinitions
    {
      get { return this.itemDefinitions; }
    }

    public override bool CreateItem(ID itemId, string itemName, ID templateId, ItemDefinition parent, CallContext context)
    {
      this.ItemDefinitions.Add(itemId, new ItemDefinition(itemId, itemName, templateId, ID.Null));

      return base.CreateItem(itemId, itemName, templateId, parent, context);
    }

    public override ItemDefinition GetItemDefinition(ID itemId, CallContext context)
    {
      return this.ItemDefinitions[itemId];
    }
  }
}