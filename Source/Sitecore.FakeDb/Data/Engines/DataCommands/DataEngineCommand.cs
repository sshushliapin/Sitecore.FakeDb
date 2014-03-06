namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  public class DataEngineCommand
  {
    private static DataEngineCommand notInitialized = new NotInitializedDataEngineCommand();

    internal DataEngineCommand()
    {
    }

    public DataEngineCommand(DataStorage dataStorage)
    {
      this.DataStorage = dataStorage;
    }

    public static DataEngineCommand NotInitialized
    {
      get { return notInitialized; }
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