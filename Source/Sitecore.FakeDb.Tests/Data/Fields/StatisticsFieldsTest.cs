namespace Sitecore.FakeDb.Tests.Data.Fields
{
  using FluentAssertions;
  using Sitecore.Data;
  using Xunit;

  public class StatisticsFieldsTest
  {
    [Fact]
    public void ShouldSetStatisticsFields()
    {
      // arrange & act
      using (var db = new Db { new DbItem("Home") })
      {
        var item = db.GetItem("/sitecore/content/home");

        // assert
        item["__Created"].Should().StartWith(DateUtil.IsoNow.Substring(0, 11));
        item["__Created by"].Should().Be(@"default\Anonymous");
        ID.IsID(item["__Revision"]).Should().BeTrue();
        item["__Updated"].Should().StartWith(DateUtil.IsoNow.Substring(0, 11));
        item["__Updated by"].Should().Be(@"default\Anonymous");
      }
    }

    [Fact]
    public void ShouldNotOverrideStatisticsIfSetExplicitly()
    {
      // arrange
      const string Created = "20080407T135900";
      const string CreatedBy = @"sitecore\admin";
      const string Revision = "17be5e4c-19ac-4a67-b582-d72eaa761b1c";
      const string Updated = "20140613T111309:635382547899455734";
      const string UpdatedBy = @"sitecore\editor";

      // act
      using (var db = new Db
                        {
                          new DbItem("Home")
                            {
                              { "__Created", Created }, { "__Created by", CreatedBy },
                              { "__Revision", Revision },
                              { "__Updated", Updated }, { "__Updated by", UpdatedBy }
                            }
                        })
      {
        var item = db.GetItem("/sitecore/content/home");

        // assert
        item["__Created"].Should().Be(Created);
        item["__Created by"].Should().Be(CreatedBy);
        item["__Revision"].Should().Be(Revision);
        item["__Updated"].Should().Be(Updated);
        item["__Updated by"].Should().Be(UpdatedBy);
      }
    }
  }
}