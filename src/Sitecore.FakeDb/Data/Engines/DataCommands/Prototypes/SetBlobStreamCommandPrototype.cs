namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Data;

  public class SetBlobStreamCommandPrototype : Sitecore.Data.Engines.DataCommands.SetBlobStreamCommand
  {
    private readonly DataEngineCommand innerCommand;

    public SetBlobStreamCommandPrototype(Database database)
    {
      this.innerCommand = new DataEngineCommand(database);
    }

    protected override Sitecore.Data.Engines.DataCommands.SetBlobStreamCommand CreateInstance()
    {
      return new SetBlobStreamCommand(this.innerCommand.DataStorage);
    }

    protected override bool DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}