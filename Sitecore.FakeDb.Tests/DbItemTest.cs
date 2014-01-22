namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Xunit;

  public class DbItemTest
  {
    [Fact]
    public void ShouldGenerateNewIdsIfNotSet()
    {
      // arrange
      var item = new DbItem("my item");

      // act & assert
      item.ID.Should().NotBeNull();
      item.TemplateID.Should().NotBeNull();
    }

    [Fact]
    public void ShouldCreateEmptyFieldsCollection()
    {
      new DbItem("home").Fields.Should().BeEmpty();
    }

    [Fact]
    public void ShouldAddFields()
    {
      // arrange 
      var item = new DbItem("home") { { "Title", "Welcome!" } };

      // act & assert
      item.Fields["Title"].Should().Be("Welcome!");
    }

    [Fact]
    public void ShouldSetSitecoreContentParentIdByDefault()
    {
      // arrange
      var item = new DbItem("home");

      // act & assert
      item.ParentID.Should().Be(ItemIDs.ContentRoot);
    }

    [Fact]
    public void ShouldSetSitecoreContentFullPathByDefault()
    {
      // arrange
      var item = new DbItem("home");

      // act & asert
      item.FullPath.Should().Be("/sitecore/content/home");
    }
  }
}