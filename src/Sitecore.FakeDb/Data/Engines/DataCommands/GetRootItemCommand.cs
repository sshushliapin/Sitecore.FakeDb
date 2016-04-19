namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
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