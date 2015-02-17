namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Diagnostics;

  public class MoveItemCommand : Sitecore.Data.Engines.DataCommands.MoveItemCommand, IDataEngineCommand
  {
    private DataEngineCommand innerCommand = DataEngineCommand.NotInitialized;

    public virtual void Initialize(DataEngineCommand command)
    {
      Assert.ArgumentNotNull(command, "command");

      this.innerCommand = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.MoveItemCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.MoveItemCommand, MoveItemCommand>();
    }

    protected override bool DoExecute()
    {
      var dataStorage = this.innerCommand.DataStorage;

      var fakeItem = dataStorage.GetFakeItem(this.Item.ID);

      var oldParent = dataStorage.GetFakeItem(fakeItem.ParentID);
      oldParent.Children.Remove(fakeItem);

      var destination = dataStorage.GetFakeItem(Destination.ID);

      fakeItem.ParentID = destination.ID;
      fakeItem.FullPath = destination.FullPath + "/" + this.Item.Name;

      destination.Children.Add(fakeItem);

      return true;
    }
  }
}