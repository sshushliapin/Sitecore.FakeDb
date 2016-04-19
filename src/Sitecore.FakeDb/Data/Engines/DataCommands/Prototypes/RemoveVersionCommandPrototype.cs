namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Data;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
  public class RemoveVersionCommandPrototype : Sitecore.Data.Engines.DataCommands.RemoveVersionCommand
  {
    private readonly DataEngineCommand innerCommand;

    public RemoveVersionCommandPrototype(Database database)
    {
      this.innerCommand = new DataEngineCommand(database);
    }

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