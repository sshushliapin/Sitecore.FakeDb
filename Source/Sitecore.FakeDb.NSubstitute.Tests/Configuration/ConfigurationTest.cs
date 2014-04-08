namespace Sitecore.FakeDb.NSubstitute.Tests.Configuration
{
  using FluentAssertions;
  using Sitecore.Buckets.Managers;
  using Sitecore.Configuration;
  using Sitecore.Security.AccessControl;
  using Sitecore.Security.Authentication;
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
    public void ShouldCreateAuthenticationManagerProviderMock()
    {
      // act & assert
      AuthenticationManager.Provider.Should().BeAssignableTo<AuthenticationProvider>();
      AuthenticationManager.Provider.GetType().FullName.Should().Be("Castle.Proxies.AuthenticationProviderProxy");
    }

    [Fact]
    public void ShouldCreateBucketProvider()
    {
      // act & assert
      BucketManager.Provider.Should().BeAssignableTo<BucketProvider>();
      BucketManager.Provider.GetType().FullName.Should().Be("Castle.Proxies.BucketProviderProxy");
    }

    [Fact]
    public void ShouldRegisterInitFakeDbProcessor()
    {
      // arrange
      Assert.DoesNotThrow(() => Factory.CreateObject("pipelines/initFakeDb/processor[@type='Sitecore.FakeDb.NSubstitute.Pipelines.InitFakeDb.InitAuthenticationProvider, Sitecore.FakeDb.NSubstitute']", true));
    }
  }
}