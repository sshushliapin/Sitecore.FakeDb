namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Data;

  public class BlobStreamExistsCommandPrototype : Sitecore.Data.Engines.DataCommands.BlobStreamExistsCommand
  {
    private readonly DataEngineCommand innerCommand;

    public BlobStreamExistsCommandPrototype(Database database)
    {
      this.innerCommand = new DataEngineCommand(database);
    }

    protected override Sitecore.Data.Engines.DataCommands.BlobStreamExistsCommand CreateInstance()
    {
      return new BlobStreamExistsCommand(this.innerCommand.DataStorage);
    }

    protected override bool DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}