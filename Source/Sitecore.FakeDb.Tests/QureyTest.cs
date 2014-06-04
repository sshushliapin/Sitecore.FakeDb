namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Xunit.Extensions;

  public class QureyTest
  {

    [Theory]
    [InlineData("/sitecore/content/home")]
    [InlineData("/sitecore/content/*")]
    [InlineData("/sitecore/content/*[@@key = 'home']")]
    public void ShouldSupportQuery(string query)
    {
      // arrange
      var homeId = ID.NewID;

      // ToDo (?): add setting (and cleaning) up Context.Database to the Db constructor/dispose
      var ctxDb = Context.Database;

      using (var db = new Db() { new DbItem("home", homeId) })
      {
        Context.Database = db.Database; // Query needs it

        // act
        Item[] result = Sitecore.Data.Query.Query.SelectItems(query);

        Context.Database = ctxDb;

        // assert 
        result.Should().HaveCount(1);
        result[0].ID.Should().Be(homeId);
      }
    }
  }
}
