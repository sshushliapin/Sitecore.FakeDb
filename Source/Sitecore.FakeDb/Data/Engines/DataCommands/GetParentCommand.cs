namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  public class GetParentCommand : Sitecore.Data.Engines.DataCommands.GetParentCommand, IDataEngineCommand
  {
    private DataEngineCommand innerCommand = DataEngineCommand.NotInitialized;

    public virtual void Initialize(DataEngineCommand command)
    {
      Assert.ArgumentNotNull(command, "command");

      this.innerCommand = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.GetParentCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.GetParentCommand, GetParentCommand>();
    }

    protected override Item DoExecute()
    {
      if (this.Item.ID == ItemIDs.RootID)
      {
        return null;
      }

      var fakeItem = this.innerCommand.DataStorage.GetFakeItem(this.Item.ID);

      return fakeItem != null ? this.innerCommand.DataStorage.GetSitecoreItem(fakeItem.ParentID, this.Item.Language) : null;
    }
  }
}