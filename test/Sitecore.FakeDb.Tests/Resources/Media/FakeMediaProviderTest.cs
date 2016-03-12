namespace Sitecore.FakeDb.Tests.Resources.Media
{
  using System;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.FakeDb.Resources.Media;
  using Sitecore.Resources.Media;
  using Xunit;

  public class FakeMediaProviderTest : IDisposable
  {
    private readonly MediaProvider localProvider;

    private readonly FakeMediaProvider provider;

    public FakeMediaProviderTest()
    {
      this.localProvider = Substitute.For<MediaProvider>();
      this.provider = new FakeMediaProvider();
      this.provider.LocalProvider.Value = this.localProvider;
    }

    [Fact]
    public void ShouldReturnDefaultValuesIfNoLocalProviderSet()
    {
      // arrange
      var mediaProvider = new FakeMediaProvider();

      // act & assert
      mediaProvider.Cache.Should().NotBeNull();
      mediaProvider.Config.Should().NotBeNull();
      mediaProvider.Creator.Should().NotBeNull();
      mediaProvider.Effects.Should().NotBeNull();
      mediaProvider.MediaLinkPrefix.Should().NotBeNull();
      mediaProvider.MimeResolver.Should().NotBeNull();

      mediaProvider.GetMedia((MediaItem)null).Should().BeNull();
      mediaProvider.GetMedia((MediaUri)null).Should().BeNull();
      mediaProvider.GetMediaUrl(null).Should().BeNull();
      mediaProvider.GetMediaUrl(null, null).Should().BeNull();
      mediaProvider.GetThumbnailUrl(null).Should().BeNull();
      mediaProvider.HasMediaContent(null).Should().BeFalse();
      mediaProvider.IsMediaRequest(null).Should().BeFalse();
      mediaProvider.IsMediaUrl(null).Should().BeFalse();
      mediaProvider.ParseMediaRequest(null).Should().BeNull();
    }

    [Fact]
    public void ShouldSetMediaProviderBehaviour()
    {
      // act
      this.provider.LocalProvider.Value = this.localProvider;

      // assert
      this.provider.LocalProvider.Value.Should().Be(this.localProvider);
    }

    [Fact]
    public void ShouldGetAndSetCache()
    {
      // arrange
      var mock = Substitute.For<MediaCache>();

      // act
      this.provider.Cache = mock;

      // assert
      this.localProvider.Cache.Should().Be(mock);
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
      this.localProvider.Config.Should().Be(mock);
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
      this.localProvider.Creator.Should().Be(mock);
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
      this.localProvider.Effects.Should().Be(mock);
      this.provider.Effects.Should().Be(mock);
    }

    [Fact]
    public void ShouldGetMediaLinkPrefix()
    {
      // arrange
      this.localProvider.MediaLinkPrefix.Returns("prefix");

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
      this.localProvider.MimeResolver.Should().Be(mock);
      this.provider.MimeResolver.Should().Be(mock);
    }

    [Fact]
    public void ShouldGetMedia()
    {
      // arrange
      var mock = CreateMediaItemMock();
      var result = Substitute.For<Media>();

      // act
      this.localProvider.GetMedia(mock).Returns(result);

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
      this.localProvider.GetMedia(mock).Returns(result);

      // assert
      this.provider.GetMedia(mock).Should().Be(result);
    }

    [Fact]
    public void ShouldGetMediaUrl()
    {
      // arrange
      var mock = CreateMediaItemMock();

      this.localProvider.GetMediaUrl(mock).Returns("http://smth");

      // assert
      this.provider.GetMediaUrl(mock).Should().Be("http://smth");
    }

    [Fact]
    public void ShouldGetMediaUrlWithOptions()
    {
      // arrange
      var mock = CreateMediaItemMock();

      // act
      this.localProvider.GetMediaUrl(mock, null).Returns("http://smth");

      // assert
      this.provider.GetMediaUrl(mock, null).Should().Be("http://smth");
    }

    [Fact]
    public void ShouldGetThumbnailUrl()
    {
      // arrange
      var mock = CreateMediaItemMock();

      // act
      this.localProvider.GetThumbnailUrl(mock).Returns("http://smth");

      // assert
      this.provider.GetThumbnailUrl(mock).Should().Be("http://smth");
    }

    [Fact]
    public void ShouldCallHasMediaContent()
    {
      // arrange
      var mock = CreateMediaItemMock();

      // act
      this.localProvider.HasMediaContent(mock).Returns(true);

      // assert
      this.provider.HasMediaContent(mock).Should().BeTrue();
    }

    [Fact]
    public void ShouldCallIsMediaRequest()
    {
      // act
      this.localProvider.IsMediaRequest(null).ReturnsForAnyArgs(true);

      // assert
      this.provider.IsMediaRequest(null).Should().BeTrue();
    }

    [Fact]
    public void ShouldCallIsMediaUrl()
    {
      // act
      this.localProvider.IsMediaUrl("http://smth").Returns(true);

      // assert
      this.provider.IsMediaUrl("http://smth").Should().BeTrue();
    }

    [Fact]
    public void ShouldCallParseMediaRequest()
    {
      // arrange
      var mock = Substitute.For<MediaRequest>();

      // act
      this.localProvider.ParseMediaRequest(null).Returns(mock);

      // assert
      this.provider.ParseMediaRequest(null).Should().Be(mock);
    }

    private static MediaItem CreateMediaItemMock()
    {
      return Substitute.For<MediaItem>(ItemHelper.CreateInstance(Database.GetDatabase("master")));
    }

    public void Dispose()
    {
      this.provider.Dispose();
    }
  }
}