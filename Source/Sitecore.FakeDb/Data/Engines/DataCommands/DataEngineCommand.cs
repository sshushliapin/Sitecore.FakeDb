namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  public class DataEngineCommand
  {
    public static readonly DataEngineCommand NotInitialized = new NotInitializedDataEngineCommand();

    internal DataEngineCommand()
    {
    }

    public DataEngineCommand(DataStorage dataStorage)
    {
      this.DataStorage = dataStorage;
    }

    public virtual DataStorage DataStorage { get; private set; }

    public virtual TBaseCommand CreateInstance<TBaseCommand, TCommand>() where TCommand : TBaseCommand, IDataEngineCommand, new()
    {
      var command = new TCommand();
      command.Initialize(this);

      return command;
    }
  }
}