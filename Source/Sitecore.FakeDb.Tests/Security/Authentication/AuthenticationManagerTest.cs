namespace Sitecore.FakeDb.Tests.Security.Authentication
{
  using FluentAssertions;
  using Sitecore.Security.Authentication;
  using Xunit;

  public class AuthenticationManagerTest
  {
    [Fact]
    public void ShouldLogin()
    {
      // arrange
      AuthenticationManager.Login("admin").Should().BeTrue();
    }
  }
}