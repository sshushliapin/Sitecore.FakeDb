namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Engines.DataCommands;

  public class FakeSaveItemCommand : SaveItemCommand
  {
    protected override SaveItemCommand CreateInstance()
    {
      return new FakeSaveItemCommand();
    }

    protected override bool DoExecute()
    {
      var database = (FakeDatabase)this.Database;
      database.DataStorage.Items[Item.ID] = Item;

      return true;
    }
  }
}