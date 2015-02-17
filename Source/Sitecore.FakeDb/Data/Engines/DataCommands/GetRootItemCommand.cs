namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  public class GetRootItemCommand : Sitecore.Data.Engines.DataCommands.GetRootItemCommand, IDataEngineCommand
  {
    private DataEngineCommand innerCommand = DataEngineCommand.NotInitialized;

    public virtual void Initialize(DataEngineCommand command)
    {
      Assert.ArgumentNotNull(command, "command");

      this.innerCommand = command;
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