namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  public class SaveItemCommand : Sitecore.Data.Engines.DataCommands.SaveItemCommand
  {
    protected override Sitecore.Data.Engines.DataCommands.SaveItemCommand CreateInstance()
    {
      return new SaveItemCommand();
    }

    protected override bool DoExecute()
    {
      var dataStorage = CommandHelper.GetDataStorage(this);
      dataStorage.Items[Item.ID] = Item;

      return true;
    }
  }
}