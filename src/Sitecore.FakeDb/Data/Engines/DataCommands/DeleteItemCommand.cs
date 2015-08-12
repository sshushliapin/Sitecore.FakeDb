namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Collections.Generic;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  public class DeleteItemCommand : Sitecore.Data.Engines.DataCommands.DeleteItemCommand, IDataEngineCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    public virtual void Initialize(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.innerCommand.Initialize(dataStorage);
    }

    protected override Sitecore.Data.Engines.DataCommands.DeleteItemCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.DeleteItemCommand, DeleteItemCommand>();
    }

    protected override bool DoExecute()
    {
      return this.RemoveRecursive(this.innerCommand.DataStorage.GetFakeItems(), this.Item.ID);
    }

    protected virtual bool RemoveRecursive(IEnumerable<DbItem> fakeItems, ID itemId)
    {
      var dataStorage = this.innerCommand.DataStorage;

      var item = dataStorage.GetFakeItem(itemId);
      if (item == null)
      {
        return false;
      }

      foreach (var child in item.Children)
      {
        this.RemoveRecursive(fakeItems, child.ID);
      }

      if (!ID.IsNullOrEmpty(item.ParentID))
      {
        var parent = dataStorage.GetFakeItem(item.ParentID);
        parent.Children.Remove(item);
      }

      return dataStorage.RemoveFakeItem(itemId);
    }
  }
}