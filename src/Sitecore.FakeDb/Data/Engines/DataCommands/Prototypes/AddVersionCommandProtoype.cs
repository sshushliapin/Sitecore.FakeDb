namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Data;
  using Sitecore.Data.Items;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
  public class AddVersionCommandProtoype : Sitecore.Data.Engines.DataCommands.AddVersionCommand
  {
    private readonly DataEngineCommand innerCommand;

    public AddVersionCommandProtoype(Database database)
    {
      this.innerCommand = new DataEngineCommand(database);
    }

    protected override Sitecore.Data.Engines.DataCommands.AddVersionCommand CreateInstance()
    {
      return new AddVersionCommand(this.innerCommand.DataStorage);
    }

    protected override Item DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}