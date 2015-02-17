namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Diagnostics;

  public class HasChildrenCommand : Sitecore.Data.Engines.DataCommands.HasChildrenCommand, IDataEngineCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    public virtual void Initialize(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.innerCommand.Initialize(dataStorage);
    }

    protected override Sitecore.Data.Engines.DataCommands.HasChildrenCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.HasChildrenCommand, HasChildrenCommand>();
    }

    protected override bool DoExecute()
    {
      var fakeItem = this.innerCommand.DataStorage.GetFakeItem(Item.ID);

      return fakeItem.Children.Count > 0;
    }
  }
}