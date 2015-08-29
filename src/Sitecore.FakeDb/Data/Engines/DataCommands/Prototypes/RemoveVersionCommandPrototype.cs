namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;

  public class RemoveVersionCommandPrototype : Sitecore.Data.Engines.DataCommands.RemoveVersionCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    protected override Sitecore.Data.Engines.DataCommands.RemoveVersionCommand CreateInstance()
    {
      return new RemoveVersionCommand(this.innerCommand.DataStorage);
    }

    protected override bool DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}