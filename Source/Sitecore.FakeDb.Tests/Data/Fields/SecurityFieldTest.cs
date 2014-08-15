namespace Sitecore.FakeDb.Tests.Data.Fields
{
  using FluentAssertions;
  using Xunit;

  public class SecurityFieldTest
  {
    private const string AllowAllAccessRule = "ar|Everyone|p*|+*|";

    [Fact]
    public void ShouldSetDefaultRootItemSecurity()
    {
      // arrange & act
      using (var db = new Db())
      {
        var item = db.GetItem("/sitecore");

        // assert
        item[FieldIDs.Security].Should().Be(AllowAllAccessRule);
      }
    }

    [Fact]
    public void ShouldSetSecurityFieldByName()
    {
      // arrange & act
      using (var db = new Db 
                        {
                          new DbItem("home") { { "__Security", AllowAllAccessRule } }
                        })
      {
        var item = db.Database.GetItem("/sitecore/content/home");

        // assert
        item[FieldIDs.Security].Should().Be(AllowAllAccessRule);
      }
    }

    [Fact]
    public void ShouldSetSecurityFieldById()
    {
      // arrange & act
      using (var db = new Db 
                        {
                          new DbItem("home") { new DbField(FieldIDs.Security) { Value = AllowAllAccessRule } }
                        })
      {
        var item = db.Database.GetItem("/sitecore/content/home");

        // assert
        item[FieldIDs.Security].Should().Be(AllowAllAccessRule);
      }
    }

    [Fact]
    public void ShouldHaveEmptySecurityFieldByDefault()
    {
      // arrange & act
      using (var db = new Db { new DbItem("home") })
      {
        var item = db.Database.GetItem("/sitecore/content/home");

        // assert
        item.Fields[FieldIDs.Security].Should().NotBeNull();
        item.Fields[FieldIDs.Security].Value.Should().BeEmpty();
      }
    }

    [Fact]
    public void ShouldBeShared()
    {
      // arrange & act
      using (var db = new Db())
      {
        var item = db.GetItem("/sitecore");

        // assert
        item.Fields[FieldIDs.Security].Shared.Should().BeTrue();
      }
    }
  }
}
