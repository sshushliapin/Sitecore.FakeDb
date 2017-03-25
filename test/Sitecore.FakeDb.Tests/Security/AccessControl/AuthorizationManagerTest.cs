namespace Sitecore.FakeDb.Tests.Security.AccessControl
{
  using FluentAssertions;
  using Sitecore.FakeDb.Security.AccessControl;
  using Sitecore.Security.AccessControl;
  using Xunit;

  public class AuthorizationManagerTest
  {
    [Fact]
    public void ShouldResolveDefaultAuthorizationProvider()
    {
      // act & assert
#pragma warning disable 618
      AuthorizationManager.Provider.Should().BeOfType<FakeAuthorizationProvider>();
#pragma warning restore 618
    }
  }
}