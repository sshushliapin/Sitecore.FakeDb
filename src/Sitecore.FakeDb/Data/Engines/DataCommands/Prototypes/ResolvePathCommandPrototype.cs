namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Data;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
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