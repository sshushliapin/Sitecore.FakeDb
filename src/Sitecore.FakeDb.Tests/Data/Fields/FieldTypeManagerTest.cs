namespace Sitecore.FakeDb.Tests.Data.Fields
{
  using FluentAssertions;
  using Sitecore.Data.Fields;
  using Xunit;

  public class FieldTypeManagerTest
  {
    [Fact]
    public void ShouldGetField()
    {
      // arrange
      using (var db = new Db
                        {
                          new DbItem("home")
                            {
                              new DbField("Is Active") { Type = "Checkbox", Value = "1" }
                            }
                        })
      {
        var home = db.GetItem("/sitecore/content/home");

        // act
        var customField = FieldTypeManager.GetField(home.Fields["Is Active"]);

        // assert
        customField.Should().NotBeNull();
        customField.Value.Should().Be("1");
      }
    }
  }
}