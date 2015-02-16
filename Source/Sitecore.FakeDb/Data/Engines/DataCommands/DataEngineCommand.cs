namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Diagnostics;

  public class DataEngineCommand
  {
    public static readonly DataEngineCommand NotInitialized = new NotInitializedDataEngineCommand();

    private readonly DataStorage dataStorage;

    public DataEngineCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    protected DataEngineCommand()
    {
    }

    public virtual DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    public virtual TBaseCommand CreateInstance<TBaseCommand, TCommand>() where TCommand : TBaseCommand, IDataEngineCommand, new()
    {
      var command = new TCommand();
      command.Initialize(this);

      return command;
    }
  }
}