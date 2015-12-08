namespace Sitecore.FakeDb.Tests.ContentSearch
{
  using System;
  using FluentAssertions;
  using Sitecore.ContentSearch;
  using Xunit;

  public class ContentSearchManagerTest
  {
    [Fact]
    public void ShouldGetContentSearchConfiguration()
    {
      Action a = () => { var c = ContentSearchManager.SearchConfiguration; };
      a.ShouldNotThrow();
    }
  }
}