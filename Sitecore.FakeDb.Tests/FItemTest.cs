namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Xunit;

  public class FItemTest
  {
    [Fact]
    public void ShouldGenerateNewIdsIfNotSet()
    {
      // arrange
      var item = new FItem("my item");

      // act & assert
      item.ID.Should().NotBeNull();
      item.TemplateID.Should().NotBeNull();
    }

    [Fact]
    public void ShouldCreateEmptyFieldsCollection()
    {
      new FItem("home").Fields.Should().BeEmpty();
    }

    [Fact]
    public void ShouldAddFields()
    {
      // arrange 
      var item = new FItem("home") { { "Title", "Welcome!" } };

      // act & assert
      item.Fields["Title"].Should().Be("Welcome!");
    }

    [Fact]
    public void ShouldSetSitecoreContentParentIdByDefault()
    {
      // arrange
      var item = new FItem("home");

      // act & assert
      item.ParentID.Should().Be(ItemIDs.ContentRoot);
    }

    [Fact]
    public void ShouldSetSitecoreContentFullPathByDefault()
    {
      // arrange
      var item = new FItem("home");

      // act & asert
      item.FullPath.Should().Be("/sitecore/content/home");
    }
  }
}