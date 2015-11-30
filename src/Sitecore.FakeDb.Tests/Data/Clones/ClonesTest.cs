namespace Sitecore.FakeDb.Tests.Data.Clones
{
  using FluentAssertions;
  using Sitecore.Data.Items;
  using Xunit;

  public class ClonesTest
  {
    [Fact]
    public void ShouldCloneAndEditItem()
    {
      // arrange
      using (var db = new Db { new DbItem("source") { { "Title", "Source Title" } } })
      {
        var contentRoot = db.GetItem("/sitecore/content/");
        var source = db.GetItem("/sitecore/content/source");

        // act
        var clone = source.CloneTo(contentRoot, "clone", true);
        using (new EditContext(clone))
        {
          clone["Title"] = "New Title";
        }

        // assert
        clone["Title"].Should().Be("New Title");
      }
    }
  }
}