namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
  using System;

  public class RemoveDataCommandPrototype : Sitecore.Data.Engines.DataCommands.RemoveDataCommand
  {
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