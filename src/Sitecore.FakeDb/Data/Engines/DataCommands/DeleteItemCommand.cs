namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using System.Collections.Generic;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
  public class DeleteItemCommand : Sitecore.Data.Engines.DataCommands.DeleteItemCommand
  {
    private readonly DataStorage dataStorage;

    public DeleteItemCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.DeleteItemCommand CreateInstance()
    {
      throw new NotSupportedException();
    }

    protected override bool DoExecute()
    {
      return this.RemoveRecursive(this.dataStorage.GetFakeItems(), this.Item.ID);
    }

    protected virtual bool RemoveRecursive(IEnumerable<DbItem> fakeItems, ID itemId)
    {
      var item = this.dataStorage.GetFakeItem(itemId);
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
        var parent = this.dataStorage.GetFakeItem(item.ParentID);
        if (parent != null)
        {
          parent.Children.Remove(item);
        }
      }

      return this.dataStorage.RemoveFakeItem(itemId);
    }
  }
}