namespace Sitecore.FakeDb.Tests.Data.Fields
{
  using System;
  using FluentAssertions;
  using Sitecore.Analytics.Data;
  using Sitecore.Data.Fields;
  using Xunit;

  public class FieldTypeManagerTest
  {
    [Theory]

    // Simple Types
    [InlineData("Checkbox", typeof(CheckboxField))]
    [InlineData("Date", typeof(DateField))]
    [InlineData("Datetime", typeof(DateField))]
    [InlineData("File", typeof(FileField))]
    [InlineData("Image", typeof(ImageField))]
    [InlineData("Rich Text", typeof(HtmlField))]
    [InlineData("Single-Line Text", typeof(TextField))]
    [InlineData("Word Document", typeof(WordDocumentField))]
    [InlineData("Multi-Line Text", typeof(TextField))]

    // List Types
    [InlineData("Checklist", typeof(MultilistField))]
    [InlineData("Droplist", typeof(ValueLookupField))]
    [InlineData("Grouped Droplink", typeof(GroupedDroplinkField))]
    [InlineData("Grouped Droplist", typeof(GroupedDroplistField))]
    [InlineData("Multilist", typeof(MultilistField))]
    [InlineData("Multilist with Search", typeof(MultilistField))]
    [InlineData("Name Value List", typeof(NameValueListField))]
    [InlineData("Treelist", typeof(MultilistField))]
    [InlineData("Treelist with Search", typeof(MultilistField))]
    [InlineData("TreelistEx", typeof(MultilistField))]

    // Link Types
    [InlineData("Droplink", typeof(LookupField))]
    [InlineData("Droptree", typeof(ReferenceField))]
    [InlineData("General Link", typeof(LinkField))]
    [InlineData("General Link with Search", typeof(LinkField))]
    [InlineData("Version Link", typeof(VersionLinkField))]

    // Developer Types
    [InlineData("Frame", typeof(TextField))]
    [InlineData("Rules", typeof(RulesField))]

    // System Types 
    [InlineData("Datasource", typeof(DatasourceField))]
    [InlineData("Custom", typeof(CustomCustomField))]
    [InlineData("Internal Link", typeof(InternalLinkField))]
    [InlineData("Template Field Source", typeof(TemplateFieldSourceField))]
    [InlineData("File Drop Area", typeof(FileDropAreaField))]
    [InlineData("Page Preview", typeof(PagePreviewField))]

    // [InlineData("Rendering Datasource", typeof(RenderingDatasourceField))]
    [InlineData("Thumbnail", typeof(ThumbnailField))]
    [InlineData("Security", typeof(TextField))]
    [InlineData("UserList", typeof(TextField))]
    public void ShouldGetField(string name, Type type)
    {
      // arrange
      using (var db = new Db
                        {
                          new DbItem("home") { new DbField("field") { Type = name } }
                        })
      {
        var home = db.GetItem("/sitecore/content/home");

        // act & assert
        FieldTypeManager.GetField(home.Fields["field"]).Should().BeOfType(type);
        FieldTypeManager.GetFieldType(name).Type.Should().Be(type);
      }
    }

    [Fact]
    public void ShouldGetLayoutField()
    {
      // arrange
      using (var db = new Db { new DbItem("home") })
      {
        var home = db.GetItem("/sitecore/content/home");

        // act & assert
        FieldTypeManager.GetField(home.Fields[FieldIDs.LayoutField]).Should().BeOfType<LayoutField>();
        FieldTypeManager.GetFieldType("Layout").Type.Should().Be<LayoutField>();
      }
    }

    [Fact]
    public void ShouldGetTrackingField()
    {
      // arrange
      using (var db = new Db { new DbItem("home") })
      {
        var home = db.GetItem("/sitecore/content/home");

        // act & assert
        FieldTypeManager.GetField(home.Fields["__Tracking"]).Should().BeOfType<TrackingField>();
        FieldTypeManager.GetFieldType("Tracking").Type.Should().Be<TrackingField>();
      }
    }
  }
}