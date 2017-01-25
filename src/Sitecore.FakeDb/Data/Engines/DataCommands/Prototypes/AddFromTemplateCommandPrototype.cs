namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Data;
  using Sitecore.Data.Items;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
  public class AddFromTemplateCommandPrototype : Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand
  {
    private readonly DataEngineCommand innerCommand;

    public AddFromTemplateCommandPrototype(Database database)
    {
      this.innerCommand = new DataEngineCommand(database);
    }

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