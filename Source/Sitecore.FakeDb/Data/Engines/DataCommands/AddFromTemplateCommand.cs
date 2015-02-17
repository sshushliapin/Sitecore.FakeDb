namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Items;

  public class AddFromTemplateCommand : Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand, IDataEngineCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    public virtual void Initialize(DataStorage dataStorage)
    {
      this.innerCommand.Initialize(dataStorage);
    }

    protected override Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand, AddFromTemplateCommand>();
    }

    protected override Item DoExecute()
    {
      var dataStorage = this.innerCommand.DataStorage;
      dataStorage.Create(this.ItemName, this.NewId, this.TemplateId, this.Destination, true);
      
      return dataStorage.GetSitecoreItem(this.NewId);
    }
  }
}