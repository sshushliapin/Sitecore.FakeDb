namespace Sitecore.FakeDb.Tests.Data.Managers
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Xunit;

  public class ItemManagerTest
  {
    private readonly ID templateId = ID.NewID;

    [Fact]
    public void ShouldCreateAndEditItemUsingItemManager()
    {
      // arrange
      using (var db = new Db { new DbTemplate("Sample", this.templateId) { "Title" } })
      {
        var root = db.GetItem("/sitecore/content");

        // act
        var item = ItemManager.AddFromTemplate("home", this.templateId, root);
        using (new EditContext(item))
        {
          item["Title"] = "Hello";
        }

        // assert
        item["Title"].Should().Be("Hello");
      }
    }

    [Fact]
    public void ShouldAddVersionOneWhenAddFromTemplate()
    {
      // arrange
      using (var db = new Db { new DbTemplate("Sample", this.templateId) { "Title" } })
      {
        var root = db.GetItem("/sitecore/content");

        // act
        var item = ItemManager.AddFromTemplate("home", this.templateId, root);

        // assert
        item.Versions.Count.Should().Be(1);
      }
    }

    [Fact]
    public void ShouldNotAddVersionWhenCreateItem()
    {
      // arrange
      using (var db = new Db { new DbTemplate("Sample", this.templateId) { "Title" } })
      {
        var root = db.GetItem("/sitecore/content");

        // act
        var item = ItemManager.CreateItem("home", root, this.templateId);

        // assert
        item.Versions.Count.Should().Be(0);
      }
    }
  }
}