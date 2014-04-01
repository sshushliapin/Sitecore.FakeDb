namespace Sitecore.FakeDb.NSubstitute.Tests.Configuration
{
  using FluentAssertions;
  using Sitecore.Configuration;
  using Sitecore.Security.AccessControl;
  using Xunit;

  public class ConfigurationTest
  {
    [Fact]
    public void ShouldResolveNSubstituteFactory()
    {
      // act & assert
      Factory.CreateObject("factories/factory", true).Should().BeOfType<NSubstituteFactory>();
    }

    [Fact]
    public void ShouldCreateAuthorizationProviderMock()
    {
      // act & assert
      AuthorizationManager.Provider.Should().BeAssignableTo<AuthorizationProvider>();
    }
  }
}