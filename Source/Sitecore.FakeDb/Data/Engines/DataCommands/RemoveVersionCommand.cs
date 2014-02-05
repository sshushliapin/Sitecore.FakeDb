namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;

  public class RemoveVersionCommand : Sitecore.Data.Engines.DataCommands.RemoveVersionCommand
  {
    protected override Sitecore.Data.Engines.DataCommands.RemoveVersionCommand CreateInstance()
    {
      return new RemoveVersionCommand();
    }

    protected override bool DoExecute()
    {
      throw new NotImplementedException();
    }
  }
}