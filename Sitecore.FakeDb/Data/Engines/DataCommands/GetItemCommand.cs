namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Engines.DataCommands;
  using Sitecore.Data.Items;

  public class FakeGetItemCommand : GetItemCommand
  {
    protected override GetItemCommand CreateInstance()
    {
      return new FakeGetItemCommand();
    }

    protected override Item DoExecute()
    {
      var database = (FakeDatabase)this.Database;
      var dataStorage = database.DataStorage;

      return dataStorage.Items.ContainsKey(ItemId) ? dataStorage.Items[ItemId] : null;
    }
  }
}