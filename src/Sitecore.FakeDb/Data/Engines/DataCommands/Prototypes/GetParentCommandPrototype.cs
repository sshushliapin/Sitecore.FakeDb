namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Data;
  using Sitecore.Data.Items;

  public class GetParentCommandPrototype : Sitecore.Data.Engines.DataCommands.GetParentCommand
  {
    private readonly DataEngineCommand innerCommand;

    public GetParentCommandPrototype(Database database)
    {
      this.innerCommand = new DataEngineCommand(database);
    }

    protected override Sitecore.Data.Engines.DataCommands.GetParentCommand CreateInstance()
    {
      return new GetParentCommand(this.innerCommand.DataStorage);
    }

    protected override Item DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}