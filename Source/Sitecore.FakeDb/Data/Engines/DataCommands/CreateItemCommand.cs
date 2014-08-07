namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Configuration;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using System.Threading;

  public class CreateItemCommand : Sitecore.Data.Engines.DataCommands.CreateItemCommand, IDataEngineCommand
  {
    private ThreadLocal<DataEngineCommand> innerCommand;

    private ThreadLocal<ItemCreator> itemCreator;

    public CreateItemCommand()
    {
      this.itemCreator = new ThreadLocal<ItemCreator>();

      this.innerCommand = new ThreadLocal<DataEngineCommand>();
      this.innerCommand.Value = DataEngineCommand.NotInitialized;
    }

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand.Value = command;
    }

    public ItemCreator ItemCreator
    {
      get { return this.itemCreator.Value ?? (this.itemCreator.Value = new ItemCreator(this.innerCommand.Value.DataStorage)); }
      set { this.itemCreator.Value = value; }
    }

    protected override Sitecore.Data.Engines.DataCommands.CreateItemCommand CreateInstance()
    {
      return this.innerCommand.Value.CreateInstance<Sitecore.Data.Engines.DataCommands.CreateItemCommand, CreateItemCommand>();
    }

    protected override Item DoExecute()
    {
      return this.ItemCreator.Create(this.ItemName, this.ItemId, this.TemplateId, this.Database, this.Destination);
    }
  }
}