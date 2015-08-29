namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Data.Items;

  public class AddVersionCommandProtoype : Sitecore.Data.Engines.DataCommands.AddVersionCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

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