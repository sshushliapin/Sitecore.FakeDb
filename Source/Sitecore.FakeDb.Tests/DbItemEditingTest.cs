namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Xunit;

  public class DbItemEditingTest
  {

    private readonly ID itemId = ID.NewID;

    private readonly ID templateId = ID.NewID;

    [Fact]
    public void ShouldCreateAndEditItemOfPredefinedTemplate()
    {
      // arrange
      using (var db = new Db
                        {
                          new DbTemplate("Sample", this.templateId) { "Title" },
                          new DbItem("Home", this.itemId, this.templateId)
                        })
      {
        var item = db.GetItem(this.itemId);

        // act
        using (new EditContext(item))
        {
          item.Fields["Title"].Value = "Welcome!";
        }

        // assert
        item["Title"].Should().Be("Welcome!");
      }
    }

    [Fact]
    public void ShouldEditItem()
    {
      // arrange
      using (var db = new Db { new DbItem("home") { { "Title", "Hello!" } } })
      {
        var item = db.Database.GetItem("/sitecore/content/home");

        // act
        using (new EditContext(item))
        {
          item["Title"] = "Welcome!";
        }

        // assert
        item["Title"].Should().Be("Welcome!");
      }
    }


    [Fact]
    public void ShouldNotUpdateOriginalItemOnEditing()
    {
      // arrange
      using (var db = new Db
                        {
                          new DbItem("old root")
                            {
                              new DbItem("item") { new DbField("Title") { { "en", 1, "Hi!" } }, }
                            },
                          new DbItem("new root")
                        })
      {
        var item = db.GetItem("/sitecore/content/old root/item");
        var newRoot = db.GetItem("/sitecore/content/new root");
        var copy = item.CopyTo(newRoot, "new item");

        // act
        using (new EditContext(copy))
        {
          copy["Title"] = "Welcome!";
        }

        // assert
        db.GetItem(copy.ID)["Title"].Should().Be("Welcome!");
        db.GetItem(item.ID)["Title"].Should().Be("Hi!");
      }
    }

    [Fact]
    public void ShouldEditAnItemCreatedUsingItemManager()
    {
      // arrange
      using (var db = new Db
      {
        new DbTemplate("Sample", this.templateId) { "Title" },
        new DbItem("main", ID.NewID, this.templateId)
      })
      {
        var root = db.Database.GetItem("/sitecore/content");

        // act
        var home = ItemManager.CreateItem("Home", root, this.templateId, this.itemId);
        var main = db.GetItem("/sitecore/content/main");

        using (new EditContext(home))
        {
          home["Title"] = "Welcome!";
        }

        // assert
        home.Fields.Count.Should().Be(main.Fields.Count); // make sure we properly created both - these two should look the same
        home["Title"].Should().Be("Welcome!");
      }
    }
  }
}
