namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  public class CreateItemCommand : Sitecore.Data.Engines.DataCommands.CreateItemCommand, IDataEngineCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    public virtual void Initialize(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.innerCommand.Initialize(dataStorage);
    }

    protected override Sitecore.Data.Engines.DataCommands.CreateItemCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.CreateItemCommand, CreateItemCommand>();
    }

    protected override Item DoExecute()
    {
      var dataStorage = this.innerCommand.DataStorage;
      var item = new DbItem(this.ItemName, this.ItemId, this.TemplateId) { ParentID = this.Destination.ID };
      dataStorage.AddFakeItem(item);
      item.VersionsCount.Clear();

      return dataStorage.GetSitecoreItem(this.ItemId);
    }
  }
}