namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Collections.Generic;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  public class DeleteItemCommand : Sitecore.Data.Engines.DataCommands.DeleteItemCommand, IDataEngineCommand
  {
    private DataEngineCommand innerCommand = DataEngineCommand.NotInitialized;

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.DeleteItemCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.DeleteItemCommand, DeleteItemCommand>();
    }

    protected override bool DoExecute()
    {
      return this.RemoveRecursive(this.innerCommand.DataStorage.FakeItems, Item.ID);
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