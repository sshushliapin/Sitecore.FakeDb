namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Data.Items;

  public class AddFromTemplateCommandPrototype : Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    protected override Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand CreateInstance()
    {
      return new AddFromTemplateCommand(this.innerCommand.DataStorage);
    }

    protected override Item DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}