namespace Sitecore.FakeDb.Tests
{
  using System.Linq;
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.FakeDb.Security.AccessControl;
  using Xunit;

  public class DbItemTest
  {
    [Fact]
    public void ShouldGenerateNewIdsIfNotSet()
    {
      // arrange & act
      var item = new DbItem("my item");

      // assert
      item.ID.IsNull.Should().BeFalse();
    }

    [Fact]
    public void ShouldGenerateNameBasedOnIdIfNotSet()
    {
      // arrange
      var id = ID.NewID;
      var item = new DbItem(null, id);

      // act & assert
      item.Name.Should().Be(id.ToShortID().ToString());
    }

    [Fact]
    public void ShouldCreateNewDbItem()
    {
      // arrange & act
      var item = new DbItem("home");

      // assert
      item.TemplateID.IsNull.Should().BeTrue();
      item.Children.Should().BeEmpty();
      item.Fields.Should().BeEmpty();
      item.FullPath.Should().BeNull();
      item.ParentID.Should().BeNull();
    }

    [Fact]
    public void ShouldAddFieldByNameAndValue()
    {
      // arrange 
      var item = new DbItem("home") { { "Title", "Welcome!" } };

      // act & assert
      item.Fields.Should().ContainSingle(f => f.Name == "Title" && f.Value == "Welcome!");
    }

    [Fact]
    public void ShouldAddFieldByIdAndValue()
    {
      // arrange 
      var item = new DbItem("home") { { FieldIDs.Hidden, "1" } };

      // act & assert
      item.Fields.Should().ContainSingle(f => f.ID == FieldIDs.Hidden && f.Value == "1");
    }

    [Fact]
    public void ShouldAddChildItem()
    {
      // arrange
      var parent = new DbItem("parent");
      var child = new DbItem("child");

      // act
      parent.Add(child);

      // assert
      parent.Children.Single().Should().BeEquivalentTo(child);
    }

    [Fact]
    public void ShouldCreateNewItemAccess()
    {
      // arrange
      var item = new DbItem("home");

      // act
      item.Access.Should().BeOfType<DbItemAccess>();
    }

    [Fact]
    public void ShouldSetItemAccess()
    {
      // arrange
      var item = new DbItem("home") { Access = new DbItemAccess { CanRead = false } };

      // act & assert
      item.Access.CanRead.Should().BeFalse();
    }
  }
}