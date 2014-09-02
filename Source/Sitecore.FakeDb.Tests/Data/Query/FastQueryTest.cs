namespace Sitecore.FakeDb.Tests.Data.Query
{
  using FluentAssertions;
  using Sitecore.Data;
  using Xunit;

  public class FastQueryTest
  {
    [Fact]
    public void ShouldSelectSingleItem()
    {
      // arrange
      using (var db = new Db { new DbItem("home") })
      {
        // act
        var result = db.Database.SelectSingleItem("fast:/sitecore/content/home");

        // assert 
        result.Should().NotBeNull();
        result.Name.Should().Be("home");
      }
    }

    [Fact]
    public void ShouldSelectItems()
    {
      // arrange
      using (var db = new Db
                        {
                          new DbItem("home")
                            {
                              new DbItem("child 1"),
                              new DbItem("child 2")
                            }
                        })
      {
        // act
        var result = db.Database.SelectItems("fast:/sitecore/content/home/*");

        // assert 
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("child 1");
        result[1].Name.Should().Be("child 2");
      }
    }

    [Fact]
    public void ShouldReturnNullIfNoSingleItemFound()
    {
      // arrange
      using (var db = new Db())
      {
        // act
        var result = db.Database.SelectSingleItem("fast:/sitecore/content/home");

        // assert 
        result.Should().BeNull();
      }
    }

    [Fact]
    public void ShouldReturnEmptyArrayIfNoItemsFound()
    {
      // arrange
      using (var db = new Db())
      {
        // act
        var result = db.Database.SelectItems("fast:/sitecore/content/*");

        // assert 
        result.Should().BeEmpty();
      }
    }

    [Fact]
    public void ShouldSelectItemByFieldName()
    {
      // arrange
      using (var db = new Db { new DbItem("Home") { { "Title", "Welcome to Sitecore" } } })
      {
        // act
        var item = db.Database.SelectSingleItem("fast:/sitecore/content/*[@Title = 'Welcome to Sitecore']");

        // assert
        item.Should().NotBeNull();
        item.Name.Should().Be("Home");
      }
    }

    [Fact]
    public void ShouldSelectItemById()
    {
      // arrange
      using (var db = new Db { new DbItem("Home", new ID("{787EE6C5-0885-495D-855E-1D129C643E55}")) })
      {
        // act
        var item = db.Database.SelectSingleItem("fast://*[@@id = '{787EE6C5-0885-495D-855E-1D129C643E55}']");

        // assert
        item.Should().NotBeNull();
        item.Name.Should().Be("Home");
      }
    }
  }
}