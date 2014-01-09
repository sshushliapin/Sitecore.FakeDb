namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Items;

  public class GetParentCommand : Sitecore.Data.Engines.DataCommands.GetParentCommand
  {
    protected override Sitecore.Data.Engines.DataCommands.GetParentCommand CreateInstance()
    {
      return new GetParentCommand();
    }

    protected override Item DoExecute()
    {
      var dataStorage = CommandHelper.GetDataStorage(this);

      var fakeItem = dataStorage.GetFakeItem(Item.ID);
      return fakeItem != null ? dataStorage.GetSitecoreItem(fakeItem.ParentID) : null;
    }
  }
}