namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Items;
  using System.Threading;

  public class GetItemCommand : Sitecore.Data.Engines.DataCommands.GetItemCommand, IDataEngineCommand
  {
    private ThreadLocal<DataEngineCommand> innerCommand;

    public GetItemCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand>();
      this.innerCommand.Value = DataEngineCommand.NotInitialized;
    }

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand.Value = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.GetItemCommand CreateInstance()
    {
      return this.innerCommand.Value.CreateInstance<Sitecore.Data.Engines.DataCommands.GetItemCommand, GetItemCommand>();
    }

    protected override Item DoExecute()
    {
      return this.innerCommand.Value.DataStorage.GetSitecoreItem(this.ItemId, this.Language, this.Version);
    }
  }
}