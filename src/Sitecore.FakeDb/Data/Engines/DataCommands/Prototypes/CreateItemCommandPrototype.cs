namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Data;
  using Sitecore.Data.Items;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
  public class CreateItemCommandPrototype : Sitecore.Data.Engines.DataCommands.CreateItemCommand
  {
    private readonly DataEngineCommand innerCommand;

    public CreateItemCommandPrototype(Database database)
    {
      this.innerCommand = new DataEngineCommand(database);
    }

    protected override Sitecore.Data.Engines.DataCommands.CreateItemCommand CreateInstance()
    {
      return new CreateItemCommand(this.innerCommand.DataStorage);
    }

    protected override Item DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}