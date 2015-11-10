namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Data;

  public class RemoveDataCommandPrototype : Sitecore.Data.Engines.DataCommands.RemoveDataCommand
  {
    private readonly DataEngineCommand innerCommand;

    public RemoveDataCommandPrototype(Database database)
    {
      this.innerCommand = new DataEngineCommand(database);
    }

    protected override Sitecore.Data.Engines.DataCommands.RemoveDataCommand CreateInstance()
    {
      throw new NotImplementedException();
    }

    protected override bool DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}