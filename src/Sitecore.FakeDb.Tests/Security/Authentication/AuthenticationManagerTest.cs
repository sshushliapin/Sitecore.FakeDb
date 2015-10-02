namespace Sitecore.FakeDb.Tests.Security.Authentication
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Security.Accounts;
  using Sitecore.Security.Authentication;
  using Xunit;

  public class AuthenticationManagerTest
  {
    private readonly AuthenticationProvider provider;

    private readonly User user;

    public AuthenticationManagerTest()
    {
      this.provider = Substitute.For<AuthenticationProvider>();

      this.user = Substitute.For<User>("John", false);
      this.user.Name.Returns("John");
    }

    [Fact]
    public void ShouldGetActiveUser()
    {
      // arrange
      this.provider.GetActiveUser().Returns(this.user);

      using (new AuthenticationSwitcher(this.provider))
      {
        // act & assert
        AuthenticationManager.GetActiveUser().Should().Be(this.user);
      }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldLoginUser(bool login)
    {
      // arrange
      this.provider.Login(this.user).Returns(login);

      using (new AuthenticationSwitcher(this.provider))
      {
        // act & assert
        AuthenticationManager.Login(this.user).Should().Be(login);
      }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldLoginUserByName(bool login)
    {
      // arrange
      this.provider.Login("John", false).Returns(login);

      using (new AuthenticationSwitcher(this.provider))
      {
        // act & assert
        AuthenticationManager.Login("John", false).Should().Be(login);
      }
    }

    [Fact]
    public void ShouldLogout()
    {
      // arrange
      this.provider.GetActiveUser().Returns(this.user);

      using (new AuthenticationSwitcher(this.provider))
      {
        // act
        AuthenticationManager.Logout();

        // assert
        this.provider.Received().Logout();
      }
    }

    [Fact]
    public void ShouldSetActiveUser()
    {
      // arrange
      using (new AuthenticationSwitcher(this.provider))
      {
        // act
        AuthenticationManager.SetActiveUser(this.user);

        // assert
        this.provider.Received().SetActiveUser(this.user);
      }
    }

    [Fact]
    public void ShouldSetActiveUserByName()
    {
      // arrange
      using (new AuthenticationSwitcher(this.provider))
      {
        // act
        AuthenticationManager.SetActiveUser("John");

        // assert
        this.provider.Received().SetActiveUser("John");
      }
    }
  }
}