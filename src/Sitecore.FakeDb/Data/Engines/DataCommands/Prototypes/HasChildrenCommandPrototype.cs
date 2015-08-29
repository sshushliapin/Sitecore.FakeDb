namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;

  public class HasChildrenCommandPrototype : Sitecore.Data.Engines.DataCommands.HasChildrenCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    protected override Sitecore.Data.Engines.DataCommands.HasChildrenCommand CreateInstance()
    {
      return new HasChildrenCommand(this.innerCommand.DataStorage);
    }

    protected override bool DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}