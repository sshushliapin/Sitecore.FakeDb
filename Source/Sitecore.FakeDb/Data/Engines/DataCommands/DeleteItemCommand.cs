namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Collections.Generic;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  public class DeleteItemCommand : Sitecore.Data.Engines.DataCommands.DeleteItemCommand
  {
    private readonly DataStorage dataStorage;

    public DeleteItemCommand()
      : this((DataStorage)Factory.CreateObject("dataStorage", true))
    {
    }

    public DeleteItemCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public virtual DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.DeleteItemCommand CreateInstance()
    {
      return new DeleteItemCommand(this.DataStorage);
    }

    protected override bool DoExecute()
    {
      return this.RemoveRecursive(this.DataStorage.FakeItems, Item.ID);
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