namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Xunit.Extensions;

  public class QueryTest
  {

    [Theory]
    [InlineData("/sitecore/content/home")]
    [InlineData("/sitecore/content/*")]
    [InlineData("/sitecore/content/*[@@key = 'home']")]
    public void ShouldSupportQuery(string query)
    {
      // arrange
      var homeId = ID.NewID;

      using (var db = new Db() { new DbItem("home", homeId) })
      {
        Item[] result;

        // act
        using (new DatabaseSwitcher(db.Database))
        {
          result = Sitecore.Data.Query.Query.SelectItems(query);  
        }

        // assert 
        result.Should().HaveCount(1);
        result[0].ID.Should().Be(homeId);
      }
    }
  }
}
