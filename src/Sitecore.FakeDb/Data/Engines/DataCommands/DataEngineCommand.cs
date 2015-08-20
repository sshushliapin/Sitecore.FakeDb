namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using System.Threading;
  using Sitecore.Diagnostics;

  public class DataEngineCommand
  {
    private const string ExceptionText = "Sitecore.FakeDb.Db instance has not been initialized.";

    private readonly ThreadLocal<DataStorage> dataStorageScope = new ThreadLocal<DataStorage>();

    public void Initialize(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorageScope.Value = dataStorage;
    }

    public virtual DataStorage DataStorage
    {
      get
      {
        if (this.dataStorageScope.Value == null)
        {
          throw new InvalidOperationException(ExceptionText);
        }

        return this.dataStorageScope.Value;
      }
    }
  }
}