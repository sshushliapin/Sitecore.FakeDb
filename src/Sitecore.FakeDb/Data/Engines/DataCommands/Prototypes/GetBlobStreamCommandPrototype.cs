namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using System.IO;
  using Sitecore.Data;

  public class GetBlobStreamCommandPrototype : Sitecore.Data.Engines.DataCommands.GetBlobStreamCommand
  {
    private readonly DataEngineCommand innerCommand;

    public GetBlobStreamCommandPrototype(Database database)
    {
      this.innerCommand = new DataEngineCommand(database);
    }

    protected override Sitecore.Data.Engines.DataCommands.GetBlobStreamCommand CreateInstance()
    {
      return new GetBlobStreamCommand(this.innerCommand.DataStorage);
    }

    protected override Stream DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}