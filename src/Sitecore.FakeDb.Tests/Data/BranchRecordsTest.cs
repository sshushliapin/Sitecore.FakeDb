namespace Sitecore.FakeDb.Tests.Data
{
  using FluentAssertions;
  using Sitecore.Data;
  using Xunit;

  public class BranchRecordsTest
  {
    private readonly ID branchId = ID.NewID;

    [Fact]
    public void ShouldSetBranchId()
    {
      // arrange
      using (var db = new Db
                        {
                          new DbItem("home") { BranchId = this.branchId }
                        })
      {
        // act
        var item = db.GetItem("/sitecore/content/home");

        // assert
        item.BranchId.Should().Be(this.branchId);
      }
    }

    [Fact(Skip = "To be implemented.")]
    public void ShouldCreateBranchIfBranchIdIsSet()
    {
      // act
      using (var db = new Db
                        {
                          new DbItem("home") { BranchId = this.branchId }
                        })
      {
        var branch = db.Database.Branches[this.branchId];

        // assert
        branch.Should().NotBeNull();
        branch.Name.Should().Be("home");
      }
    }

    [Fact]
    public void ShouldResolveAndSetBranchIdIfExists()
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
      // act
      using (var db = new Db
                        {
                          new DbItem("Sample Branch", this.branchId, TemplateIDs.BranchTemplate),
                        })
      {
        // assert
        db.Database.Branches["/sitecore/templates/branches/Sample Branch"].Should().NotBeNull();
        db.Database.Branches[this.branchId].Should().NotBeNull();
      }
    }

    [Fact(Skip = "To be implemented.")]
    public void ShouldCreateItemFromBranch()
    {
      // arrange
      using (var db = new Db { new DbItem("home") })
      {
        var item = db.GetItem("/sitecore/content/home");

        // act
        var child = item.Add("child", new BranchId(this.branchId));

        // assert
        child.BranchId.Should().Be(this.branchId);
        child.TemplateID.Should().NotBe(this.branchId);
      }
    }
  }
}