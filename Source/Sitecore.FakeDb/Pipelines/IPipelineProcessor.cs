namespace Sitecore.FakeDb.Pipelines
{
  using Sitecore.Pipelines;

  public interface IPipelineProcessor
  {
    void Process(PipelineArgs args);
  }
}