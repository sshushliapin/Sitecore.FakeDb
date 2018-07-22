namespace Sitecore.FakeDb.Tests.Data.Fields
{
  using FluentAssertions;
  using global::AutoFixture.Xunit2;
  using Xunit;

  [Trait("Category", "RequireLicense")]
  public class StandardFieldsTest
  {
    [Theory]
    [InlineAutoData("__Final Renderings")]
    [InlineAutoData("__Page Level Test Set Definition")]
    public void ShouldReadStandardFieldValues(string fieldName, string fieldValue)
    {
      // arrange
      using (var db = new Db
                        {
                          new DbItem("home") { { fieldName, fieldValue } }
                        })
      {
        // act & assert
        var item = db.GetItem("/sitecore/content/home");
        item.Should().NotBeNull();

        item[fieldName].Should().Be(fieldValue);
      }
    }
  }
}