namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using Sitecore.Common;

  public class DataEngineCommand
  {
    private const string ExceptionText = "Sitecore.FakeDb.Db instance has not been initialized.";

    public virtual DataStorage DataStorage
    {
      get
      {
        var dataStorage = Switcher<DataStorage>.CurrentValue;
        if (dataStorage == null)
        {
          throw new InvalidOperationException(ExceptionText);
        }

        return dataStorage;
      }
    }
  }
}