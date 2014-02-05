namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;

  public class SetBlobStreamCommand : Sitecore.Data.Engines.DataCommands.SetBlobStreamCommand
  {
    protected override Sitecore.Data.Engines.DataCommands.SetBlobStreamCommand CreateInstance()
    {
      return new SetBlobStreamCommand();
    }

    protected override bool DoExecute()
    {
      throw new NotImplementedException();
    }
  }
}