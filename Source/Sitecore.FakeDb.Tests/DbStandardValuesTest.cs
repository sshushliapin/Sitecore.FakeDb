namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Xunit;
  using Xunit.Extensions;
  using Sitecore.Data;
  using Sitecore.Data.Managers;

  public class DbStandardValuesTest
  {
    private readonly ID templateId = ID.NewID;

    [Theory]
    [InlineData("$name", "Home")]
    [InlineData("static-text", "static-text")]
    public void ShouldCreateTemplateWithStandardValues(string standardValue, string expectedValue)
    {
      // arrange
      using (var db = new Db { new DbTemplate("sample", templateId) { { "Title", standardValue } } })
      {
        var root = db.GetItem(ItemIDs.ContentRoot);

        // act
        var item = ItemManager.CreateItem("Home", root, templateId);

        // assert
        item["Title"].Should().Be(expectedValue);
      }
    }

    [Theory]
    [InlineData("Value", "Value")]
    [InlineData("$name", "home")]
    public void ShouldSetStandardValueFromTheBaseTemplate(string value, string expectation)
    {
      // arrange
      var baseTemplateId = ID.NewID;
      var templateId = ID.NewID;

      using (var db = new Db
      {
        new DbTemplate("Base Template", baseTemplateId) {{"Title", value}},
        new DbTemplate("My Template", templateId) {BaseIDs = new ID[] {baseTemplateId}},
        new DbItem("home", ID.NewID, templateId)
      })
      {
        // act
        var home = db.GetItem("/sitecore/content/home");

        // assert
        home["Title"].Should().Be(expectation);
      }
    }

    [Fact]
    public void ShouldUseTheClosestStandardValue()
    {
      // arrange
      var baseTemplate = new DbTemplate() { { "Title", "Default" } };
      var myTemplate = new DbTemplate() { BaseIDs = new ID[] { baseTemplate.ID } };
      myTemplate.Add("Title", "New Default");

      using (var db = new Db
      {
        baseTemplate,
        myTemplate,
        new DbItem("home", ID.NewID, myTemplate.ID)
      })
      {
        // act
        var home = db.GetItem("/sitecore/content/home");

        // assert
        home["Title"].Should().Be("New Default");
      }
    }
  }

}
