namespace Sitecore.FakeDb.Tests.Resources.Media
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.FakeDb.Resources.Media;
  using Sitecore.Resources.Media;
  using Xunit;

  public class FakeMediaProviderTest
  {
    private readonly MediaProvider behaviour;

    private readonly FakeMediaProvider provider;

    public FakeMediaProviderTest()
    {
      this.behaviour = Substitute.For<MediaProvider>();
      this.provider = new FakeMediaProvider { Behavior = behaviour };
    }

    [Fact]
    public void ShouldGetStubMediaProviderByDefault()
    {
      // act & assert
      new FakeMediaProvider().Behavior.Should().BeOfType<StubMediaProvider>();
    }

    [Fact]
    public void ShouldSetMediaProviderBehaviour()
    {
      // act
      this.provider.Behavior = this.behaviour;

      // assert
      this.provider.Behavior.Should().Be(this.behaviour);
    }

    [Fact]
    public void ShouldGetAndSetCache()
    {
      // arrange
      var mock = Substitute.For<MediaCache>();

      // act
      this.provider.Cache = mock;

      // assert
      this.behaviour.Cache.Should().Be(mock);
      this.provider.Cache.Should().Be(mock);
    }

    [Fact]
    public void ShouldGetAndSetConfig()
    {
      // arrange
      var mock = Substitute.For<MediaConfig>();

      // act
      this.provider.Config = mock;

      // assert
      this.behaviour.Config.Should().Be(mock);
      this.provider.Config.Should().Be(mock);
    }

    [Fact]
    public void ShouldGetAndSetCreator()
    {
      // arrange
      var mock = Substitute.For<MediaCreator>();

      // act
      this.provider.Creator = mock;

      // assert
      this.behaviour.Creator.Should().Be(mock);
      this.provider.Creator.Should().Be(mock);
    }

    [Fact]
    public void ShouldGetAndSetEffects()
    {
      // arrange
      var mock = Substitute.For<ImageEffects>();

      // act
      this.provider.Effects = mock;

      // assert
      this.behaviour.Effects.Should().Be(mock);
      this.provider.Effects.Should().Be(mock);
    }

    [Fact]
    public void ShouldGetMediaLinkPrefix()
    {
      // arrange
      this.behaviour.MediaLinkPrefix.Returns("prefix");

      // act
      this.provider.MediaLinkPrefix.Should().Be("prefix");
    }

    [Fact]
    public void ShouldGetAndSetMimeResolver()
    {
      // arrange
      var mock = Substitute.For<MimeResolver>();

      // act
      this.provider.MimeResolver = mock;

      // assert
      this.behaviour.MimeResolver.Should().Be(mock);
      this.provider.MimeResolver.Should().Be(mock);
    }

    [Fact]
    public void ShouldGetMedia()
    {
      // arrange
      var mock = CreateMediaItemMock();
      var result = Substitute.For<Media>();

      // act
      this.behaviour.GetMedia(mock).Returns(result);

      // assert
      this.provider.GetMedia(mock).Should().Be(result);
    }

    [Fact]
    public void ShouldGetMediaByUri()
    {
      // arrange
      var mock = Substitute.For<MediaUri>();
      var result = Substitute.For<Media>();

      // act
      this.behaviour.GetMedia(mock).Returns(result);

      // assert
      this.provider.GetMedia(mock).Should().Be(result);
    }

    [Fact]
    public void ShouldGetMediaUrl()
    {
      // arrange
      var mock = CreateMediaItemMock();

      this.behaviour.GetMediaUrl(mock).Returns("http://smth");

      // assert
      this.provider.GetMediaUrl(mock).Should().Be("http://smth");
    }

    [Fact]
    public void ShouldGetMediaUrlWithOptions()
    {
      // arrange
      var mock = CreateMediaItemMock();

      // act
      this.behaviour.GetMediaUrl(mock, null).Returns("http://smth");

      // assert
      this.provider.GetMediaUrl(mock, null).Should().Be("http://smth");
    }

    [Fact]
    public void ShouldGetThumbnailUrl()
    {
      // arrange
      var mock = CreateMediaItemMock();

      // act
      this.behaviour.GetThumbnailUrl(mock).Returns("http://smth");

      // assert
      this.provider.GetThumbnailUrl(mock).Should().Be("http://smth");
    }

    [Fact]
    public void ShouldCallHasMediaContent()
    {
      // arrange
      var mock = CreateMediaItemMock();

      // act
      this.behaviour.HasMediaContent(mock).Returns(true);

      // assert
      this.provider.HasMediaContent(mock).Should().BeTrue();
    }

    [Fact]
    public void ShouldCallIsMediaRequest()
    {
      // act
      this.behaviour.IsMediaRequest(null).ReturnsForAnyArgs(true);

      // assert
      this.provider.IsMediaRequest(null).Should().BeTrue();
    }

    [Fact]
    public void ShouldCallIsMediaUrl()
    {
      // act
      this.behaviour.IsMediaUrl("http://smth").Returns(true);

      // assert
      this.provider.IsMediaUrl("http://smth").Should().BeTrue();
    }

    [Fact]
    public void ShouldCallParseMediaRequest()
    {
      // arrange
      var mock = Substitute.For<MediaRequest>();

      // act
      this.behaviour.ParseMediaRequest(null).Returns(mock);

      // assert
      this.provider.ParseMediaRequest(null).Should().Be(mock);
    }

    private static MediaItem CreateMediaItemMock()
    {
      return Substitute.For<MediaItem>(ItemHelper.CreateInstance(Database.GetDatabase("master")));
    }
  }
}