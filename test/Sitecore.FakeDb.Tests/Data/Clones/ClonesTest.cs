namespace Sitecore.FakeDb.Tests.Data.Clones
{
  using FluentAssertions;
  using Sitecore.Data.Items;
  using Xunit;

  [Trait("Category", "RequireLicense")]
  public class ClonesTest
  {
    [Fact]
    public void ShouldCloneItem()
    {
      // arrange
      using (var db = new Db { new DbItem("home") })
      {
        var item = db.GetItem("/sitecore/content/home");

        // act
        var clone = item.CloneTo(item.Parent, "clone", true);

        // assert
        clone.SourceUri.Should().Be(item.Uri);
      }
    }

    [Fact]
    public void ShouldReadFieldsFromClonedItem()
    {
      // arrange
      using (var db = new Db
        {
          new DbItem("source") { { "Title", "Source Title" } }
        })
      {
        var contentRoot = db.GetItem("/sitecore/content/");
        var source = db.GetItem("/sitecore/content/source");

        // act
        var clone = source.CloneTo(contentRoot, "clone", true);

        // assert
        clone["Title"].Should().Be("Source Title");
      }
    }

    [Fact]
    public void ShouldCloneAndEditItem()
    {
      // arrange
      using (var db = new Db
        {
          new DbItem("source") { { "Title", "Source Title" } }
        })
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