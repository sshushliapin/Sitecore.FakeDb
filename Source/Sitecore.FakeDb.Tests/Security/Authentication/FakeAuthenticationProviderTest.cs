namespace Sitecore.FakeDb.Tests.Security.Authentication
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.FakeDb.Security.Authentication;
  using Sitecore.Security.Accounts;
  using Sitecore.Security.Authentication;
  using Xunit;

  public class FakeAuthenticationProviderTest
  {
    private const string UserName = @"extrante\Rambo";

    private readonly FakeAuthenticationProvider provider;

    public FakeAuthenticationProviderTest()
    {
      this.provider = new FakeAuthenticationProvider();
    }

    [Fact]
    public void ShouldGetDefaultValuesIfNoBehaviourSet()
    {
      // act & assert
      this.provider.BuildVirtualUser(null, false).Name.Should().Be("default\\Virtual");
      this.provider.CheckLegacyPassword(null, null).Should().BeFalse();
      this.provider.GetActiveUser().Name.Should().Be("default\\Anonymous");

      this.provider.Login(null).Should().BeFalse();
      this.provider.Login(null, false).Should().BeFalse();
      this.provider.Login(null, true).Should().BeFalse();
      this.provider.Login(null, null, true).Should().BeFalse();

      this.provider.Logout();
    }

    [Fact]
    public void ShouldGetTheSameDefaultVirtualUserInstance()
    {
      // act & assert
      this.provider.BuildVirtualUser(null, false).Should().BeSameAs(this.provider.BuildVirtualUser(null, true));
    }

    [Fact]
    public void ShouldGetTheSameDefaultActiveUserInstance()
    {
      // act & assert
      this.provider.GetActiveUser().Should().BeSameAs(this.provider.GetActiveUser());
    }

    [Fact]
    public void ShouldSetAndGetActiveUser()
    {
      // arrange
      var user = User.FromName(UserName, false);

      // act & assert
      this.provider.SetActiveUser(user);
      this.provider.GetActiveUser().Should().BeSameAs(user);
    }

    [Fact]
    public void ShouldSetAndGetActiveUserByName()
    {
      // act & assert
      this.provider.SetActiveUser(UserName);
      this.provider.GetActiveUser().Name.Should().Be(UserName);
    }

    [Fact]
    public void ShouldCallLogin()
    {
      // arrange
      var behaviour = Substitute.For<AuthenticationProvider>();
      behaviour.Login(UserName, true).Returns(true);

      this.provider.Behavior = behaviour;

      // act & assert
      this.provider.Login(UserName, true).Should().Be(true);
    }
  }
}