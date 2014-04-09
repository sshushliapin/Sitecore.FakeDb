namespace Sitecore.FakeDb.NSubstitute.Pipelines.InitFakeDb
{
  using global::NSubstitute;
  using Sitecore.Buckets.Managers;
  using Sitecore.Pipelines;
  using Sitecore.Security.Accounts;
  using Sitecore.Security.Authentication;

  public class InitProviderMock
  {
    public virtual void InitAuthenticationProvider(PipelineArgs args)
    {
      AuthenticationManager.Provider.Name.Returns("mock");

      var user = User.FromName(@"sitecore\Anonymous", false);
      AuthenticationManager.Provider.GetActiveUser().Returns(user);
    }

    public virtual void InitBucketProvider(PipelineArgs args)
    {
      BucketManager.Provider.Name.Returns("mock");
    }
  }
}