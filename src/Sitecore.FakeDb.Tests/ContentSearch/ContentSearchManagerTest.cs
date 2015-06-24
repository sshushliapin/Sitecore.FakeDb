namespace Sitecore.FakeDb.Tests.ContentSearch
{
  using System;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.ContentSearch;
  using Xunit;

  public class ContentSearchManagerTest
  {
    [Fact]
    public void ShouldGetContentSearchConfiguration()
    {
      // act
      Action a = () => { var c = ContentSearchManager.SearchConfiguration; };

      // assert
      a.ShouldNotThrow();
    }

    [Fact]
    public void ShouldCreateSearchContext()
    {
      // arrange
      // TODO: Consider setting is by default. Potential concurrency issue.
      ContentSearchManager.SearchConfiguration.Indexes["fake_index"] = Substitute.For<ISearchIndex>();

      // act
      Action action = () => ContentSearchManager.CreateSearchContext(null);

      // assert
      action.ShouldNotThrow();
    }
  }
}