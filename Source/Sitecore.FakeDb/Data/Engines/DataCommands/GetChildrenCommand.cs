namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Linq;
  using System.Threading;
  using Sitecore.Collections;

  public class GetChildrenCommand : Sitecore.Data.Engines.DataCommands.GetChildrenCommand, IDataEngineCommand
  {
    private readonly ThreadLocal<DataEngineCommand> innerCommand;

    public GetChildrenCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand> { Value = DataEngineCommand.NotInitialized };
    }

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand.Value = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.GetChildrenCommand CreateInstance()
    {
      return this.innerCommand.Value.CreateInstance<Sitecore.Data.Engines.DataCommands.GetChildrenCommand, GetChildrenCommand>();
    }

    protected override ItemList DoExecute()
    {
      var item = this.innerCommand.Value.DataStorage.GetFakeItem(this.Item.ID);
      var itemList = new ItemList();

      if (item == null)
      {
        return itemList;
      }

      var children = item.Children.Select(child => this.innerCommand.Value.DataStorage.GetSitecoreItem(child.ID, this.Item.Language));
      itemList.AddRange(children);

      return itemList;
    }
  }
}