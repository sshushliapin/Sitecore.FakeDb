namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Sitecore.Data.Templates;
  using Xunit;
  using Xunit.Extensions;

  public class DbTemplateInheritanceTest
  {
    private readonly DbTemplate _baseTemplateOne;
    private readonly DbTemplate _baseTemplateThree;
    private readonly DbTemplate _baseTemplateTwo;

    public DbTemplateInheritanceTest()
    {
      _baseTemplateOne = new DbTemplate("Base One", ID.NewID) {"Title"};
      _baseTemplateTwo = new DbTemplate("Base Two", ID.NewID) {"Description"};
      _baseTemplateThree = new DbTemplate("Base Three", ID.NewID)
      {
        BaseIDs = new[] { _baseTemplateTwo.ID }
      };
    }

    [Fact]
    public void ShouldPropagateFieldFromTheBaseTemplate()
    {
      // arrange
      ID templateId = ID.NewID;

      using (var db = new Db
      {
        _baseTemplateOne,
        new DbTemplate("My Template", templateId) {BaseIDs = new[] {_baseTemplateOne.ID}},
        new DbItem("home", ID.NewID, templateId)
      })
      {
        // act
        Item home = db.GetItem("/sitecore/content/home");
        Template template = TemplateManager.GetTemplate(templateId, db.Database);
        Template baseTemplate = TemplateManager.GetTemplate(_baseTemplateOne.ID, db.Database);

        TemplateField titleField = baseTemplate.GetField("Title");

        // assert 

        // note: it should "just" work as Sitecore wil do all the looking up the templates chain
        template.GetFields(false).Should().NotContain(f => f.Name == "Title" || f.ID == titleField.ID);
        template.GetFields(true).Should().Contain(f => f.Name == "Title" && f.ID == titleField.ID);

        template.GetField("Title").Should().NotBeNull();
        template.GetField(titleField.ID).Should().NotBeNull();

        home.Fields["Title"].Should().NotBeNull();
        home.Fields[titleField.ID].Should().NotBeNull();
      }
    }

    [Fact]
    public void ShouldPropagateFieldFromAllBaseTemplates()
    {
      // arrange
      ID myTemplateId = ID.NewID;

      using (var db = new Db
      {
        _baseTemplateOne,
        _baseTemplateTwo,
        _baseTemplateThree,
        new DbTemplate("Main Template", myTemplateId) {BaseIDs = new[] {_baseTemplateOne.ID, _baseTemplateThree.ID}},
        new DbItem("home", ID.NewID, myTemplateId)
      })
      {
        // act
        Item home = db.GetItem("/sitecore/content/home");
        Template template = TemplateManager.GetTemplate(myTemplateId, db.Database);

        // assert

        // note: as noted above, fields propagation should "just" work
        home.Fields["Title"].Should().NotBeNull();
        home.Fields["Description"].Should().NotBeNull();

        template.GetField("Title").Should().NotBeNull();
        template.GetField("Description").Should().NotBeNull();
      }
    }

    [Fact]
    public void ShouldHaveTheValueDefinedOnTheItemForTheFieldFromTheBaseTemplates()
    {
      // arrange
      ID templateId = ID.NewID;

      using (var db = new Db
      {
        _baseTemplateOne,
        _baseTemplateTwo,
        _baseTemplateThree,
        new DbTemplate("My Template", templateId)
        {
          BaseIDs = new ID[] {_baseTemplateOne.ID, _baseTemplateThree.ID}
        },
        new DbItem("home", ID.NewID, templateId) {{"Title", "Home"}, {"Description", "My Home"}}
      })
      {
        // act
        Item home = db.GetItem("/sitecore/content/home");

        // assert
        home["Title"].Should().Be("Home");
        home["Description"].Should().Be("My Home");
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

  }
}