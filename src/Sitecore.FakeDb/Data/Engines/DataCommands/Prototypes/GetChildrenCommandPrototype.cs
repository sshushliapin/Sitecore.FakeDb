namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Collections;
  using Sitecore.Data;

  public class GetChildrenCommandPrototype : Sitecore.Data.Engines.DataCommands.GetChildrenCommand
  {
    private readonly DataEngineCommand innerCommand;

    public GetChildrenCommandPrototype(Database database)
    {
      this.innerCommand = new DataEngineCommand(database);
    }

    protected override Sitecore.Data.Engines.DataCommands.GetChildrenCommand CreateInstance()
    {
      return new GetChildrenCommand(this.innerCommand.DataStorage);
    }

    protected override ItemList DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}