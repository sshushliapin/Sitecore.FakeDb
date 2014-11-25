namespace Sitecore.FakeDb.Tests.Data
{
  using FluentAssertions;
  using Sitecore.Data;
  using Xunit;

  public class BranchRecordsTest
  {
    [Fact]
    public void ShouldSetItemBranchIfExists()
    {
      // arrange
      var branchItemId = ID.NewID;

      using (var db = new Db
                        {
                          new DbItem("Sample Branch", branchItemId, TemplateIDs.BranchTemplate),
                          new DbItem("Item from Branch", ID.NewID, branchItemId)
                        })
      {
        // act
        var item = db.GetItem("/sitecore/content/item from branch");

        // assert
        item.BranchId.Should().Be(branchItemId);
        item.TemplateID.Should().NotBe(branchItemId);
      }
    }

    [Fact]
    public void ShouldReadDatabaseBranches()
    {
      // arrange
      var branchItemId = ID.NewID;

      // act
      using (var db = new Db
                        {
                          new DbItem("Sample Branch", branchItemId, TemplateIDs.BranchTemplate),
                        })
      {
        // assert
        db.Database.Branches["/sitecore/templates/branches/Sample Branch"].Should().NotBeNull();
        db.Database.Branches[branchItemId].Should().NotBeNull();
      }
    }

    [Fact(Skip = "To be implemented.")]
    public void ShouldCreateItemFromBranch()
    {
      // arrange
      var branchId = ID.NewID;

      using (var db = new Db { new DbItem("home") })
      {
        var item = db.GetItem("/sitecore/content/home");

        // act
        var child = item.Add("child", new BranchId(branchId));

        // assert
        child.BranchId.Should().Be(branchId);
        child.TemplateID.Should().NotBe(branchId);
      }
    }
  }
}