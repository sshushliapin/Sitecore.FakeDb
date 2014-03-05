namespace Sitecore.FakeDb.Pipelines.InitFakeDb
{
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.Pipelines;

  public class InitDbArgs : PipelineArgs
  {
    public InitDbArgs(Database database, DataStorage dataStorage)
    {
      this.Database = database;
      this.DataStorage = dataStorage;
    }

    public Database Database { get; private set; }

    public DataStorage DataStorage { get; private set; }
  }
}