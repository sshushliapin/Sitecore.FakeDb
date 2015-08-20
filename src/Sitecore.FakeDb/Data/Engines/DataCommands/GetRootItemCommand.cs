namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  public class GetRootItemCommand : Sitecore.Data.Engines.DataCommands.GetRootItemCommand
  {
    private readonly DataStorage dataStorage;

    public GetRootItemCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.GetRootItemCommand CreateInstance()
    {
      throw new NotSupportedException();
    }

    protected override Item DoExecute()
    {
      return this.dataStorage.GetSitecoreItem(ItemIDs.RootID, this.Language);
    }
  }
}