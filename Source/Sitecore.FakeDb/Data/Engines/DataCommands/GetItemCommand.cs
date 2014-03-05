namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Configuration;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  public class GetItemCommand : Sitecore.Data.Engines.DataCommands.GetItemCommand, IRequireDataStorage
  {
    private DataStorage dataStorage;

    public GetItemCommand()
    {
    }

    public GetItemCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public virtual DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.GetItemCommand CreateInstance()
    {
      return new GetItemCommand(this.DataStorage);
    }

    protected override Item DoExecute()
    {
      return this.DataStorage.GetSitecoreItem(this.ItemId, this.Language);
    }

    public void SetDataStorage(DataStorage dataStorage)
    {
      Assert.IsNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }
  }
}