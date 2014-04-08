namespace Examples
{
  using global::NSubstitute;
  using Xunit;

  public class GettingStarted
  {
    [Fact]
    public void HowDoIMockAuthenticationProvider()
    {
      // the authentication provider is a mock created by NSubstitute;
      // the Login() method should return 'true' when it is called with parameters 'John' and 'true'
      Sitecore.Security.Authentication.AuthenticationManager.Provider.Login("John", true).Returns(true);

      // the authentication manager is called with the expected parameters. It returns 'true'
      Assert.True(Sitecore.Security.Authentication.AuthenticationManager.Login("John", true));

      // the authentication manager is called with some unexpected parameters. It returns 'false'
      Assert.False(Sitecore.Security.Authentication.AuthenticationManager.Login("Robber", true));
    }

    [Fact]
    public void HowDoIMockBucketProvider()
    {
      // act
      Sitecore.Buckets.Managers.BucketManager.AddSearchTabToItem(null);

      // assert
      Sitecore.Buckets.Managers.BucketManager.Provider.Received().AddSearchTabToItem(null);
    }
  }
}