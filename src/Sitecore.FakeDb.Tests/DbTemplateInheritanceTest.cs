namespace Sitecore.FakeDb.Tests
{
  using System;
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Sitecore.Data.Templates;
  using Sitecore.Exceptions;
  using Xunit;

  public class DbTemplateInheritanceTest
  {
    private readonly DbTemplate baseTemplateOne;

    private readonly DbTemplate baseTemplateThree;

    private readonly DbTemplate baseTemplateTwo;

    public DbTemplateInheritanceTest()
    {
      this.baseTemplateOne = new DbTemplate("Base One", ID.NewID) { "Title" };
      this.baseTemplateTwo = new DbTemplate("Base Two", ID.NewID) { "Description" };
      this.baseTemplateThree = new DbTemplate("Base Three", ID.NewID)
        {
          BaseIDs = new[] { this.baseTemplateTwo.ID }
        };
    }

    [Fact]
    public void ShouldPropagateFieldsFromTheBaseTemplate()
    {
      // arrange
      ID templateId = ID.NewID;

      using (var db = new Db
      {
        this.baseTemplateOne,
        new DbTemplate("My Template", templateId) {BaseIDs = new[] {this.baseTemplateOne.ID}},
        new DbItem("home", ID.NewID, templateId)
      })
      {
        // act
        Item home = db.GetItem("/sitecore/content/home");
        Template template = TemplateManager.GetTemplate(templateId, db.Database);
        Template baseTemplate = TemplateManager.GetTemplate(this.baseTemplateOne.ID, db.Database);

        TemplateField titleField = baseTemplate.GetField("Title");

        // assert 

        // note: it should "just" work as Sitecore wil do all the looking up the templates chain
        template.GetFields(false).Should().NotContain(f => f.Name == "Title" || f.ID == titleField.ID);
        template.GetFields(true).Should().Contain(f => f.Name == "Title" && f.ID == titleField.ID);

        template.GetField("Title").Should().NotBeNull("template.GetField(\"Title\")");
        template.GetField(titleField.ID).Should().NotBeNull("template.GetField(titleField.ID)");

        home.Fields["Title"].Should().NotBeNull("home.Fields[\"Title\"]");
        home.Fields[titleField.ID].Should().NotBeNull("home.Fields[titleField.ID]");
      }
    }

    [Fact]
    public void ShouldPropagateFieldsFromAllBaseTemplates()
    {
      // arrange
      ID myTemplateId = ID.NewID;

      using (var db = new Db
      {
        this.baseTemplateOne,
        this.baseTemplateTwo,
        this.baseTemplateThree,
        new DbTemplate("Main Template", myTemplateId) {BaseIDs = new[] {this.baseTemplateOne.ID, this.baseTemplateThree.ID}},
        new DbItem("home", ID.NewID, myTemplateId)
      })
      {
        // act
        Item home = db.GetItem("/sitecore/content/home");
        Template template = TemplateManager.GetTemplate(myTemplateId, db.Database);

        // assert

        // note: as noted above, fields propagation should "just" work
        home.Fields["Title"].Should().NotBeNull("home.Fields[\"Title\"]");
        home.Fields["Description"].Should().NotBeNull("home.Fields[\"Description\"]");

        template.GetField("Title").Should().NotBeNull("template.GetField(\"Title\")");
        template.GetField("Description").Should().NotBeNull("template.GetField(\"Description\")");
      }
    }

    [Fact]
    public void ShouldHaveTheValueDefinedOnTheItemForTheFieldsFromTheBaseTemplates()
    {
      // arrange
      ID templateId = ID.NewID;

      using (var db = new Db
      {
        this.baseTemplateOne,
        this.baseTemplateTwo,
        this.baseTemplateThree,
        new DbTemplate("My Template", templateId)
        {
          BaseIDs = new ID[] {this.baseTemplateOne.ID, this.baseTemplateThree.ID}
        },
        new DbItem("home", ID.NewID, templateId) { { "Title", "Home" }, { "Description", "My Home" } }
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
    [AutoData]
    public void ShouldEditEmptyInheritedField(ID baseTemplateId, ID templateId, ID fieldId)
    {
      // arrange
      using (var db = new Db
                        {
                          new DbTemplate("base", baseTemplateId) { new DbField(fieldId) },
                          new DbTemplate("sample", templateId) { BaseIDs = new[] { baseTemplateId } },
                          new DbItem("Home", ID.NewID, templateId)
                        })
      {
        var item = db.GetItem("/sitecore/content/Home");

        // act
        using (new EditContext(item))
        {
          item.Fields[fieldId].Value = "new value";
        }

        // assert
        item.Fields[fieldId].Value.Should().Be("new value");
      }
    }

    [Theory]
    [AutoData]
    public void ShouldIgnoreBaseTemplateIfNull(ID templateId)
    {
      // arrange
      using (var db = new Db
                  {
                    new DbTemplate(templateId) { BaseIDs = new[] { ID.Null } },
                    new DbItem("home", ID.NewID, templateId)
                  })
      {
        // act
        Action action = () => db.GetItem("/sitecore/content/home");

        // assert
        action.ShouldNotThrow();
      }
    }

    [Theory]
    [AutoData]
    public void ShouldThrowIfBaseTemplateIsMissing(ID templateId)
    {
      // arrange
      const string MissingBaseTemplateId = "{4F2BBCE8-92EC-4514-8A5F-2C1F432FEE5A}";

      using (var db = new Db
                  {
                    new DbTemplate(templateId) { BaseIDs = new[] { new ID(MissingBaseTemplateId) } },
                    new DbItem("home", ID.NewID, templateId)
                  })
      {
        // act
        Action action = () => db.GetItem("/sitecore/content/home");

        // assert
        action.ShouldThrow<TemplateNotFoundException>().WithMessage("The template \"{4F2BBCE8-92EC-4514-8A5F-2C1F432FEE5A}\" was not found.");
      }
    }
  }
}