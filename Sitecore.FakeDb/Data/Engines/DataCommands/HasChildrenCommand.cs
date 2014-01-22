namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  public class HasChildrenCommand : Sitecore.Data.Engines.DataCommands.HasChildrenCommand
  {
    protected override Sitecore.Data.Engines.DataCommands.HasChildrenCommand CreateInstance()
    {
      return new HasChildrenCommand();
    }

    protected override bool DoExecute()
    {
      var dataStorage = this.Database.GetDataStorage();

      var fakeItem = dataStorage.GetFakeItem(Item.ID);

      return fakeItem.Children.Count > 0;
    }
  }
}