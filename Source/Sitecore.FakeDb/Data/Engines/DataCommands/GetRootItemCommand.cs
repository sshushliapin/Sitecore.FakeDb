namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Threading;
  using Sitecore.Data.Items;

  public class GetRootItemCommand : Sitecore.Data.Engines.DataCommands.GetRootItemCommand, IDataEngineCommand
  {
    private readonly ThreadLocal<DataEngineCommand> innerCommand;

    public GetRootItemCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand> { Value = DataEngineCommand.NotInitialized };
    }

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand.Value = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.GetRootItemCommand CreateInstance()
    {
      return this.innerCommand.Value.CreateInstance<Sitecore.Data.Engines.DataCommands.GetRootItemCommand, GetRootItemCommand>();
    }

    protected override Item DoExecute()
    {
      return this.innerCommand.Value.DataStorage.GetSitecoreItem(ItemIDs.RootID, this.Language);
    }
  }
}