namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using System.Threading;

  public class AddFromTemplateCommand : Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand, IDataEngineCommand
  {
    private ThreadLocal<DataEngineCommand> innerCommand;

    private ThreadLocal<ItemCreator> itemCreator;

    public AddFromTemplateCommand()
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