namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Collections;
  using Sitecore.Data;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
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