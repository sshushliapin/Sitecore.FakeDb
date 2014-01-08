namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Items;

  public class GetItemCommand : Sitecore.Data.Engines.DataCommands.GetItemCommand
  {
    protected override Sitecore.Data.Engines.DataCommands.GetItemCommand CreateInstance()
    {
      return new GetItemCommand();
    }

    protected override Item DoExecute()
    {
      var dataStorage = CommandHelper.GetDataStorage(this);

      return dataStorage.Items.ContainsKey(ItemId) ? dataStorage.Items[ItemId] : null;
    }
  }
}