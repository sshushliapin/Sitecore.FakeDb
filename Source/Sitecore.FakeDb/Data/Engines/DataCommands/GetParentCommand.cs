namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Configuration;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  public class GetParentCommand : Sitecore.Data.Engines.DataCommands.GetParentCommand, IRequireDataStorage
  {
    private DataStorage dataStorage;

    public GetParentCommand()
    {
    }

    public GetParentCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public virtual DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.GetParentCommand CreateInstance()
    {
      return new GetParentCommand(this.DataStorage);
    }

    protected override Item DoExecute()
    {
      if (this.Item.ID == ItemIDs.RootID)
      {
        return null;
      }

      var fakeItem = this.DataStorage.GetFakeItem(this.Item.ID);

      return fakeItem != null ? this.DataStorage.GetSitecoreItem(fakeItem.ParentID, this.Item.Language) : null;
    }

    public void SetDataStorage(DataStorage dataStorage)
    {
      Assert.IsNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }
  }
}