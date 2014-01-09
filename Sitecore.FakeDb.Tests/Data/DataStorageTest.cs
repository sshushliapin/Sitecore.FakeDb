namespace Sitecore.FakeDb.Tests.Data
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;
  using Xunit.Extensions;

  public class DataStorageTest
  {
    private readonly Database database;

    private readonly DataStorage dataStorage;

    private const string ItemIdsRootId = "{11111111-1111-1111-1111-111111111111}";

    private const string ItemIdsContentRoot = "{0DE95AE4-41AB-4D01-9EB0-67441B7C2450}";

    private const string ItemIdsTemplateRoot = "{3C1715FE-6A13-4FCF-845F-DE308BA9741D}";

    private const string TemplateIdsTemplate = "{AB86861A-6030-46C5-B394-E8F99E8B87DB}";

    private const string ItemIdsTemplateSection = "{E269FBB5-3750-427A-9149-7AA950B49301}";

    private const string ItemIdsTemplateField = "{455A3E98-A627-4B40-8035-E683A0331AC7}";

    private const string RootParentId = "{00000000-0000-0000-0000-000000000000}";

    public DataStorageTest()
    {
      this.database = Database.GetDatabase("master");
      this.dataStorage = new DataStorage(this.database);
    }

    [Theory]
    [InlineData(ItemIdsRootId, "sitecore", "{C6576836-910C-4A3D-BA03-C277DBD3B827}", RootParentId, "/sitecore")]
    [InlineData(ItemIdsContentRoot, "content", "{E3E2D58C-DF95-4230-ADC9-279924CECE84}", ItemIdsRootId, "/sitecore/content")]
    [InlineData(ItemIdsTemplateRoot, "templates", "{E3E2D58C-DF95-4230-ADC9-279924CECE84}", ItemIdsRootId, "/sitecore/templates")]
    [InlineData(TemplateIdsTemplate, "Template", TemplateIdsTemplate, ItemIdsTemplateRoot, "/sitecore/templates/template")]
    [InlineData(ItemIdsTemplateSection, "Template section", TemplateIdsTemplate, ItemIdsTemplateRoot, "/sitecore/templates/template section")]
    [InlineData(ItemIdsTemplateField, "Template field", TemplateIdsTemplate, ItemIdsTemplateRoot, "/sitecore/templates/template field")]
    public void ShouldInitializeDefaultFakeItems(string itemId, string itemName, string templateId, string parentId, string fullPath)
    {
      // assert
      this.dataStorage.FakeItems[ID.Parse(itemId)].ID.ToString().Should().Be(itemId);
      this.dataStorage.FakeItems[ID.Parse(itemId)].Name.Should().Be(itemName);
      this.dataStorage.FakeItems[ID.Parse(itemId)].TemplateID.ToString().Should().Be(templateId);
      this.dataStorage.FakeItems[ID.Parse(itemId)].ParentID.ToString().Should().Be(parentId);
      this.dataStorage.FakeItems[ID.Parse(itemId)].FullPath.Should().Be(fullPath);
    }

    [Theory]
    [InlineData(ItemIdsRootId, "sitecore", "{C6576836-910C-4A3D-BA03-C277DBD3B827}")]
    [InlineData(ItemIdsContentRoot, "content", "{E3E2D58C-DF95-4230-ADC9-279924CECE84}")]
    [InlineData(ItemIdsTemplateRoot, "templates", "{E3E2D58C-DF95-4230-ADC9-279924CECE84}")]
    [InlineData(TemplateIdsTemplate, "Template", TemplateIdsTemplate)]
    [InlineData(ItemIdsTemplateSection, "Template section", TemplateIdsTemplate)]
    [InlineData(ItemIdsTemplateField, "Template field", TemplateIdsTemplate)]
    public void ShouldInitializeDefaultSitecoreItems(string itemId, string itemName, string templateId)
    {
      // assert
      dataStorage.Items[ID.Parse(itemId)].Name.Should().Be(itemName);
      dataStorage.Items[ID.Parse(itemId)].TemplateID.ToString().Should().Be(templateId);
    }

    [Fact]
    public void ShouldResetItemsToDefault()
    {
      // arrange

      var itemId = ID.NewID;

      this.dataStorage.FakeItems.Add(itemId, new FItem("new item"));
      this.dataStorage.Items.Add(itemId, ItemHelper.CreateInstance("new item", itemId, ID.NewID, new FieldList(), database));

      // act
      this.dataStorage.Reset();

      // assert
      this.dataStorage.FakeItems.ContainsKey(itemId).Should().BeFalse();
      this.dataStorage.FakeItems.ContainsKey(ItemIDs.RootID).Should().BeTrue();
      this.dataStorage.FakeItems.ContainsKey(ItemIDs.ContentRoot).Should().BeTrue();
      this.dataStorage.FakeItems.ContainsKey(ItemIDs.TemplateRoot).Should().BeTrue();

      this.dataStorage.Items.ContainsKey(itemId).Should().BeFalse();
      this.dataStorage.Items.ContainsKey(ItemIDs.RootID).Should().BeTrue();
      this.dataStorage.Items.ContainsKey(ItemIDs.ContentRoot).Should().BeTrue();
      this.dataStorage.Items.ContainsKey(ItemIDs.TemplateRoot).Should().BeTrue();
    }

    [Fact]
    public void ShouldGetExistingItem()
    {
      // act & assert
      dataStorage.GetFakeItem(ItemIDs.ContentRoot).Should().NotBeNull();
      dataStorage.GetFakeItem(ItemIDs.ContentRoot).Should().BeOfType<FItem>();

      dataStorage.GetSitecoreItem(ItemIDs.ContentRoot).Should().NotBeNull();
      dataStorage.GetSitecoreItem(ItemIDs.ContentRoot).Should().BeOfType<Item>();
    }

    [Fact]
    public void ShouldGetNullIdNoItemPresent()
    {
      // act & assert
      dataStorage.GetFakeItem(ID.NewID).Should().BeNull();
      dataStorage.GetSitecoreItem(ID.NewID).Should().BeNull();
    }
  }
}