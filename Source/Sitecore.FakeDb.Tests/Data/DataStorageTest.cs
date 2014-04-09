namespace Sitecore.FakeDb.Tests.Data
{
  using System;
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.Globalization;
  using Xunit;
  using Xunit.Extensions;

  public class DataStorageTest
  {
    private readonly DataStorage dataStorage;

    private const string ItemIdsRootId = "{11111111-1111-1111-1111-111111111111}";

    private const string ItemIdsContentRoot = "{0DE95AE4-41AB-4D01-9EB0-67441B7C2450}";

    private const string ItemIdsTemplateRoot = "{3C1715FE-6A13-4FCF-845F-DE308BA9741D}";

    private const string TemplateIdsTemplate = "{AB86861A-6030-46C5-B394-E8F99E8B87DB}";

    private const string ItemIdsTemplateSection = "{E269FBB5-3750-427A-9149-7AA950B49301}";

    private const string ItemIdsTemplateField = "{455A3E98-A627-4B40-8035-E683A0331AC7}";

    private const string TemplateIdsBranch = "{35E75C72-4985-4E09-88C3-0EAC6CD1E64F}";

    private const string RootParentId = "{00000000-0000-0000-0000-000000000000}";

    public DataStorageTest()
    {
      this.dataStorage = new DataStorage(Database.GetDatabase("master"));
    }

    [Theory]
    [InlineData(ItemIdsRootId, "sitecore", "{C6576836-910C-4A3D-BA03-C277DBD3B827}", RootParentId, "/sitecore")]
    [InlineData(ItemIdsContentRoot, "content", "{E3E2D58C-DF95-4230-ADC9-279924CECE84}", ItemIdsRootId, "/sitecore/content")]
    [InlineData(ItemIdsTemplateRoot, "templates", "{E3E2D58C-DF95-4230-ADC9-279924CECE84}", ItemIdsRootId, "/sitecore/templates")]
    [InlineData(TemplateIdsTemplate, "Template", TemplateIdsTemplate, ItemIdsTemplateRoot, "/sitecore/templates/template")]
    [InlineData(ItemIdsTemplateSection, "Template section", TemplateIdsTemplate, ItemIdsTemplateRoot, "/sitecore/templates/template section")]
    [InlineData(ItemIdsTemplateField, "Template field", TemplateIdsTemplate, ItemIdsTemplateRoot, "/sitecore/templates/template field")]
    [InlineData(TemplateIdsBranch, "Branch", TemplateIdsTemplate, ItemIdsTemplateRoot, "/sitecore/templates/branch")]
    public void ShouldInitializeDefaultFakeItems(string itemId, string itemName, string templateId, string parentId, string fullPath)
    {
      // assert
      this.dataStorage.FakeItems[ID.Parse(itemId)].ID.ToString().Should().Be(itemId);
      this.dataStorage.FakeItems[ID.Parse(itemId)].Name.Should().Be(itemName);
      this.dataStorage.FakeItems[ID.Parse(itemId)].TemplateID.ToString().Should().Be(templateId);
      this.dataStorage.FakeItems[ID.Parse(itemId)].ParentID.ToString().Should().Be(parentId);
      this.dataStorage.FakeItems[ID.Parse(itemId)].FullPath.Should().Be(fullPath);
    }

    [Fact]
    public void ShouldCreateDefaultFakeTemplate()
    {
      this.dataStorage.FakeTemplates[TemplateIDs.Template].Should().BeEquivalentTo(new DbTemplate("Template", TemplateIDs.Template));
      this.dataStorage.FakeTemplates[TemplateIDs.Folder].Should().BeEquivalentTo(new DbTemplate("Folder", TemplateIDs.Folder));
    }

    [Fact]
    public void ShouldGetExistingItem()
    {
      // act & assert
      this.dataStorage.GetFakeItem(ItemIDs.ContentRoot).Should().NotBeNull();
      this.dataStorage.GetFakeItem(ItemIDs.ContentRoot).Should().BeOfType<DbItem>();

      this.dataStorage.GetSitecoreItem(ItemIDs.ContentRoot, Language.Current).Should().NotBeNull();
      this.dataStorage.GetSitecoreItem(ItemIDs.ContentRoot, Language.Current).Should().BeAssignableTo<Item>();
    }

    [Fact]
    public void ShouldGetNullIdIfNoItemPresent()
    {
      // act & assert
      this.dataStorage.GetFakeItem(ID.NewID).Should().BeNull();
      this.dataStorage.GetSitecoreItem(ID.NewID, Language.Current).Should().BeNull();
    }

    [Fact]
    public void ShouldGetFieldListByTemplateId()
    {
      // arrange
      var templateId = ID.NewID;
      var field1 = new DbField("Title");
      var field2 = new DbField("Title");

      var template = new DbTemplate() { ID = templateId, Fields = { field1, field2 } };

      this.dataStorage.FakeTemplates.Add(templateId, template);

      // act
      var fieldList = this.dataStorage.GetFieldList(template.ID);

      // assert
      fieldList.Count.Should().Be(2);
      fieldList[field1.ID].Should().BeEmpty();
      fieldList[field2.ID].Should().BeEmpty();
    }

    [Fact]
    public void ShouldThrowExceptionIfNoTemplateFound()
    {
      // arrange
      var missingTemplateId = new ID("{C4520D42-33CA-48C7-972D-6CEE1BC4B9A6}");

      // act
      Action a = () => this.dataStorage.GetFieldList(missingTemplateId);

      // assert
      a.ShouldThrow<InvalidOperationException>().WithMessage("Template \'{C4520D42-33CA-48C7-972D-6CEE1BC4B9A6}\' not found.");
    }
  }
}