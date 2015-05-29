namespace Sitecore.FakeDb.Tests.Data.Fields
{
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Xunit;

  public class StandardFieldsTest
  {
    [Theory]
    [InlineAutoData("__Final Renderings")]
    public void ShouldReadStandardFieldValues(string fieldName, string fieldValue, Db db)
    {
      // arrange
      db.Add(new DbItem("home") { { fieldName, fieldValue } });

      // act & assert
      db.GetItem("/sitecore/content/home")[fieldName].Should().Be(fieldValue);
    }
  }
}