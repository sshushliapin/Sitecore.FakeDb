namespace Sitecore.FakeDb.Tests
{
  using System;
  using FluentAssertions;
  using global::AutoFixture.Xunit2;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Sitecore.Data.Templates;
  using Sitecore.Exceptions;
  using Xunit;

  [Trait("Category", "RequireLicense")]
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

    [Theory, AutoData]
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

    [Theory, AutoData]
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

    [Theory, AutoData]
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

    [Theory, AutoData]
    public void ShouldGetEmptyBaseTemplatesCollectionForStandardTemplate(Db db)
    {
      // act
      var standardTemplate = (TemplateItem)db.GetItem(TemplateIDs.StandardTemplate);

      // assert
      standardTemplate.BaseTemplates.Should().BeEmpty();
    }

    // https://github.com/sergeyshushlyapin/Sitecore.FakeDb/issues/165
    [Theory, AutoData]
    public void EditItemsSharedInheritenceFail(
      ID field1Id,
      ID field2Id,
      ID field3Id,
      ID template1Id,
      ID template2Id,
      ID template3Id,
      ID template4Id,
      ID item1Id,
      ID item2Id,
      ID item3Id)
    {
      var field1 = new DbField("Field One", field1Id);
      var field2 = new DbField("Field Two", field2Id);

      using (var db = new Db())
      {
        db.Add(new DbTemplate("Template One", template1Id)
          {
            field1
          });
        db.Add(new DbTemplate("Template Two", template2Id)
          {
            field2
          });
        db.Add(new DbTemplate("Template Three", template3Id)
        {
          BaseIDs = new[] { template1Id }
        });
        db.Add(new DbTemplate("Template Four", template4Id)
        {
          BaseIDs = new[] { template1Id, template2Id }
        });

        db.Add(new DbItem("Item One", item1Id, template3Id));
        db.Add(new DbItem("Item Two", item2Id, template4Id));

        var item1 = db.GetItem(item1Id);
        item1.Editing.BeginEdit();
        item1[field1Id] = "Value One";
        item1.Editing.EndEdit();

        var item2 = db.GetItem(item2Id);
        item2.Editing.BeginEdit();
        item2["Field One"] = "Value One";
        item2["Field Two"] = "Value Two";

        // next line throws "Item field not found. Item: 'Item Two', '{046655DC-7E41-4BDC-AA78-694524CCADC3}'; field: '{C28F483D-1A31-4EA0-A990-028D76B57F1B}'."
        item2.Editing.EndEdit();
      }
    }
  }
}