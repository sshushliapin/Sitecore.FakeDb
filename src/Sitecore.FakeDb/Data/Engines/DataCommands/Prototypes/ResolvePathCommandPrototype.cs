namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Data;

  public class ResolvePathCommandPrototype : Sitecore.Data.Engines.DataCommands.ResolvePathCommand
  {
    private readonly DataEngineCommand innerCommand;

    public ResolvePathCommandPrototype(Database database)
    {
      this.innerCommand = new DataEngineCommand(database);
    }

    protected override Sitecore.Data.Engines.DataCommands.ResolvePathCommand CreateInstance()
    {
      return new ResolvePathCommand(this.innerCommand.DataStorage);
    }

    protected override ID DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}