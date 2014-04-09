namespace Sitecore.FakeDb.NSubstitute.Tests.Pipelines.InitFakeDb
{
  using FluentAssertions;
  using Sitecore.Buckets.Managers;
  using Sitecore.FakeDb.NSubstitute.Pipelines.InitFakeDb;
  using Sitecore.Pipelines;
  using Sitecore.Security.Authentication;
  using Xunit;

  public class InitAuthenticationProviderTest
  {
    [Fact]
    public void ShouldInitAuthenticationProvider()
    {
      // arrange
      var processor = new InitProviderMock();

      // act
      processor.InitAuthenticationProvider(new PipelineArgs());

      // assert
      AuthenticationManager.Provider.Name.Should().Be("mock");
      AuthenticationManager.Provider.GetActiveUser().Name.Should().Be(@"sitecore\Anonymous");
    }

    [Fact]
    public void ShouldInitBucketProvider()
    {
      // arrange
      var processor = new InitProviderMock();

      // act
      processor.InitBucketProvider(new PipelineArgs());

      // assert
      BucketManager.Provider.Name.Should().Be("mock");
    }
  }
}