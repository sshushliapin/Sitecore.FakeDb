namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  public class GetRootItemCommand : Sitecore.Data.Engines.DataCommands.GetRootItemCommand, IDataEngineCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    public virtual void Initialize(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.innerCommand.Initialize(dataStorage);
    }

    protected override Sitecore.Data.Engines.DataCommands.GetRootItemCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.GetRootItemCommand, GetRootItemCommand>();
    }

    protected override Item DoExecute()
    {
      return this.innerCommand.DataStorage.GetSitecoreItem(ItemIDs.RootID, this.Language);
    }
  }
}