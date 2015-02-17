namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using System.Threading;
  using Sitecore.Data.Events;
  using Sitecore.Data.Items;

  public class CreateItemCommand : Sitecore.Data.Engines.DataCommands.CreateItemCommand, IDataEngineCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    public virtual void Initialize(DataStorage dataStorage)
    {
      this.innerCommand.Initialize(dataStorage);
    }

    public override Sitecore.Data.Engines.DataCommands.CreateItemCommand Clone(EventHandler<ExecutingEventArgs<Sitecore.Data.Engines.DataCommands.CreateItemCommand>> executingEvent, EventHandler<ExecutedEventArgs<Sitecore.Data.Engines.DataCommands.CreateItemCommand>> executedEvent)
    {
      return base.Clone();
    }

    protected override Sitecore.Data.Engines.DataCommands.CreateItemCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.CreateItemCommand, CreateItemCommand>();
    }

    protected override Item DoExecute()
    {
      var dataStorage = this.innerCommand.DataStorage;
      dataStorage.Create(this.ItemName, this.ItemId, this.TemplateId, this.Destination);

      return dataStorage.GetSitecoreItem(this.ItemId);
    }
  }
}