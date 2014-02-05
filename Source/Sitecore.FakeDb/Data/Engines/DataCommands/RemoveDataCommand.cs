namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;

  public class RemoveDataCommand : Sitecore.Data.Engines.DataCommands.RemoveDataCommand
  {
    protected override Sitecore.Data.Engines.DataCommands.RemoveDataCommand CreateInstance()
    {
      return new RemoveDataCommand();
    }

    protected override bool DoExecute()
    {
      throw new NotImplementedException();
    }
  }
}