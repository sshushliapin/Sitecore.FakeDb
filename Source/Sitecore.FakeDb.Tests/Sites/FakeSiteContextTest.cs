namespace Sitecore.FakeDb.Tests.Sites
{
  using FluentAssertions;
  using Sitecore.FakeDb.Sites;
  using Xunit;
  using StringDictionary = Sitecore.Collections.StringDictionary;

  public class FakeSiteContextTest
  {
    [Fact]
    public void ShouldCreateSimpleFakeSiteContext()
    {
      // arrange & act
      var siteContext = new FakeSiteContext("mywebsite");

      // assert
      siteContext.Name.Should().Be("mywebsite");
      siteContext.Database.Should().BeNull();
    }

    [Fact]
    public void ShouldCreateAdvancedFakeSiteContext()
    {
      // arrange & act
      var siteContext = new FakeSiteContext(new StringDictionary { { "name", "mywebsite" }, { "database", "web" } });

      // assert
      siteContext.Name.Should().Be("mywebsite");
      siteContext.Database.Name.Should().Be("web");
    }
  }
}