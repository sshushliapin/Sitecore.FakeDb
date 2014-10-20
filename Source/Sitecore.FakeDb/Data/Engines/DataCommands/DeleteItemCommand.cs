namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Collections.Generic;
  using System.Threading;
  using Sitecore.Data;

  public class DeleteItemCommand : Sitecore.Data.Engines.DataCommands.DeleteItemCommand, IDataEngineCommand
  {
    private readonly ThreadLocal<DataEngineCommand> innerCommand;

    public DeleteItemCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand> { Value = DataEngineCommand.NotInitialized };
    }

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand.Value = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.DeleteItemCommand CreateInstance()
    {
      return this.innerCommand.Value.CreateInstance<Sitecore.Data.Engines.DataCommands.DeleteItemCommand, DeleteItemCommand>();
    }

    protected override bool DoExecute()
    {
      return this.RemoveRecursive(this.innerCommand.Value.DataStorage.FakeItems, Item.ID);
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