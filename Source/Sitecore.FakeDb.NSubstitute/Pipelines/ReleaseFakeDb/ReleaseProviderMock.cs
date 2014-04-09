namespace Sitecore.FakeDb.NSubstitute.Pipelines.ReleaseFakeDb
{
  using global::NSubstitute;
  using Sitecore.Buckets.Managers;
  using Sitecore.Pipelines;
  using Sitecore.Security.Authentication;

  public class ReleaseProviderMock
  {
    public virtual void ReleaseAuthenticationProvider(PipelineArgs args)
    {
      AuthenticationManager.Provider.ClearReceivedCalls();
    }

    public virtual void ReleaseBucketProvider(PipelineArgs args)
    {
      BucketManager.Provider.ClearReceivedCalls();
    }
  }
}