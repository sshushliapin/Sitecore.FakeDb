namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Collections.Generic;
  using Sitecore.Data;

  public class DeleteItemCommand : Sitecore.Data.Engines.DataCommands.DeleteItemCommand
  {
    protected override Sitecore.Data.Engines.DataCommands.DeleteItemCommand CreateInstance()
    {
      return new DeleteItemCommand();
    }

    protected override bool DoExecute()
    {
      var dataStorage = this.Database.GetDataStorage();

      return RemoveRecursive(dataStorage.FakeItems, Item.ID);
    }

    private bool RemoveRecursive(IDictionary<ID, DbItem> fakeItems, ID itemId)
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

      return fakeItems.Remove(itemId);
    }
  }
}