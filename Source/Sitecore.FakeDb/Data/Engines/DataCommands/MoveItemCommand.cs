using System.Threading;
namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  public class MoveItemCommand : Sitecore.Data.Engines.DataCommands.MoveItemCommand, IDataEngineCommand
  {
    private ThreadLocal<DataEngineCommand> innerCommand;

    public MoveItemCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand>();
      this.innerCommand.Value = DataEngineCommand.NotInitialized;
    }

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand.Value = command;
    }


    protected override Sitecore.Data.Engines.DataCommands.MoveItemCommand CreateInstance()
    {
      return this.innerCommand.Value.CreateInstance<Sitecore.Data.Engines.DataCommands.MoveItemCommand, MoveItemCommand>();
    }

    protected override bool DoExecute()
    {
      var dataStorage = this.innerCommand.Value.DataStorage;

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