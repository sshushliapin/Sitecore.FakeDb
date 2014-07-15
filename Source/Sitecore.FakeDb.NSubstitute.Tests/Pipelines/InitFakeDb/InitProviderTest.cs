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