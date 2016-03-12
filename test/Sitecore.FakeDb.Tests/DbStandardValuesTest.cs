namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Managers;
  using Xunit;

  public class DbStandardValuesTest
  {
    private readonly ID templateId = ID.NewID;

    [Theory]
    [InlineData("$name", "Home")]
    [InlineData("static-text", "static-text")]
    public void ShouldCreateTemplateWithStandardValues(string standardValue, string expectedValue)
    {
      // arrange
      using (var db = new Db
                        {
                          new DbTemplate("sample", this.templateId) { { "Title", standardValue } }
                        })
      {
        var root = db.GetItem(ItemIDs.ContentRoot);

        // act
        var item = ItemManager.CreateItem("Home", root, this.templateId);

        // assert
        var fieldId = TemplateManager.GetFieldId("Title", this.templateId, db.Database);
        fieldId.Should().NotBeNull("'fieldId' should not be null");
        item.Fields["Title"].Should().NotBeNull("'item.Fields[\"Title\"]' should not be null");
        item.InnerData.Fields[fieldId].Should().BeNull("'item.InnerData.Fields[fieldId]' should be null");

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

      using (var db = new Db
                        {
                          new DbTemplate("Base Template", baseTemplateId) { { "Title", value } },
                          new DbTemplate("My Template", this.templateId) { BaseIDs = new[] { baseTemplateId } },
                          new DbItem("home", ID.NewID, this.templateId)
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
      var baseTemplate = new DbTemplate { { "Title", "Default" } };
      var myTemplate = new DbTemplate { BaseIDs = new[] { baseTemplate.ID } };
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

    [Fact]
    public void ShouldBeAbleToAskForTheItemValueAndTheStandardValue()
    {
      // arrange
      using (var db = new Db
                        {
                          new DbTemplate(this.templateId) { { "Title", "Value" } },
                          new DbItem("home", ID.NewID, this.templateId)
                        })
      {
        // act
        var home = db.GetItem("/sitecore/content/home");
        var title = home.Fields["Title"];
        var fieldId = TemplateManager.GetFieldId("Title", this.templateId, db.Database);

        // assert
        fieldId.Should().NotBeNull("'fieldId' should not be null");
        home.Fields[fieldId].Should().NotBeNull("'home.Fields[fieldId]' should not be null");
        home.Fields["Title"].Should().NotBeNull("'home.Fields[\"Title\"]' should not be null");

        home.InnerData.Fields[fieldId].Should().BeNull("'home.InnerData.Fields[fieldId]' should be null");

        home.Fields["Title"].Value.Should().Be("Value");
        home["Title"].Should().Be("Value");

        title.GetValue(false, false).Should().BeNull();
        title.GetValue(true, true).Should().Be("Value");
      }
    }
  }
}
