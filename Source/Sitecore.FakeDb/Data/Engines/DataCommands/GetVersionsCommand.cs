namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Collections;
  using Sitecore.Data;

  public class GetVersionsCommand : Sitecore.Data.Engines.DataCommands.GetVersionsCommand
  {
    protected override Sitecore.Data.Engines.DataCommands.GetVersionsCommand CreateInstance()
    {
      return new GetVersionsCommand();
    }

    protected override VersionCollection DoExecute()
    {
      // TODO: Implement versioning.
      return new VersionCollection { new Version(1) };
    }
  }
}