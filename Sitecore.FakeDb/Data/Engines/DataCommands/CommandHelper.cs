namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Engines;

  public static class CommandHelper
  {
    public static DataStorage GetDataStorage<TReturnType, TCommandType>(DataEngineCommand<TReturnType, TCommandType> command)
      where TCommandType : DataEngineCommand<TReturnType, TCommandType>, new()
    {
      var database = (FakeDatabase)command.Database;
      return database.DataStorage;
    }
  }
}