namespace Sitecore.FakeDb.Tests.Security.AccessControl
{
  using FluentAssertions;
  using Sitecore.Security.AccessControl;
  using Xunit;

  public class ItemSecurityTest
  {
    [Fact]
    public void ShouldGrantFullAccessToEveryone()
    {
      // arrange & act
      using (var db = new Db())
      {
        var item = db.GetItem("/sitecore");

        // assert
        item.Should().NotBeNull();
        item.Access.CanRead().Should().BeTrue("read");
        item.Access.CanWrite().Should().BeTrue("write");
      }
    }

    [Fact]
    public void ShouldRestrictItemSecurity()
    {
      // arrange
      using (var db = new Db { new DbItem("home") { new DbItem("about") } })
      {
        var item = db.GetItem("/sitecore/content/home");
        var rules = new AccessRuleCollection 
          {
            AccessRule.Create(Context.User, AccessRight.ItemRead, PropagationType.Descendants, AccessPermission.Deny)
          };

        // act
        AuthorizationManager.SetAccessRules(item, rules);

        // assert
        Assert.NotNull(db.GetItem("/sitecore/content/home"));
        Assert.Null(db.GetItem("/sitecore/content/home/about"));
      }
    }
  }
}