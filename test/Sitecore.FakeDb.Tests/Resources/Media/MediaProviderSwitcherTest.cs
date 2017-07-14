namespace Sitecore.FakeDb.Tests.Resources.Media
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.FakeDb.Resources.Media;
  using Sitecore.Resources.Media;
  using Xunit;

  public class MediaProviderSwitcherTest
  {
    [Fact]
    public void ShouldSetMediaProviderBehaviour()
    {
      // arrange
      var behaviour = Substitute.For<MediaProvider>();

      // act
      using (new MediaProviderSwitcher(behaviour))
      {
        // assert
        ((IThreadLocalProvider<MediaProvider>)MediaManager.Provider).LocalProvider.Value.Should().Be(behaviour);
      }
    }
  }
}