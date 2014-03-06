namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Configuration;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  public class CreateItemCommand : Sitecore.Data.Engines.DataCommands.CreateItemCommand, IDataEngineCommand
  {
    private ItemCreator itemCreator;

    private DataEngineCommand innerCommand = DataEngineCommand.NotInitialized;

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand = command;
    }

    public ItemCreator ItemCreator
    {
      get { return this.itemCreator ?? (this.itemCreator = new ItemCreator(this.innerCommand.DataStorage)); }
      set { this.itemCreator = value; }
    }

    protected override Sitecore.Data.Engines.DataCommands.CreateItemCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.CreateItemCommand, CreateItemCommand>();
    }

    protected override Item DoExecute()
    {
      return this.ItemCreator.Create(this.ItemName, this.ItemId, this.TemplateId, this.Database, this.Destination);
    }
  }
}