namespace Sitecore.FakeDb.Tests.Security.Authentication
{
  using System;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.FakeDb.Security.Authentication;
  using Sitecore.Security.Accounts;
  using Sitecore.Security.Authentication;
  using Xunit;

  public class FakeAuthenticationProviderTest : IDisposable
  {
    private const string UserName = @"extrante\Rambo";

    private readonly FakeAuthenticationProvider provider;

    private readonly AuthenticationProvider localProvider;

    private readonly User user;

    public FakeAuthenticationProviderTest()
    {
      this.provider = new FakeAuthenticationProvider();
      this.localProvider = Substitute.For<AuthenticationProvider>();
      this.user = User.FromName(UserName, false);
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
      // act & assert
      this.provider.SetActiveUser(this.user);
      this.provider.GetActiveUser().Should().BeSameAs(this.user);
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

      this.provider.LocalProvider.Value = behaviour;

      // act & assert
      this.provider.Login(UserName, true).Should().Be(true);
    }

    #region Local provider

    [Fact]
    public void ShouldBuildVirtualUserUsingLocalProvider()
    {
      // arrange
      this.localProvider.BuildVirtualUser(UserName, true).Returns(this.user);
      this.provider.LocalProvider.Value = this.localProvider;

      // act & assert
      this.provider.BuildVirtualUser(UserName, true).Should().Be(this.user);
    }

    [Fact]
    public void ShouldCheckLegacyPasswordUsingLocalProvider()
    {
      // arrange
      this.localProvider.CheckLegacyPassword(this.user, "******").Returns(true);
      this.provider.LocalProvider.Value = this.localProvider;

      // act & assert
      this.provider.CheckLegacyPassword(this.user, "******").Should().BeTrue();
    }

    [Fact]
    public void ShouldGetActiveUserUsingLocalProvider()
    {
      // arrange
      this.localProvider.GetActiveUser().Returns(this.user);
      this.provider.LocalProvider.Value = this.localProvider;

      // act & assert
      this.provider.GetActiveUser().Should().Be(this.user);
    }

    [Fact]
    public void ShouldLoginUsingLocalProvider()
    {
      // arrange
      this.localProvider.Login(this.user).Returns(true);
      this.provider.LocalProvider.Value = this.localProvider;

      // act & assert
      this.provider.Login(this.user).Should().BeTrue();
    }

    [Fact]
    public void ShouldLoginByNameUsingLocalProvider()
    {
      // arrange
      this.localProvider.Login(UserName, true).Returns(true);
      this.provider.LocalProvider.Value = this.localProvider;

      // act & assert
      this.provider.Login(UserName, true).Should().BeTrue();
    }

    [Fact]
    public void ShouldLoginByNameAndPasswordUsingLocalProvider()
    {
      // arrange
      this.localProvider.Login(UserName, "******", true).Returns(true);
      this.provider.LocalProvider.Value = this.localProvider;

      // act & assert
      this.provider.Login(UserName, "******", true).Should().BeTrue();
    }

    [Fact]
    public void ShouldLogoutUsingLocalProvider()
    {
      // arrange
      this.provider.LocalProvider.Value = this.localProvider;

      // act
      this.provider.Logout();

      // assert
      this.localProvider.Received().Logout();
    }

    [Fact]
    public void ShouldSetActiveUserInLocalProvider()
    {
      // arrange
      this.provider.LocalProvider.Value = this.localProvider;

      // act
      this.provider.SetActiveUser(this.user);

      // assert
      this.localProvider.Received().SetActiveUser(this.user);
    }

    [Fact]
    public void ShouldSetActiveUserInLocalProviderByName()
    {
      // arrange
      this.provider.LocalProvider.Value = this.localProvider;

      // act
      this.provider.SetActiveUser(UserName);

      // assert
      this.localProvider.Received().SetActiveUser(UserName);
    }

    #endregion

    public void Dispose()
    {
      this.provider.Dispose();
    }
  }
}