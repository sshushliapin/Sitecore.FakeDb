namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Collections;
  using Sitecore.Diagnostics;

  public class GetChildrenCommandPrototype : Sitecore.Data.Engines.DataCommands.GetChildrenCommand, IDataEngineCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    public virtual void Initialize(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.innerCommand.Initialize(dataStorage);
    }

    protected override Sitecore.Data.Engines.DataCommands.GetChildrenCommand CreateInstance()
    {
      return new GetChildrenCommand(this.innerCommand.DataStorage);
    }

    protected override ItemList DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}