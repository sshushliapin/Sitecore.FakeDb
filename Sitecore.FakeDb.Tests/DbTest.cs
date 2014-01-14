namespace Sitecore.FakeDb.Tests
{
  using System.Linq;
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data;
  using Xunit;

  // TODO: The tests bellow depend from commands implementation which makes them brittle.
  public class DbTest
  {
    private readonly ID itemId = ID.NewID;

    [Fact]
    public void ShouldHaveDefaultMasterDatabase()
    {
      // arrange
      var db = new Db();

      // act & assert
      db.Database.Name.Should().Be("master");
    }

    [Fact]
    public void ShouldReadDefaultContentItem()
    {
      // arrange
      using (var db = new Db())
      {
        // act
        var item = db.Database.GetItem(ItemIDs.ContentRoot);

        // assert
        item.Should().NotBeNull();
      }
    }

    [Fact]
    public void ShouldCreateSimpleItem()
    {
      // arrange
      var id = new ID("{91494A40-B2AE-42B5-9469-1C7B023B886B}");

      // act
      using (var db = new Db { new FItem("myitem", id) })
      {
        var i = db.Database.GetItem(id);

        // assert
        i.Should().NotBeNull();
        i.Name.Should().Be("myitem");
      }
    }

    /// <summary>
    /// Shoulds the cleanup items after dispose.
    /// </summary>
    [Fact]
    public void ShouldCleanupItemsAfterDispose()
    {
      // act
      using (new Db { new FItem("myitem", this.itemId) })
      {
        Database.GetDatabase("master").GetItem(this.itemId).Should().NotBeNull();
      }

      // assert
      Database.GetDatabase("master").GetItem(this.itemId).Should().BeNull();
    }

    [Fact]
    public void ShouldGetItemByPath()
    {
      // arrange
      using (var db = new Db { new FItem("my item") })
      {
        db.Database.GetItem("/sitecore/content/my item").Should().NotBeNull();
      }
    }

    [Fact]
    public void ShouldCreateFakeTemplate()
    {
      // arrange
      using (var db = new Db { new FItem("my item") { { "my field", "" } } })
      {
        // act
        var dataStorage = db.Database.GetDataStorage();

        // assert
        var template = dataStorage.FakeTemplates.Single().Value;
        template.Name.Should().Be("my item");
        template.Fields["my field"].Should().NotBeNull();
      }
    }

    [Fact]
    public void ShouldCreateItemWithFields()
    {
      // act
      using (var db = new Db { new FItem("home", itemId) { { "Title", "Welcome!" } } })
      {
        var item = db.Database.GetItem(itemId);

        // assert
        item["Title"].Should().Be("Welcome!");
      }
    }
  }
}