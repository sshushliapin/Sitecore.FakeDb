namespace Sitecore.FakeDb.Tests.Security.Authentication
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Common;
  using Sitecore.Security.Accounts;
  using Sitecore.Security.Authentication;
  using Xunit;
  using Xunit.Extensions;

  public class AuthenticationManagerTest
  {
    private readonly AuthenticationProvider provider;

    public AuthenticationManagerTest()
    {
      this.provider = Substitute.For<AuthenticationProvider>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldLoginUser(bool login)
    {
      // arrange
      var user = Substitute.For<User>("John", true);
      user.Name.Returns("John");

      this.provider.Login(user).Returns(login);

      using (new Switcher<AuthenticationProvider>(this.provider))
      {
        // act & assert
        AuthenticationManager.Login(user).Should().Be(login);
      }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldLoginUserByName(bool login)
    {
      // arrange
      this.provider.Login("John", false).Returns(login);

      using (new Switcher<AuthenticationProvider>(this.provider))
      {
        // act & assert
        AuthenticationManager.Login("John", false).Should().Be(login);
      }
    }

    [Fact]
    public void ShouldLogout()
    {
      // arrange
      using (new Switcher<AuthenticationProvider>(this.provider))
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
      var user = Substitute.For<User>("John", false);
      user.Name.Returns("John");

      using (new Switcher<AuthenticationProvider>(this.provider))
      {
        // act
        AuthenticationManager.SetActiveUser(user);

        // assert
        this.provider.Received().SetActiveUser(user);
      }
    }

    [Fact]
    public void ShouldSetActiveUserByName()
    {
      // arrange
      using (new Switcher<AuthenticationProvider>(this.provider))
      {
        // act
        AuthenticationManager.SetActiveUser("John");

        // assert
        this.provider.Received().SetActiveUser("John");
      }
    }
  }
}