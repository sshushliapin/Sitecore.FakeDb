namespace Sitecore.FakeDb.Tests.Data.Managers
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Xunit;

  public class ItemManagerTest
  {
    [Fact]
    public void ShouldCreateAndEditItemUsingItemManager()
    {
      // arrange
      var templateId = ID.NewID;

      using (var db = new Db
                        {
                          new DbTemplate("Sample", templateId) { "Title" },
                        })
      {
        var root = db.GetItem("/sitecore/content");

        // act
        var item = ItemManager.AddFromTemplate("home", templateId, root);
        using (new EditContext(item))
        {
          item["Title"] = "Hello";
        }

        // assert
        item["Title"].Should().Be("Hello");
      }
    }
  }
}