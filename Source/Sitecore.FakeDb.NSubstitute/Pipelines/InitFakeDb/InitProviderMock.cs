namespace Sitecore.FakeDb.NSubstitute.Pipelines.InitFakeDb
{
  using global::NSubstitute;
  using Sitecore.Buckets.Managers;
  using Sitecore.Pipelines;

  public class InitProviderMock
  {
    public virtual void InitBucketProvider(PipelineArgs args)
    {
      BucketManager.Provider.Name.Returns("mock");
    }
  }
}