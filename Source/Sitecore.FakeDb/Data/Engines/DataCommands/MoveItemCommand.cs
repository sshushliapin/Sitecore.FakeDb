namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Diagnostics;

  public class MoveItemCommand : Sitecore.Data.Engines.DataCommands.MoveItemCommand, IDataEngineCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    public virtual void Initialize(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.innerCommand.Initialize(dataStorage);
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