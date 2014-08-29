namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Threading;
  using Sitecore.Data.Items;

  public class AddFromTemplateCommand : Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand, IDataEngineCommand
  {
    private readonly ThreadLocal<DataEngineCommand> innerCommand;

    private ThreadLocal<ItemCreator> itemCreator;

    public AddFromTemplateCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand> { Value = DataEngineCommand.NotInitialized };
      this.itemCreator = new ThreadLocal<ItemCreator>();
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

    protected override Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand CreateInstance()
    {
      return this.innerCommand.Value.CreateInstance<Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand, AddFromTemplateCommand>();
    }

    protected override Item DoExecute()
    {
      return this.ItemCreator.Create(this.ItemName, this.NewId, this.TemplateId, this.Database, this.Destination);
    }
  }
}