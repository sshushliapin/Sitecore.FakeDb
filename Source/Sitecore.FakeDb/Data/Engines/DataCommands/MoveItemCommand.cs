namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;

  public class MoveItemCommand : Sitecore.Data.Engines.DataCommands.MoveItemCommand
  {
    protected override Sitecore.Data.Engines.DataCommands.MoveItemCommand CreateInstance()
    {
      return new MoveItemCommand();
    }

    protected override bool DoExecute()
    {
      throw new NotImplementedException();
    }
  }
}