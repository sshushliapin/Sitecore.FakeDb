namespace Sitecore.FakeDb.NSubstitute.Tests.Pipelines.ReleaseFakeDb
{
  using FluentAssertions;
  using global::NSubstitute;
  using Sitecore.Buckets.Managers;
  using Sitecore.FakeDb.NSubstitute.Pipelines.ReleaseFakeDb;
  using Sitecore.Pipelines;
  using Sitecore.Security.Authentication;
  using Xunit;

  public class ReleaseProviderMockTest
  {
    private readonly ReleaseProviderMock processor;

    public ReleaseProviderMockTest()
    {
      processor = new ReleaseProviderMock();
    }

    [Fact]
    public void ShouldReleaseAuthenticationProvider()
    {
      // arrange
      AuthenticationManager.Provider.Login(null);

      // assert
      this.processor.ReleaseAuthenticationProvider(new PipelineArgs());

      // assert
      AuthenticationManager.Provider.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public void ShouldReleaseBucketProvider()
    {
      // arrange
      BucketManager.Provider.AddSearchTabToItem(null);

      // assert
      this.processor.ReleaseBucketProvider(new PipelineArgs());

      // assert
      BucketManager.Provider.ReceivedCalls().Should().BeEmpty();
    }
  }
}