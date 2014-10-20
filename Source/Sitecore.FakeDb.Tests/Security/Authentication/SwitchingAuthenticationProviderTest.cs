namespace Sitecore.FakeDb.Tests.Security.Authentication
{
  using System.Collections.Specialized;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Security.Accounts;
  using Sitecore.Security.Authentication;
  using Xunit;
  using SwitchingAuthenticationProvider = Sitecore.FakeDb.Security.Authentication.SwitchingAuthenticationProvider;

  public class SwitchingAuthenticationProviderTest
  {
    private readonly SwitchingAuthenticationProvider provider;

    private readonly User user;

    public SwitchingAuthenticationProviderTest()
    {
      this.provider = new SwitchingAuthenticationProvider();

      this.user = User.FromName(@"extranet\Rambo", false);
    }

    [Fact]
    public void ShouldGetActiveUserFromCurrentProvider()
    {
      // arrange
      var currentProvider = Substitute.For<AuthenticationProvider>();
      currentProvider.GetActiveUser().Returns(this.user);

      using (new AuthenticationSwitcher(currentProvider))
      {
        // act & assert
        this.provider.GetActiveUser().Should().BeSameAs(this.user);
      }
    }

    [Fact]
    public void ShouldGetActiveUserFromDefaultProviderIfNoCurrentProviderSet()
    {
      // arrange
      AuthenticationManager.Providers["fake"].SetActiveUser(this.user);

      try
      {
        var config = new NameValueCollection { { "defaultProvider", "fake" } };

        this.provider.Initialize("switching", config);

        // act & assert
        this.provider.GetActiveUser().Name.Should().Be(this.user.Name);
      }
      finally
      {
        AuthenticationManager.Providers["fake"].SetActiveUser((User)null);
      }
    }

    [Fact]
    public void ShouldGetDefaultUser()
    {
      // act & assert
      this.provider.GetActiveUser().Name.Should().Be(@"default\Anonymous");
    }

    [Fact]
    public void ShouldGetDefaultUserIfNoDefaultProviderExists()
    {
      // arrange
      var config = new NameValueCollection { { "defaultProvider", "some_missing_provider" } };
      this.provider.Initialize("switching", config);

      // act & assert
      this.provider.GetActiveUser().Name.Should().Be(@"default\Anonymous");
    }
  }
}