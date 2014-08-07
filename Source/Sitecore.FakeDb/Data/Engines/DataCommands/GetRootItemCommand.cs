namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Items;
  using System.Threading;

  public class GetRootItemCommand : Sitecore.Data.Engines.DataCommands.GetRootItemCommand, IDataEngineCommand
  {
    private ThreadLocal<DataEngineCommand> innerCommand;

    public GetRootItemCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand>();
      this.innerCommand.Value = DataEngineCommand.NotInitialized;
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