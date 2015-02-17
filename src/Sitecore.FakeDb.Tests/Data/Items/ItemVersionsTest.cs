namespace Sitecore.FakeDb.Tests.Data.Items
{
  using FluentAssertions;
  using Xunit;

  public class ItemVersionsTest
  {
    [Fact]
    public void ShouldAddItemVersionAndUpdateCountIfNoVersionedFieldsExist()
    {
      // arrange
      using (var db = new Db { new DbItem("home") })
      {
        var item = db.GetItem("/sitecore/content/home");

        // act
        var itemV2 = item.Versions.AddVersion();

        // assert
        itemV2.Versions.Count.Should().Be(2);
      }
    }

    [Fact]
    public void ShouldRemoveItemVersionAndUpdateCountIfNoVersionedFieldsExist()
    {
      // arrange
      using (var db = new Db { new DbItem("home") })
      {
        var item = db.GetItem("/sitecore/content/home");

        // act
        item.Versions.AddVersion()
            .Versions.RemoveVersion();

        // assert
        db.GetItem("/sitecore/content/home").Versions.Count.Should().Be(1);
      }
    }

    [Fact]
    public void ShouldReturnEmptyVersionsCollectionIfNoVersionsFound()
    {
      // arrange
      using (var db = new Db
                        {
                          new DbItem("home") { new DbField("Title") { { "en", 1, "value" } } }
                        })
      {
        // act
        var target = db.GetItem("/sitecore/content/home", "af-ZA");

        // assert
        target.Versions.Count.Should().Be(0);
      }
    }
  }
}