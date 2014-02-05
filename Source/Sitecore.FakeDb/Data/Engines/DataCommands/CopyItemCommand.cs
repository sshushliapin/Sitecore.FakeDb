namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using Sitecore.Data.Items;

  public class CopyItemCommand : Sitecore.Data.Engines.DataCommands.CopyItemCommand
  {
    protected override Sitecore.Data.Engines.DataCommands.CopyItemCommand CreateInstance()
    {
      return new CopyItemCommand();
    }

    protected override Item DoExecute()
    {
      throw new NotImplementedException();
    }
  }
}