namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using Sitecore.Data.Events;
  using System.Threading;
  using Sitecore.Data.Items;

  public class CreateItemCommand : Sitecore.Data.Engines.DataCommands.CreateItemCommand, IDataEngineCommand
  {
    private readonly ThreadLocal<DataEngineCommand> innerCommand;

    private readonly ThreadLocal<ItemCreator> itemCreator;

    public CreateItemCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand> { Value = DataEngineCommand.NotInitialized };
      this.itemCreator = new ThreadLocal<ItemCreator>();
    }

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand.Value = command;
    }

    public override Sitecore.Data.Engines.DataCommands.CreateItemCommand Clone(EventHandler<ExecutingEventArgs<Sitecore.Data.Engines.DataCommands.CreateItemCommand>> executingEvent, EventHandler<ExecutedEventArgs<Sitecore.Data.Engines.DataCommands.CreateItemCommand>> executedEvent)
    {
      return base.Clone();
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