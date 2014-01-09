namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Sitecore.Data;
  using Xunit;

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
    public void ShouldCreateTemplateItem()
    {
      // arrange
      var templateId = ID.NewID;

      // act
      using (var db = new Db { new FItem("my item", itemId, templateId) })
      {
        // assert
        var templateItem = db.Database.GetItem(templateId);
        templateItem.Should().NotBeNull();
        templateItem.Name.Should().Be("my item");
        templateItem.ID.Should().Be(templateId);
        templateItem.TemplateID.Should().Be(TemplateIDs.Template);
        templateItem.Paths.FullPath.Should().Be("/sitecore/templates/my item");
      }
    }

    [Fact]
    public void ShouldCreateTemplateSection()
    {
      // act
      using (var db = new Db { new FItem("my item") })
      {
        // assert
        var templateSection = db.Database.GetItem("/sitecore/templates/my item/Data");

        // assert
        templateSection.Should().NotBeNull();
        templateSection.TemplateID.Should().Be(TemplateIDs.TemplateSection);
      }
    }

    [Fact]
    public void ShouldCreateItemFields()
    {
      // act
      using (var db = new Db { new FItem("my item") { { "Title", string.Empty } } })
      {
        // assert
        var templateField = db.Database.GetItem("/sitecore/templates/my item/Data/Title");

        // assert
        templateField.Should().NotBeNull();
        templateField.TemplateID.Should().Be(TemplateIDs.TemplateField);
      }
    }

    [Fact]
    public void ShouldCreateItemWithFields()
    {
      // act
      using (var db = new Db
               {
                 new FItem("home", itemId) { {"Title", "Welcome!"}  }
               })
      {
        var item = db.Database.GetItem(itemId);

        // assert
        item["Title"].Should().Be("Welcome!");
      }
    }
  }
}