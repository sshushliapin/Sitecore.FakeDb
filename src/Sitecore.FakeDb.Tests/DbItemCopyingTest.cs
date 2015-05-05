namespace Sitecore.FakeDb.Tests
{
  using System.Linq;
  using FluentAssertions;
  using Xunit;

  public class DbItemCopyingTest
  {
    [Fact]
    public void ShouldCopyItem()
    {
      // arrange
      using (var db = new Db
                        {
                          new DbItem("old root")
                            {
                              new DbItem("item") { { "Title", "Welcome!" } }
                            },
                            new DbItem("new root")
                        })
      {
        var item = db.GetItem("/sitecore/content/old root/item");
        var newRoot = db.GetItem("/sitecore/content/new root");

        // act
        var copy = item.CopyTo(newRoot, "new item");

        // assert
        db.GetItem("/sitecore/content/new root/new item").Should().NotBeNull();
        db.GetItem("/sitecore/content/new root").Children["new item"].Should().NotBeNull();
        db.GetItem("/sitecore/content/old root/item").Should().NotBeNull();
        db.GetItem("/sitecore/content/old root").Children["item"].Should().NotBeNull();

        copy["Title"].Should().Be("Welcome!");
      }
    }

    [Fact]
    public void ShouldCopyItemInAllLanguagesAndVersions()
    {
      // arrange
      using (var db = new Db
                        {
                          new DbItem("old root")
                            {
                              new DbItem("item") { new DbField("Title") { { "en", 1, "Hi!" }, { "en", 2, "Welcome!" }, { "da", 1, "Velkommen!" } }, }
                            },
                          new DbItem("new root")
                        })
      {
        var item = db.GetItem("/sitecore/content/old root/item");
        var newRoot = db.GetItem("/sitecore/content/new root");

        // act
        var copy = item.CopyTo(newRoot, "new item");

        // assert
        db.GetItem(copy.ID, "en", 1)["Title"].Should().Be("Hi!");
        db.GetItem(copy.ID, "en", 2)["Title"].Should().Be("Welcome!");
        db.GetItem(copy.ID, "da", 1)["Title"].Should().Be("Velkommen!");
      }
    }

    [Fact]
    public void ShouldDeepCopyItem()
    {
      // arrange
      using (var db = new Db
                        {
                          new DbItem("original") { new DbItem("child") { { "Title", "Child" } } }
                        })
      {
        var original = db.GetItem("/sitecore/content/original");
        var root = db.GetItem("/sitecore/content");

        // act
        var copy = original.CopyTo(root, "copy"); // deep is the default

        // assert
        copy.Should().NotBeNull();
        copy.Children.Should().HaveCount(1);

        var child = copy.Children.First();
        child.Fields["Title"].Should().NotBeNull("'child.Fields[\"Title\"]' should not be null");
        child.Fields["Title"].Value.Should().Be("Child");
        child.ParentID.Should().Be(copy.ID);
        child.Name.Should().Be("child");
        child.Paths.FullPath.Should().Be("/sitecore/content/copy/child");
      }
    }

    [Fact]
    public void ShouldCopyStandardFieldsWithoutTemplate()
    {
      // arrange
      using (var db = new Db
                        {
                          new DbItem("home") { new DbField(FieldIDs.LayoutField) { Value = "<r />" } }
                        })
      {
        var home = db.GetItem("/sitecore/content/home");

        // act
        var copy = home.CopyTo(home.Parent, "copy");

        // assert
        copy.Fields[FieldIDs.LayoutField].Value.Should().NotBeNullOrEmpty();
      }
    }

    [Fact]
    public void ShouldCopySharedFields()
    {
      // arrange
      using (var db = new Db
                        {
                          new DbItem("home") { new DbField("Title") { Shared = true, Value = "Me" } }
                        })
      {
        var home = db.GetItem("/sitecore/content/home");

        // act
        var copy = home.CopyTo(home.Parent, "copy");

        // assert
        copy.Fields["Title"].Should().NotBeNull("'copy.Fields[\"Title\"]' should not be null");
        copy.Fields["Title"].Shared.Should().BeTrue();
        copy["Title"].Should().Be("Me");
      }
    }

    [Fact]
    public void ShouldCopyFieldType()
    {
      using (var db = new Db
                        {
                          new DbItem("home") { new DbField("Active") { Value = "1", Type = "Checkbox" } }
                        })
      {
        var home = db.GetItem("/sitecore/content/home");

        // act
        var copy = home.CopyTo(home.Parent, "copy");

        // assert
        copy["Active"].Should().Be("1");
        copy.Fields["Active"].Should().NotBeNull("'copy.Fields[\"Active\"]' should not be null");
        copy.Fields["Active"].Type.Should().Be("Checkbox");
      }
    }
  }
}
