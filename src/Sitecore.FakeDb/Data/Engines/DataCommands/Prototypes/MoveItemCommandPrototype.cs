namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Data;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
  public class MoveItemCommandPrototype : Sitecore.Data.Engines.DataCommands.MoveItemCommand
  {
    private readonly DataEngineCommand innerCommand;

    public MoveItemCommandPrototype(Database database)
    {
      this.innerCommand = new DataEngineCommand(database);
    }

    protected override Sitecore.Data.Engines.DataCommands.MoveItemCommand CreateInstance()
    {
      return new MoveItemCommand(this.innerCommand.DataStorage);
    }

    protected override bool DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}