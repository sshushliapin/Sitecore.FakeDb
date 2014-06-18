namespace Sitecore.FakeDb.Tests.Resources.Media
{
  using FluentAssertions;
  using Sitecore.FakeDb.Resources.Media;
  using Sitecore.Resources.Media;
  using Xunit;

  public class StubMediaProviderTest
  {
    [Fact]
    public void ShouldReturnEmptyValues()
    {
      // arrange
      var mediaProvider = new StubMediaProvider();

      // act & assert
      mediaProvider.Cache.Should().NotBeNull();
      mediaProvider.Config.Should().NotBeNull();
      mediaProvider.Creator.Should().NotBeNull();
      mediaProvider.Effects.Should().NotBeNull();
    }
  }
}