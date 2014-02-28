namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Configuration;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  public class GetRootItemCommand : Sitecore.Data.Engines.DataCommands.GetRootItemCommand
  {
    private readonly DataStorage dataStorage;

    public GetRootItemCommand()
      : this((DataStorage)Factory.CreateObject("dataStorage", true))
    {
    }

    public GetRootItemCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public virtual DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.GetRootItemCommand CreateInstance()
    {
      return new GetRootItemCommand(this.DataStorage);
    }

    protected override Item DoExecute()
    {
      return this.DataStorage.GetSitecoreItem(ItemIDs.RootID, this.Language);
    }
  }
}