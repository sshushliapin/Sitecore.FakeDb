namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  public class DeleteItemCommand : Sitecore.Data.Engines.DataCommands.DeleteItemCommand
  {
    protected override Sitecore.Data.Engines.DataCommands.DeleteItemCommand CreateInstance()
    {
      return new DeleteItemCommand();
    }

    protected override bool DoExecute()
    {
      var dataStorage = this.Database.GetDataStorage();
      
      return dataStorage.FakeItems.Remove(Item.ID);
    }
  }
}