namespace Sitecore.FakeDb.Tests.Data.Engines
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.Globalization;
  using Xunit;

  public class DataStorageTest
  {
    private readonly DataStorage dataStorage;

    private const string ItemIdsRootId = "{11111111-1111-1111-1111-111111111111}";

    private const string ItemIdsContentRoot = "{0DE95AE4-41AB-4D01-9EB0-67441B7C2450}";

    private const string ItemIdsTemplateRoot = "{3C1715FE-6A13-4FCF-845F-DE308BA9741D}";

    private const string ItemIdsBranchesRoot = "{BAD98E0E-C1B5-4598-AC13-21B06218B30C}";

    private const string ItemIdsSystemRoot = "{13D6D6C6-C50B-4BBD-B331-2B04F1A58F21}";

    private const string ItemIdsMediaLibraryRoot = "{3D6658D8-A0BF-4E75-B3E2-D050FABCF4E1}";

    private const string TemplateIdsTemplate = "{AB86861A-6030-46C5-B394-E8F99E8B87DB}";

    private const string ItemIdsTemplateSection = "{E269FBB5-3750-427A-9149-7AA950B49301}";

    private const string ItemIdsTemplateField = "{455A3E98-A627-4B40-8035-E683A0331AC7}";

    private const string TemplateIdsBranch = "{35E75C72-4985-4E09-88C3-0EAC6CD1E64F}";

    private const string RootParentId = "{00000000-0000-0000-0000-000000000000}";

    private const string TemplateIdSitecore = "{C6576836-910C-4A3D-BA03-C277DBD3B827}";

    public const string TemplateIdMainSection = "{E3E2D58C-DF95-4230-ADC9-279924CECE84}";

    public const string TemplateIdBranchFolder = "{85ADBF5B-E836-4932-A333-FE0F9FA1ED1E}";

    public DataStorageTest()
    {
      this.dataStorage = new DataStorage(Database.GetDatabase("master"));
    }

    [Theory]
    [InlineData(ItemIdsRootId, "sitecore", TemplateIdSitecore, RootParentId, "/sitecore")]
    [InlineData(ItemIdsContentRoot, "content", TemplateIdMainSection, ItemIdsRootId, "/sitecore/content")]
    [InlineData(ItemIdsTemplateRoot, "templates", TemplateIdMainSection, ItemIdsRootId, "/sitecore/templates")]
    [InlineData(ItemIdsBranchesRoot, "Branches", TemplateIdBranchFolder, ItemIdsTemplateRoot, "/sitecore/templates/Branches")]
    [InlineData(ItemIdsSystemRoot, "system", TemplateIdMainSection, ItemIdsRootId, "/sitecore/system")]
    [InlineData(ItemIdsMediaLibraryRoot, "media library", TemplateIdMainSection, ItemIdsRootId, "/sitecore/media library")]
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
      this.dataStorage.FakeItems[new TemplateID(new ID(TemplateIdSitecore))].Should().BeEquivalentTo(new DbTemplate("Main Section", new TemplateID(new ID(TemplateIdSitecore))));
      this.dataStorage.FakeItems[new TemplateID(new ID(TemplateIdMainSection))].Should().BeEquivalentTo(new DbTemplate("Main Section", new TemplateID(new ID(TemplateIdMainSection))));

      this.dataStorage.FakeItems[TemplateIDs.Template].Should().BeEquivalentTo(new DbTemplate("Template", TemplateIDs.Template));
      this.dataStorage.FakeItems[TemplateIDs.Folder].Should().BeEquivalentTo(new DbTemplate("Folder", TemplateIDs.Folder));
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
    public void ShouldGetSitecoreItemFieldIdsFromTemplateAndValuesFromItems()
    {
      // arrange
      var itemId = ID.NewID;
      var templateId = ID.NewID;
      var fieldId = ID.NewID;

      this.dataStorage.FakeItems.Add(templateId, new DbTemplate("Sample", templateId) { Fields = { new DbField("Title", fieldId) } });
      this.dataStorage.FakeItems.Add(itemId, new DbItem("Sample", itemId, templateId) { Fields = { new DbField("Title", fieldId) { Value = "Welcome!" } } });

      // act
      var item = this.dataStorage.GetSitecoreItem(itemId, Language.Current);

      // assert
      item[fieldId].Should().Be("Welcome!");
    }

    [Fact]
    public void ShouldGetSitecoreItemWithEmptyFieldIfNoItemFieldFound()
    {
      // arrange
      var itemId = ID.NewID;
      var templateId = ID.NewID;
      var fieldId = ID.NewID;

      this.dataStorage.FakeItems.Add(templateId, new DbTemplate("Sample", templateId) { Fields = { new DbField("Title", fieldId) } });
      this.dataStorage.FakeItems.Add(itemId, new DbItem("Sample", itemId, templateId));

      // act
      var item = this.dataStorage.GetSitecoreItem(itemId, Language.Current);

      // assert
      item.InnerData.Fields[fieldId].Should().BeNull();

      // We have changed the way we create ItemData to give more control to Sitecore
      // and in order for the default string.Empty to come back from Field.Value
      // Sitecore needs to be able to make a trip up the templates path 
      // and it in turn requires the Db context

      // item[fieldId].Should().BeEmpty();
    }

    [Fact]
    public void ShouldSetSecurityFieldForRootItem()
    {
      // assert
      this.dataStorage.FakeItems[ItemIDs.RootID].Fields[FieldIDs.Security].Value.Should().Be("ar|Everyone|p*|+*|");
    }
  }
}