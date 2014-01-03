namespace Sitecore.FakeDb.Data.DataProviders
{
  using Sitecore.Data;
  using Sitecore.Data.DataProviders;

  public class FakeDbDataProvider : DataProvider
  {
    private readonly FakeDbDataStorage dataStorage = new FakeDbDataStorage();

    public FakeDbDataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    public override bool CreateItem(ID itemId, string itemName, ID templateId, ItemDefinition parent, CallContext context)
    {
      this.DataStorage.ItemDefinitions.Add(itemId, new ItemDefinition(itemId, itemName, templateId, ID.Null));

      return base.CreateItem(itemId, itemName, templateId, parent, context);
    }

    public override ItemDefinition GetItemDefinition(ID itemId, CallContext context)
    {
      return this.DataStorage.ItemDefinitions[itemId];
    }
  }
}