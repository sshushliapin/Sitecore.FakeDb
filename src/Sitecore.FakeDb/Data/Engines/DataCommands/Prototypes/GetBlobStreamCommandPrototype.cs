namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using System.IO;
  using Sitecore.Diagnostics;

  public class GetBlobStreamCommandPrototype : Sitecore.Data.Engines.DataCommands.GetBlobStreamCommand, IDataEngineCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    public virtual void Initialize(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.innerCommand.Initialize(dataStorage);
    }

    protected override Sitecore.Data.Engines.DataCommands.GetBlobStreamCommand CreateInstance()
    {
      return new GetBlobStreamCommand(this.innerCommand.DataStorage);
    }

    protected override Stream DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}