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
      return this.RemoveRecursive(this.innerCommand.DataStorage.FakeItems, Item.ID);
    }

    protected virtual bool RemoveRecursive(IDictionary<ID, DbItem> fakeItems, ID itemId)
    {
      if (!fakeItems.ContainsKey(itemId))
      {
        return false;
      }

      var item = fakeItems[itemId];
      foreach (var child in item.Children)
      {
        this.RemoveRecursive(fakeItems, child.ID);
      }

      if (!ID.IsNullOrEmpty(item.ParentID))
      {
        var parent = fakeItems[item.ParentID];
        parent.Children.Remove(item);
      }

      return fakeItems.Remove(itemId);
    }
  }
}