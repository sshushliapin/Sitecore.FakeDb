namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Collections;

  public class GetVersionsCommandPrototype : Sitecore.Data.Engines.DataCommands.GetVersionsCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    protected override Sitecore.Data.Engines.DataCommands.GetVersionsCommand CreateInstance()
    {
      return new GetVersionsCommand(this.innerCommand.DataStorage);
    }

    protected override VersionCollection DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}