namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;

  public class BlobStreamExistsCommand : Sitecore.Data.Engines.DataCommands.BlobStreamExistsCommand
  {
    protected override Sitecore.Data.Engines.DataCommands.BlobStreamExistsCommand CreateInstance()
    {
      return new BlobStreamExistsCommand();
    }

    protected override bool DoExecute()
    {
      throw new NotImplementedException();
    }
  }
}