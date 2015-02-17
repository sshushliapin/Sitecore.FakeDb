namespace Sitecore.FakeDb.Pipelines.ReleaseFakeDb
{
  using Sitecore.Configuration;
  using Sitecore.Pipelines;

  public class ResetFactory
  {
    public void Process(PipelineArgs args)
    {
      Factory.Reset();
    }
  }
}