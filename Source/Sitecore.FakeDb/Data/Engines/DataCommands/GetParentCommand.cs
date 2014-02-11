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

      if (this.Item.ID == ItemIDs.RootID)
      {
        return null;
      }

      var fakeItem = dataStorage.GetFakeItem(this.Item.ID);

      return fakeItem != null ? dataStorage.GetSitecoreItem(fakeItem.ParentID, this.Item.Language) : null;
    }
  }
}