namespace Sitecore.FakeDb.Tests.Sites
{
  using FluentAssertions;
  using Sitecore.FakeDb.Sites;
  using Sitecore.Sites;
  using Xunit;

  public class SiteContextSwitcherTest
  {
    [Fact]
    public void ShouldSwitchContextSite()
    {
      // arrange
      var site = new FakeSiteContext("mywebsite");

      // act
      using (new SiteContextSwitcher(site))
      {
        // assert
        Context.Site.Name.Should().Be("mywebsite");
      }

      Context.Site.Should().BeNull();
    }
  }
}