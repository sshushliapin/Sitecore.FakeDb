namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;

  public class BlobStreamExistsCommandPrototype : Sitecore.Data.Engines.DataCommands.BlobStreamExistsCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

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