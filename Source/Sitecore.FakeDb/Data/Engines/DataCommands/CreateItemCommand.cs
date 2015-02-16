namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using System.Threading;
  using Sitecore.Data.Events;
  using Sitecore.Data.Items;

  public class CreateItemCommand : Sitecore.Data.Engines.DataCommands.CreateItemCommand, IDataEngineCommand
  {
    private readonly ThreadLocal<DataEngineCommand> innerCommand;

    public CreateItemCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand> { Value = DataEngineCommand.NotInitialized };
    }

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand.Value = command;
    }

    public override Sitecore.Data.Engines.DataCommands.CreateItemCommand Clone(EventHandler<ExecutingEventArgs<Sitecore.Data.Engines.DataCommands.CreateItemCommand>> executingEvent, EventHandler<ExecutedEventArgs<Sitecore.Data.Engines.DataCommands.CreateItemCommand>> executedEvent)
    {
      return base.Clone();
    }

    protected override Sitecore.Data.Engines.DataCommands.CreateItemCommand CreateInstance()
    {
      return this.innerCommand.Value.CreateInstance<Sitecore.Data.Engines.DataCommands.CreateItemCommand, CreateItemCommand>();
    }

    protected override Item DoExecute()
    {
      var dataStorage = this.innerCommand.Value.DataStorage;
      dataStorage.Create(this.ItemName, this.ItemId, this.TemplateId, this.Destination);

      return dataStorage.GetSitecoreItem(this.ItemId);
    }
  }
}