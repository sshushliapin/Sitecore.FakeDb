namespace Sitecore.FakeDb.Tests.ContentSearch
{
  using System;
  using FluentAssertions;
  using Sitecore.ContentSearch;
  using Xunit;

  public class ContentSearchConfigurationTest
  {
    [Fact]
    public void ShouldGetContentSearchConfiguration()
    {
      // act
      Action a = () => { var c = ContentSearchManager.SearchConfiguration; };

      // assert
      a.ShouldNotThrow();
    }
  }
}