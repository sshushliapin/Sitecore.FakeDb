namespace Sitecore.FakeDb.Tests.Security.Authentication
{
  using System.Collections.Specialized;
  using FluentAssertions;
  using NSubstitute;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Common;
  using Sitecore.FakeDb.Security.Authentication;
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
      this.provider.DefaultProvider = new FakeAuthenticationProvider();
      this.provider.DefaultProvider.SetActiveUser(this.user);

      try
      {
        var config = new NameValueCollection { { "defaultProvider", "fake" } };

        this.provider.Initialize("switching", config);

        // act & assert
        this.provider.GetActiveUser().Name.Should().Be(this.user.Name);
      }
      finally
      {
        this.provider.DefaultProvider.SetActiveUser((User)null);
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

    [Theory, DefaultSubstituteAutoData]
    public void LoginCallsSwithcedProviderIfNotNull(
      AuthenticationProvider switchedProvider)
    {
      using (new Switcher<AuthenticationProvider>(switchedProvider))
      {
        var sut = new SwitchingAuthenticationProvider();
        sut.Login(this.user);
        switchedProvider.Received().Login(this.user);
      }
    }

    [Theory, DefaultSubstituteAutoData]
    public void LoginCallsDefaultProviderIfNotSwitched(
      AuthenticationProvider defaultProvider)
    {
      var sut = new SwitchingAuthenticationProvider
      {
        DefaultProvider = defaultProvider
      };
      sut.Login(this.user);
      defaultProvider.Received().Login(this.user);
    }

    [Theory, DefaultSubstituteAutoData]
    public void LoginDoesNotThrowIfNoAnyProfiderSet()
    {
      var sut = new SwitchingAuthenticationProvider();
      sut.Login(this.user);
    }

    public void LoginByNameAndPersistentCallsSwithcedProviderIfNotNull(
      AuthenticationProvider switchedProvider,
      string userName,
      bool persistent)
    {
      using (new Switcher<AuthenticationProvider>(switchedProvider))
      {
        var sut = new SwitchingAuthenticationProvider();
        sut.Login(userName, persistent);
        switchedProvider.Received().Login(userName, persistent);
      }
    }

    [Theory, DefaultSubstituteAutoData]
    public void LoginByNameAndPersistentCallsDefaultProviderIfNotSwitched(
      AuthenticationProvider defaultProvider,
      string userName,
      bool persistent)
    {
      var sut = new SwitchingAuthenticationProvider
      {
        DefaultProvider = defaultProvider
      };
      sut.Login(userName, persistent);
      defaultProvider.Received().Login(userName, persistent);
    }

    [Theory, AutoData]
    public void LoginByNameAndPersistentDoesNotThrowIfNoAnyProfiderSet(
      string userName,
      bool persistent)
    {
      var sut = new SwitchingAuthenticationProvider();
      sut.Login(userName, persistent);
    }

    public void LoginByNameAndPasswordCallsSwithcedProviderIfNotNull(
      AuthenticationProvider switchedProvider,
      string userName,
      string password,
      bool persistent)
    {
      using (new Switcher<AuthenticationProvider>(switchedProvider))
      {
        var sut = new SwitchingAuthenticationProvider();
        sut.Login(userName, password, persistent);
        switchedProvider.Received().Login(userName, password, persistent);
      }
    }

    [Theory, DefaultSubstituteAutoData]
    public void LoginByNameAndPasswordCallsDefaultProviderIfNotSwitched(
      AuthenticationProvider defaultProvider,
      string userName,
      string password,
      bool persistent)
    {
      var sut = new SwitchingAuthenticationProvider
      {
        DefaultProvider = defaultProvider
      };
      sut.Login(userName, password, persistent);
      defaultProvider.Received().Login(userName, password, persistent);
    }

    [Theory, AutoData]
    public void LoginByNameAndPasswordDoesNotThrowIfNoAnyProfiderSet(
      string userName,
      string password,
      bool persistent)
    {
      var sut = new SwitchingAuthenticationProvider();
      sut.Login(userName, password, persistent);
    }

    [Theory, DefaultSubstituteAutoData]
    public void LogoutCallsSwithcedProviderIfNotNull(
      AuthenticationProvider switchedProvider)
    {
      using (new Switcher<AuthenticationProvider>(switchedProvider))
      {
        var sut = new SwitchingAuthenticationProvider();
        sut.Logout();
        switchedProvider.Received().Logout();
      }
    }

    [Theory, DefaultSubstituteAutoData]
    public void LogoutCallsDefaultProviderIfNotSwitched(
      AuthenticationProvider defaultProvider)
    {
      var sut = new SwitchingAuthenticationProvider
      {
        DefaultProvider = defaultProvider
      };
      sut.Logout();
      defaultProvider.Received().Logout();
    }

    [Theory, AutoData]
    public void LogoutDoesNotThrowIfNoAnyProfiderSet()
    {
      var sut = new SwitchingAuthenticationProvider();
      sut.Logout();
    }
  }
}