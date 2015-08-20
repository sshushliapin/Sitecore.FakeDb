namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  public class ResolvePathCommandPrototype : Sitecore.Data.Engines.DataCommands.ResolvePathCommand, IDataEngineCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    public virtual void Initialize(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.innerCommand.Initialize(dataStorage);
    }

    protected override Sitecore.Data.Engines.DataCommands.ResolvePathCommand CreateInstance()
    {
      return new ResolvePathCommand(this.innerCommand.DataStorage);
    }

    protected override ID DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}