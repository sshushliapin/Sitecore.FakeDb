namespace Sitecore.FakeDb.Tests.Data.Fields
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Xunit;

  public class ReferenceFieldTest
  {
    [Fact]
    public void ShouldGetTargetByPath()
    {
      // arrange
      var targetId = ID.NewID;

      using (var db = new Db
                        { 
                           new DbItem("source") { { "Reference", "/sitecore/content/target" } },
                           new DbItem("target",  targetId)
                        })
      {
        var item = db.GetItem("/sitecore/content/source");
        item.Should().NotBeNull("the 'source' item should not be null");

        // act
        var referenceField = (ReferenceField)item.Fields["Reference"];

        // assert
        referenceField.Should().NotBeNull("'item.Fields[\"Reference\"]' should not be null");
        referenceField.Database.Should().Be(db.Database);
        referenceField.Path.Should().Be("/sitecore/content/target");
        referenceField.TargetID.Should().Be(targetId);
        referenceField.TargetItem.Should().Be(db.GetItem(targetId));
      }
    }
    [Fact]
    public void ShouldGetTargetById()
    {
      // arrange
      var targetId = ID.NewID;

      using (var db = new Db
                        { 
                           new DbItem("source") { { "Reference", targetId.ToString() } },
                           new DbItem("target",  targetId)
                        })
      {
        var item = db.GetItem("/sitecore/content/source");
        item.Should().NotBeNull("the 'source' item should not be null");

        // act
        var referenceField = (ReferenceField)item.Fields["Reference"];

        // assert
        referenceField.Should().NotBeNull("'item.Fields[\"Reference\"]' should not be null");
        referenceField.Database.Should().Be(db.Database);
        referenceField.Path.Should().Be(targetId.ToString());
        referenceField.TargetID.Should().Be(targetId);
        referenceField.TargetItem.Should().Be(db.GetItem(targetId));
      }
    }
  }
}