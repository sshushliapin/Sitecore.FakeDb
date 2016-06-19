namespace Sitecore.FakeDb.Pipelines.Initialize
{
  using Sitecore.Pipelines;
  using Sitecore.SecurityModel.License;

  public class DisableLicenseWatcher
  {
    public void Process(PipelineArgs args)
    {
      new LicenseWatcher().Dispose();
    }
  }
}