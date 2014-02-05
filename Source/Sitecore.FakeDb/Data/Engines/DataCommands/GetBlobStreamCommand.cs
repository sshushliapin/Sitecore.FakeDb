namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using System.IO;

  public class GetBlobStreamCommand : Sitecore.Data.Engines.DataCommands.GetBlobStreamCommand
  {
    protected override Sitecore.Data.Engines.DataCommands.GetBlobStreamCommand CreateInstance()
    {
      return new GetBlobStreamCommand();
    }

    protected override Stream DoExecute()
    {
      throw new NotImplementedException();
    }
  }
}