namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Threading;
  using Sitecore.Diagnostics;

  public class DataEngineCommand
  {
    public static readonly DataEngineCommand NotInitialized = new NotInitializedDataEngineCommand();

    private readonly ThreadLocal<DataStorage> dataStorageScope = new ThreadLocal<DataStorage>();

    public DataEngineCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorageScope.Value = dataStorage;
    }

    protected DataEngineCommand()
    {
    }

    public virtual DataStorage DataStorage
    {
      get { return this.dataStorageScope.Value; }
    }

    public virtual TBaseCommand CreateInstance<TBaseCommand, TCommand>() where TCommand : TBaseCommand, IDataEngineCommand, new()
    {
      var command = new TCommand();
      command.Initialize(this);

      return command;
    }
  }
}