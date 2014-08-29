namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Threading;

  public class HasChildrenCommand : Sitecore.Data.Engines.DataCommands.HasChildrenCommand, IDataEngineCommand
  {
    private readonly ThreadLocal<DataEngineCommand> innerCommand;

    public HasChildrenCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand> { Value = DataEngineCommand.NotInitialized };
    }

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand.Value = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.HasChildrenCommand CreateInstance()
    {
      return this.innerCommand.Value.CreateInstance<Sitecore.Data.Engines.DataCommands.HasChildrenCommand, HasChildrenCommand>();
    }

    protected override bool DoExecute()
    {
      var fakeItem = this.innerCommand.Value.DataStorage.GetFakeItem(Item.ID);

      return fakeItem.Children.Count > 0;
    }
  }
}