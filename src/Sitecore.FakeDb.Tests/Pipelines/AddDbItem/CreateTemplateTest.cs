namespace Sitecore.FakeDb.Tests.Pipelines.AddDbItem
{
  using System.Linq;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Managers;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Pipelines.AddDbItem;
  using Xunit;

  public class CreateTemplateTest
  {
    private const string NullTemplateId = "{00000000-0000-0000-0000-000000000000}";

    private const string SomeTemplateId = "{F445AA3F-2EFC-4E84-9719-B7729C440054}";

    private const string SitecoreBranchTemplateId = "{35E75C72-4985-4E09-88C3-0EAC6CD1E64F}";

    private readonly CreateTemplate processor;

    private readonly DataStorage dataStorage;

    public CreateTemplateTest()
    {
      this.dataStorage = Substitute.For<DataStorage>(Database.GetDatabase("master"));
      this.processor = new CreateTemplate();
    }

    [Theory]
    [InlineData(NullTemplateId, SitecoreBranchTemplateId)]
    [InlineData(SomeTemplateId, SomeTemplateId)]
    public void ShouldSetTemplateIdToBranchTemplateIdIfParentIsBrancesAndNoTemplateIdSet(string originalTemplateId, string expectedNewTemlateId)
    {
      // arrange
      var branchItem = new DbItem("branch item") { ParentID = ItemIDs.BranchesRoot, TemplateID = ID.Parse(originalTemplateId) };
      var args = new AddDbItemArgs(branchItem, this.dataStorage);

      // act
      this.processor.Process(args);

      // assert
      branchItem.TemplateID.Should().Be(ID.Parse(expectedNewTemlateId));
    }

    [Fact]
    public void ShouldSetBranchIdAndTemplateIdIfBranchResolved()
    {
      // arrange
      var branchId = ID.NewID;
      var branch = new DbItem("branch", branchId) { ParentID = ItemIDs.BranchesRoot, TemplateID = TemplateIDs.BranchTemplate };

      this.dataStorage.GetFakeItem(branchId).Returns(branch);

      var item = new DbItem("item from branch", ID.NewID, branchId);

      // act
      this.processor.Process(new AddDbItemArgs(item, this.dataStorage));

      // assert
      item.BranchId.Should().Be(branchId);
    }

    [Fact]
    public void ShouldNotReuseSiblingTemplateIfTemplateIdSpecified()
    {
      // arrange
      var myId = ID.NewID;

      // act
      using (var db = new Db
      {
          new DbTemplate("Site Root", myId),
          new DbItem("site", ID.NewID, myId)
          {
              new DbItem("home")
          },
          new DbItem("outside")
      })
      {
        // assert
        var home = db.GetItem("/sitecore/content/site/home");
        var outside = db.GetItem("/sitecore/content/outside");

        outside.TemplateID.Should().NotBe(myId); // <-- Fails
      }
    }

    [Fact]
    public void ShouldBeAbleToReadTemplateFieldsViaTemplateAndTemplateItem()
    {
      // arrange
      var titleFieldId = ID.NewID;
      var descriptionFieldId = ID.NewID;

      // act
      using (var db = new Db
      {
        new DbItem("home")
        {
          new DbField("Title", titleFieldId) { Value = "Title" },
          new DbField("Description", descriptionFieldId) { Value = "Description" }
        }
      })
      {
        var home = db.GetItem("/sitecore/content/home");

        // assert

        // via Template API 
        var template = TemplateManager.GetTemplate(home.TemplateID, db.Database);
        template.GetFields(true).Count(f => f.Name == "Title").Should().Be(1);
        template.GetFields(true).Count(f => f.Name == "Description").Should().Be(1);
        template.GetFields(true).Count(f => f.ID == titleFieldId).Should().Be(1);
        template.GetFields(true).Count(f => f.ID == descriptionFieldId).Should().Be(1);

        // via TemplateItem API
        home.Template.Fields.Count(f => f.Name == "Title").Should().Be(1);
        home.Template.Fields.Count(f => f.Name == "Description").Should().Be(1);
        home.Template.Fields.Count(f => f.ID == titleFieldId).Should().Be(1);
        home.Template.Fields.Count(f => f.ID == descriptionFieldId).Should().Be(1);
      }
    }

    [Fact]
    public void ShouldPropagateFieldSourceToTemplateFieldsForNonGeneratedTemplates()
    {
      // arrange
      var templateId = ID.NewID;
      using (var db = new Db()
      {
        new DbTemplate("Page", templateId) {new DbField("Reference") {Source = "/sitecore"}},
        new DbItem("page", ID.NewID, templateId) {{"Reference", "set"}},
      })
      {
        // act
        var page = db.GetItem("/sitecore/content/page");
        var pageTemplate = TemplateManager.GetTemplate(templateId, db.Database);

        // assert
        pageTemplate.GetFields(true).Single(f => f.Name == "Reference").Source.Should().Be("/sitecore");
        page.Template.Fields.Single(f => f.Name == "Reference").Source.Should().Be("/sitecore");
        page.Fields["Reference"].Source.Should().Be("/sitecore");
      }
    }

    [Fact]
    public void ShouldPropagateFieldSourceToTemplateFieldsForGeneratedTemplates()
    {
      // arrange
      using (var db = new Db()
      {
        new DbItem("home") { new DbField("Reference") { Source = "/sitecore" } }
      })
      {
        // act
        var home = db.GetItem("/sitecore/content/home");
        var homeTemplate = TemplateManager.GetTemplate(home.TemplateID, db.Database);

        // assert
        homeTemplate.GetFields(true).Single(f => f.Name == "Reference").Source.Should().Be("/sitecore");
        home.Template.Fields.Single(f => f.Name == "Reference").Source.Should().Be("/sitecore");
        home.Fields["Reference"].Source.Should().Be("/sitecore");
      }
    }

    [Fact]
    public void ShouldNotBreakOverStandardFieldsWhenBuildingATemplate()
    {
      using (var db = new Db()
      {
        new DbItem("home")
        {
          {Sitecore.FieldIDs.DefaultWorkflow, ""},
          new DbItem("page")
          {
            {Sitecore.FieldIDs.DefaultWorkflow, ""}, // <-- Fails Here
            {Sitecore.FieldIDs.Workflow, ""},
          }
        }
      })
      {
        // ...
      }
    }
  }
}