namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using Sitecore.Diagnostics;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
  public class MoveItemCommand : Sitecore.Data.Engines.DataCommands.MoveItemCommand
  {
    private readonly DataStorage dataStorage;

    public MoveItemCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.MoveItemCommand CreateInstance()
    {
      throw new NotSupportedException();
    }

    protected override bool DoExecute()
    {
      var fakeItem = this.dataStorage.GetFakeItem(this.Item.ID);

      var oldParent = this.dataStorage.GetFakeItem(fakeItem.ParentID);
      oldParent.Children.Remove(fakeItem);

      var destination = this.dataStorage.GetFakeItem(this.Destination.ID);

      destination.Children.Add(fakeItem);

      return true;
    }
  }
}