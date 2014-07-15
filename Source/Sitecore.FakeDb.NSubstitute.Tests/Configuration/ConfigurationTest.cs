namespace Sitecore.FakeDb.NSubstitute.Tests.Configuration
{
  using FluentAssertions;
  using Sitecore.Buckets.Managers;
  using Sitecore.Configuration;
  using Sitecore.Security.Authentication;
  using Sitecore.StringExtensions;
  using Xunit;
  using Xunit.Extensions;

  public class ConfigurationTest
  {
    [Fact]
    public void ShouldResolveNSubstituteFactory()
    {
      // act & assert
      Factory.CreateObject("factories/factory", true).Should().BeOfType<NSubstituteFactory>();
    }

    [Fact]
    public void ShouldCreateBucketProvider()
    {
      // act & assert
      BucketManager.Provider.Should().BeAssignableTo<BucketProvider>();
      BucketManager.Provider.GetType().FullName.Should().Be("Castle.Proxies.BucketProviderProxy");
    }

    [Theory]
    [InlineData("initFakeDb", "Sitecore.FakeDb.NSubstitute.Pipelines.InitFakeDb.InitProviderMock, Sitecore.FakeDb.NSubstitute", "InitBucketProvider")]
    [InlineData("releaseFakeDb", "Sitecore.FakeDb.NSubstitute.Pipelines.ReleaseFakeDb.ReleaseProviderMock, Sitecore.FakeDb.NSubstitute", "ReleaseBucketProvider")]
    public void ShouldRegisterProcessor(string processor, string type, string method)
    {
      // arrange
      Assert.DoesNotThrow(() => Factory.CreateObject("pipelines/{0}/processor[@type='{1}' and @method='{2}']".FormatWith(processor, type, method), true));
    }
  }
}