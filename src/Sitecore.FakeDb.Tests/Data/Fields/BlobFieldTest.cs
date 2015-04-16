namespace Sitecore.FakeDb.Tests.Data.Fields
{
  using System.IO;
  using FluentAssertions;
  using Sitecore.Data.Items;
  using Xunit;

  public class BlobFieldTest
  {
    [Fact]
    public void ShouldSetAndGetBlobStream()
    {
      // arrange
      var stream = new MemoryStream();

      using (var db = new Db { new DbItem("home") { new DbField("field") } })
      {
        var item = db.GetItem("/sitecore/content/home");
        var field = item.Fields["field"];

        using (new EditContext(item))
        {
          // act
          field.Should().NotBeNull("'item.Fields[\"field\"]' should not be null");
          field.SetBlobStream(stream);
        }

        // assert
        field.GetBlobStream().Should().BeSameAs(stream);
      }
    }
  }
}