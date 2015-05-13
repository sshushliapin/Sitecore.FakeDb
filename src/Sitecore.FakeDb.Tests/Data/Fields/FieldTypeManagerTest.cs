namespace Sitecore.FakeDb.Tests.Data.Fields
{
  using FluentAssertions;
  using Sitecore.Data.Fields;
  using Xunit;

  public class FieldTypeManagerTest
  {
    // TODO: Implement Layout and Tracking fields.
    [Theory]
    [InlineData("Checkbox")]
    [InlineData("Date")]
    [InlineData("Datetime")]
    [InlineData("File")]
    [InlineData("Image")]
    [InlineData("Rich Text")]
    [InlineData("Single-Line Text")]
    [InlineData("Word Document")]
    [InlineData("Multi-Line Text")]
    [InlineData("Checklist")]
    [InlineData("Droplist")]
    [InlineData("Grouped Droplink")]
    [InlineData("Grouped Droplist")]
    [InlineData("Multilist")]
    [InlineData("Multilist with Search")]
    [InlineData("Name Value List")]
    [InlineData("Treelist")]
    [InlineData("Treelist with Search")]
    [InlineData("TreelistEx")]
    [InlineData("Droplink")]
    [InlineData("Droptree")]
    [InlineData("General Link")]
    [InlineData("General Link with Search")]
    [InlineData("Version Link")]
    [InlineData("Frame")]
    [InlineData("Rules")]

    // [InlineData("Tracking")]
    [InlineData("Datasource")]
    [InlineData("Custom")]
    [InlineData("Internal Link")]

    // [InlineData("Layout")]
    [InlineData("Template Field Source")]
    [InlineData("File Drop Area")]
    [InlineData("Page Preview")]
    [InlineData("Rendering Datasource")]
    [InlineData("Thumbnail")]
    [InlineData("Security")]
    [InlineData("UserList")]
    [InlineData("html")]
    [InlineData("link")]
    [InlineData("lookup")]
    [InlineData("reference")]
    [InlineData("text")]
    [InlineData("memo")]
    [InlineData("tree")]
    [InlineData("tree list")]
    [InlineData("valuelookup")]
    public void ShouldGetField(string fieldType)
    {
      // arrange
      using (var db = new Db
                        {
                          new DbItem("home") { new DbField("field") { Type = fieldType } }
                        })
      {
        var home = db.GetItem("/sitecore/content/home");

        // act & assert
        FieldTypeManager.GetField(home.Fields["field"]).Should().NotBeNull();
      }
    }
  }
}