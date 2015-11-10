namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  public class DataEngineCommand
  {
    private const string ExceptionText = "Sitecore.FakeDb.Db instance has not been initialized.";

    private readonly string databaseName;

    public DataEngineCommand(Database database)
    {
      Assert.IsNotNull(database, "database");

      this.databaseName = database.Name;
    }

    public virtual DataStorage DataStorage
    {
      get
      {
        var dataStorage = DataStorageSwitcher.CurrentValue(this.databaseName);
        if (dataStorage == null)
        {
          throw new InvalidOperationException(ExceptionText);
        }

        return dataStorage;
      }
    }
  }
}