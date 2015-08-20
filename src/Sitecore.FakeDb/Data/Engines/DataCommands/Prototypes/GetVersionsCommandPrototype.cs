namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Collections;
  using Sitecore.Diagnostics;

  public class GetVersionsCommandPrototype : Sitecore.Data.Engines.DataCommands.GetVersionsCommand, IDataEngineCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    public virtual void Initialize(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.innerCommand.Initialize(dataStorage);
    }

    protected override Sitecore.Data.Engines.DataCommands.GetVersionsCommand CreateInstance()
    {
      return new GetVersionsCommand(this.innerCommand.DataStorage);
    }

    protected override VersionCollection DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}