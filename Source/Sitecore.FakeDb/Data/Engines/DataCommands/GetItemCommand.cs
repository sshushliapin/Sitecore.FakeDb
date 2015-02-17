namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  public class GetItemCommand : Sitecore.Data.Engines.DataCommands.GetItemCommand, IDataEngineCommand
  {
    private DataEngineCommand innerCommand = DataEngineCommand.NotInitialized;

    public virtual void Initialize(DataEngineCommand command)
    {
      Assert.ArgumentNotNull(command, "command");

      this.innerCommand = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.GetItemCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.GetItemCommand, GetItemCommand>();
    }

    protected override Item DoExecute()
    {
      return this.innerCommand.DataStorage.GetSitecoreItem(this.ItemId, this.Language, this.Version);
    }
  }
}