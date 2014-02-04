namespace Sitecore.FakeDb.Tests.Data.Items
{
  using FluentAssertions;
  using Sitecore.Data.Items;
  using Xunit;

  public class EditContextTest
  {
    [Fact]
    public void ShouldRenameItem()
    {
      // arrange
      using (var db = new Db { new DbItem("home") })
      {
        var item = db.Database.GetItem("/sitecore/content/home");

        // act
        using (new EditContext(item))
        {
          item.Name = "new home";
        }

        // assert
        item.Name.Should().Be("new home");
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
  }
}