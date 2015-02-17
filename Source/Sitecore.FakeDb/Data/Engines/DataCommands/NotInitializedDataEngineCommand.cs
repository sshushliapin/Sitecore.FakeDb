namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;









  public class NotInitializedDataEngineCommand : DataEngineCommand
  {
    private const string ExceptionText = "Sitecore.FakeDb.Db instance has not been initialized.";

    public override DataStorage DataStorage
    {
      get { throw new InvalidOperationException(ExceptionText); }
    }

    public override TBaseCommand CreateInstance<TBaseCommand, TCommand>()
    {
      throw new InvalidOperationException(ExceptionText);
    }
  }
}