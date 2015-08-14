namespace Sitecore.FakeDb.Tests.Data.Templates
{
  using System.Linq;
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Xunit;

  public class TemplatesTest
  {
    [Fact]
    public void ShouldGetOwnFields()
    {
      // arrange
      var templateId = ID.NewID;

      using (var db = new Db
                        {
                          new DbTemplate(templateId) { "expected own field" },
                          new DbItem("home", ID.NewID, templateId)
                        })
      {
        var item = db.GetItem("/sitecore/content/home");

        // assert
        item.Should().NotBeNull("the \"Home\" item should not be null");
        item.Template.Should().NotBeNull("the \"Home\" item template should not be null");
        item.Template.OwnFields.Count().Should().Be(1, string.Join("\n", item.Template.OwnFields.Select(f => f.Name)));
        item.Template.OwnFields.Single().Name.Should().Be("expected own field");
      }
    }

    [Theory, AutoData]
    public void ShouldCreateTemplateFieldItemBasedOnFieldId(ID templateId, ID fieldId)
    {
      using (var db = new Db { new DbTemplate(templateId) { fieldId } })
      {
        db.GetItem(fieldId).Should().NotBeNull();
      }
    }

    [Theory, AutoData]
    public void ShouldCreateTemplateFieldItemBasedOnDbField(ID templateId, DbField field)
    {
      using (var db = new Db { new DbTemplate(templateId) { field } })
      {
        db.GetItem(field.ID).Should().NotBeNull();
      }
    }

    [Theory, AutoData]
    public void ShouldCreateTemplateFieldItemWithTypeField(ID templateId, DbField field)
    {
      using (var db = new Db { new DbTemplate(templateId) { field } })
      {
        var templateFieldItem = (TemplateFieldItem)db.GetItem(field.ID);

        templateFieldItem.Type.Should().Be(field.Type);
      }
    }

    [Theory]
    [InlineAutoData(true)]
    [InlineAutoData(false)]
    public void ShouldCreateTemplateFieldItemWithSharedField(bool shared, ID templateId, DbField field)
    {
      field.Shared = shared;

      using (var db = new Db { new DbTemplate(templateId) { field } })
      {
        var templateFieldItem = (TemplateFieldItem)db.GetItem(field.ID);

        templateFieldItem.Shared.Should().Be(shared);
      }
    }
  }
}