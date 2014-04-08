namespace Sitecore.FakeDb.NSubstitute.Pipelines.InitFakeDb
{
  using global::NSubstitute;
  using Sitecore.Pipelines;
  using Sitecore.Security.Accounts;
  using Sitecore.Security.Authentication;

  public class InitAuthenticationProvider
  {
    public void Process(PipelineArgs args)
    {
      var user = User.FromName(@"sitecore\Anonymous", false);

      AuthenticationManager.Provider.GetActiveUser().Returns(user);
    }
  }
}