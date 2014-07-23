namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Query;
  using Xunit;
  using Xunit.Extensions;

  public class QueryTest
  {
    [Theory]
    [InlineData("/sitecore/content/home")]
    [InlineData("/sitecore/content/*")]
    [InlineData("/sitecore/content/*[@@key = 'home']")]
    // TODO:[Med] Unstable.
    // [InlineData("/sitecore/content/*[@@templatekey = 'home']")]
    public void ShouldSupportQuery(string query)
    {
      // arrange
      ID homeId = ID.NewID;

      using (var db = new Db { new DbItem("home", homeId) })
      {
        Item[] result;

        // act
        using (new DatabaseSwitcher(db.Database))
        {
          result = Query.SelectItems(query);
        }

        // assert 
        result.Should().HaveCount(1);
        result[0].ID.Should().Be(homeId);
      }
    }

    // TODO:[High] Fails sometimes in xUnit Test Runner (works fine in NCrunch). Database SingleInstance="false" solves the problem but this is no go (item URI won't work).
    [Fact]
    public void ShouldSupportQueryByBaseTemplate()
    {
      // arrange
      ID baseId = ID.NewID;
      ID templateId = ID.NewID;

      using (var db = new Db
      {
        new DbTemplate("base", baseId),
        new DbTemplate("derrived", templateId) { BaseIDs = new[] { baseId } }
      })
      {
        var query = string.Format("/sitecore/templates//*[contains(@__Base template, '{0}')]", baseId);
        Item[] result;

        // act
        using (new DatabaseSwitcher(db.Database))
        {
          result = Query.SelectItems(query);
        }

        // assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.Should().Contain(i => i.ID == templateId);
      }
    }
  }
}