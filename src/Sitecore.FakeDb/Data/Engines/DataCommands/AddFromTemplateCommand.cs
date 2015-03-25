namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  public class AddFromTemplateCommand : Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand, IDataEngineCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    public virtual void Initialize(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.innerCommand.Initialize(dataStorage);
    }

    protected override Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand, AddFromTemplateCommand>();
    }

    protected override Item DoExecute()
    {
      var dataStorage = this.innerCommand.DataStorage;
      var item = new DbItem(this.ItemName, this.NewId, this.TemplateId) { ParentID = this.Destination.ID };
      dataStorage.AddFakeItem(item);

      return dataStorage.GetSitecoreItem(this.NewId);
    }
  }
}