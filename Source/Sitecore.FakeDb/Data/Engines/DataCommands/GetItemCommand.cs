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
      var dataStorage = this.Database.GetDataStorage();

      return dataStorage.GetSitecoreItem(this.ItemId, this.Language);
    }
  }
}