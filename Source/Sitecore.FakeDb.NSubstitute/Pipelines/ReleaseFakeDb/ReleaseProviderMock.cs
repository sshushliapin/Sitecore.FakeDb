namespace Sitecore.FakeDb.NSubstitute.Pipelines.ReleaseFakeDb
{
  using global::NSubstitute;
  using Sitecore.Buckets.Managers;
  using Sitecore.Pipelines;

  public class ReleaseProviderMock
  {
    public virtual void ReleaseBucketProvider(PipelineArgs args)
    {
      BucketManager.Provider.ClearReceivedCalls();
    }
  }
}