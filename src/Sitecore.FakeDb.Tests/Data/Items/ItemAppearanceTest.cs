namespace Sitecore.FakeDb.Tests.Data.Items
{
  using FluentAssertions;
  using Sitecore.Data.Items;
  using Xunit;

  public class ItemAppearanceTest
  {
    [Fact]
    public void ShouldReadDefaultItemAppearance()
    {
      // arrange & act
      using (var db = new Db { new DbItem("home") })
      {
        var item = db.GetItem("/sitecore/content/home");

        // assert
        item.Appearance.ContextMenu.Should().BeEmpty();
        item.Appearance.DisplayName.Should().Be("home");
        item.Appearance.HelpLink.Should().BeEmpty();
        item.Appearance.Hidden.Should().BeFalse();
        item.Appearance.Icon.Should().Be("/sitecore/client/images/document16x16.gif");
        item.Appearance.LongDescription.Should().BeEmpty();
        item.Appearance.ReadOnly.Should().BeFalse();
        item.Appearance.Ribbon.Should().BeEmpty();
        item.Appearance.ShortDescription.Should().BeEmpty();
        item.Appearance.Skin.Should().BeEmpty();
        item.Appearance.Sortorder.Should().Be(0);
        item.Appearance.Style.Should().BeEmpty();
        item.Appearance.Thumbnail.Should().Be("/sitecore/client/images/document16x16.gif");
      }
    }

    [Fact]
    public void ShouldHideItem()
    {
      // arrange
      using (var db = new Db { new DbItem("home") })
      {
        var item = db.GetItem("/sitecore/content/home");

        // act
        using (new EditContext(item))
        {
          item.Appearance.DisplayName = "Home";
          item.Appearance.Hidden = true;
          item.Appearance.ReadOnly = true;
        }

        // assert
        item.Appearance.DisplayName.Should().Be("Home");
        item.Appearance.Hidden.Should().BeTrue();
        item.Appearance.ReadOnly.Should().BeTrue();
      }
    }

    [Fact]
    public void ShouldCreateReadonlyItem()
    {
      // arrange
      using (var db = new Db
                        {
                          new DbItem("home") { { FieldIDs.ReadOnly, "1" } }
                        })
      {
        var item = db.GetItem("/sitecore/content/home");

        // assert
        item.Appearance.ReadOnly.Should().BeTrue();
      }
    }
  }
}