namespace Sitecore.FakeDb.Tests.Data.Items
{
  using FluentAssertions;
  using Sitecore.Data.Items;
  using Sitecore.Security.Accounts;
  using Sitecore.SecurityModel;
  using Xunit;

  public class ItemStatisticsTest
  {
    [Fact]
    public void ShouldUpdateStatistics()
    {
      // arrange
      using (var db = new Db { new DbItem("Home") })
      {
        var item = db.GetItem("/sitecore/content/home");

        // act
        using (new UserSwitcher("Editor", false))
        using (new EditContext(item, SecurityCheck.Disable))
        {
          item.Statistics.UpdateRevision();
        }

        // assert
        item.Statistics.UpdatedBy.Should().Be("Editor");
      }
    }
  }
}