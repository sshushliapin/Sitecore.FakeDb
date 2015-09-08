namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;

  public class DeleteItemCommandPrototype : Sitecore.Data.Engines.DataCommands.DeleteItemCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    protected override Sitecore.Data.Engines.DataCommands.DeleteItemCommand CreateInstance()
    {
      return new DeleteItemCommand(this.innerCommand.DataStorage);
    }

    protected override bool DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}