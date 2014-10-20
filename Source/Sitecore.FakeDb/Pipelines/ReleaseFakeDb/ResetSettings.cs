namespace Sitecore.FakeDb.Pipelines.ReleaseFakeDb
{
  using Sitecore.Configuration;
  using Sitecore.Pipelines;

  public class ResetSettings
  {
    public void Process(PipelineArgs args)
    {
      Settings.Reset();
    }
  }
}