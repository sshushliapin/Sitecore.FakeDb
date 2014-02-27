namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Diagnostics;

  public class HasChildrenCommand : Sitecore.Data.Engines.DataCommands.HasChildrenCommand
  {
    private readonly DataStorage dataStorage;

    public HasChildrenCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public virtual DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.HasChildrenCommand CreateInstance()
    {
      return new HasChildrenCommand(this.DataStorage);
    }

    protected override bool DoExecute()
    {
      var fakeItem = this.DataStorage.GetFakeItem(Item.ID);

      return fakeItem.Children.Count > 0;
    }
  }
}