namespace Sitecore.FakeDb.Tests.Security.AccessControl
{
  using FluentAssertions;
  using Sitecore.FakeDb.Security.AccessControl;
  using Sitecore.Security.AccessControl;
  using Xunit;

  public class AuthorizationManagerTest
  {
    [Fact]
    public void ShouldResolveSwitchingAuthorizationProvider()
    {
      // act & assert
      AuthorizationManager.Provider.Should().BeOfType<SwitchingAuthorizationProvider>();
    }

    [Fact]
    public void ShouldResolveAuthorizationProviderStubAsCurrentProvider()
    {
      // arrange
      var provider = (SwitchingAuthorizationProvider)AuthorizationManager.Provider;

      // act & assert
      provider.CurrentProvider.Should().BeOfType<AuthorizationProviderStub>();
    }
  }
}