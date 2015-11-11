namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Data;

  public class SaveItemCommandPrototype : Sitecore.Data.Engines.DataCommands.SaveItemCommand
  {
    private readonly DataEngineCommand innerCommand;

    public SaveItemCommandPrototype(Database database)
    {
      this.innerCommand = new DataEngineCommand(database);
    }

    protected override Sitecore.Data.Engines.DataCommands.SaveItemCommand CreateInstance()
    {
      return new SaveItemCommand(this.innerCommand.DataStorage);
    }

    protected override bool DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}