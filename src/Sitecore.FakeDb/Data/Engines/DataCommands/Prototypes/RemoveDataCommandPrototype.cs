namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;
  using Sitecore.Data;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
  public class RemoveDataCommandPrototype : Sitecore.Data.Engines.DataCommands.RemoveDataCommand
  {
    public RemoveDataCommandPrototype(Database database)
    {
    }

    protected override Sitecore.Data.Engines.DataCommands.RemoveDataCommand CreateInstance()
    {
      throw new NotImplementedException();
    }

    protected override bool DoExecute()
    {
      throw new NotSupportedException();
    }
  }
}