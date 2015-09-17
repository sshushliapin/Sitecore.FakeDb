namespace Sitecore.FakeDb.Tests.Links
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Common;
  using Sitecore.Links;
  using Xunit;

  public class LinkProviderSwitcherTest
  {
    [Fact]
    public void ShouldSwitchLinkProvider()
    {
      // arrange
      using (var db = new Db { new DbItem("home") })
      {
        var item = db.GetItem("/sitecore/content/home");
        var options = new UrlOptions { AlwaysIncludeServerUrl = true };

        var provider = Substitute.For<LinkProvider>();
        provider.GetItemUrl(item, options).Returns("http://myawesomeurl");

        using (new LinkProviderSwitcher("switcher"))
        using (new Switcher<LinkProvider>(provider))
        {
          // act & assert
          LinkManager.GetItemUrl(item, options).Should().Be("http://myawesomeurl");
        }
      }
    }
  }
}