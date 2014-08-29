namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Threading;
  using Sitecore.Data.Items;

  public class GetParentCommand : Sitecore.Data.Engines.DataCommands.GetParentCommand, IDataEngineCommand
  {
    private readonly ThreadLocal<DataEngineCommand> innerCommand;

    public GetParentCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand> { Value = DataEngineCommand.NotInitialized };
    }

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand.Value = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.GetParentCommand CreateInstance()
    {
      return this.innerCommand.Value.CreateInstance<Sitecore.Data.Engines.DataCommands.GetParentCommand, GetParentCommand>();
    }

    protected override Item DoExecute()
    {
      if (this.Item.ID == ItemIDs.RootID)
      {
        return null;
      }

      var fakeItem = this.innerCommand.Value.DataStorage.GetFakeItem(this.Item.ID);

      return fakeItem != null ? this.innerCommand.Value.DataStorage.GetSitecoreItem(fakeItem.ParentID, this.Item.Language) : null;
    }
  }
}