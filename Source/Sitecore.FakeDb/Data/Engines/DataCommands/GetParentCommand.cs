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
      var dataStorage = this.Database.GetDataStorage();

      if (Item.ID == ItemIDs.RootID)
      {
        return null;
      }

      var fakeItem = dataStorage.GetFakeItem(Item.ID);

      return fakeItem != null ? dataStorage.GetSitecoreItem(fakeItem.ParentID) : null;
    }
  }
}