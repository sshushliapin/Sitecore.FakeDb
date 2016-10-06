namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Data;
  using Sitecore.Data.Items;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
  public class CopyItemCommandPrototype : Sitecore.Data.Engines.DataCommands.CopyItemCommand
  {
    private readonly DataEngineCommand innerCommand;

    public CopyItemCommandPrototype(Database database)
    {
      this.innerCommand = new DataEngineCommand(database);
    }

    protected override Sitecore.Data.Engines.DataCommands.CopyItemCommand CreateInstance()
    {
      return new CopyItemCommand(this.innerCommand.DataStorage);
    }

    protected override Item DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}