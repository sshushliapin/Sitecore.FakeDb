namespace Sitecore.FakeDb.Tests.Data
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;
  using Xunit.Extensions;

  public class FakeDbDataStorageTest
  {
    private readonly Database database;

    private readonly FakeDbDataStorage dataStorage;

    public FakeDbDataStorageTest()
    {
      this.database = Database.GetDatabase("master");
      dataStorage = new FakeDbDataStorage(this.database);
    }

    private const string RootId = "{11111111-1111-1111-1111-111111111111}";
    private const string ContentRoot = "{0DE95AE4-41AB-4D01-9EB0-67441B7C2450}";
    private const string TemplateRoot = "{3C1715FE-6A13-4FCF-845F-DE308BA9741D}";

    [Theory]
    [InlineData(RootId, "sitecore", "{C6576836-910C-4A3D-BA03-C277DBD3B827}")]
    [InlineData(ContentRoot, "content", "{E3E2D58C-DF95-4230-ADC9-279924CECE84}")]
    [InlineData(TemplateRoot, "templates", "{E3E2D58C-DF95-4230-ADC9-279924CECE84}")]
    public void ShouldInitializeDefaultSitecoreItems(string itemId, string itemName, string templateId)
    {
      // assert
      dataStorage.Items[ID.Parse(itemId)].Name.Should().Be(itemName);
      dataStorage.Items[ID.Parse(itemId)].TemplateID.ToString().Should().Be(templateId);
    }

    [Fact]
    public void ShouldResetItemDefinitionsToDefault()
    {
      // arrange

      var itemId = ID.NewID;
      var item = ItemHelper.CreateInstance("new item", itemId, ID.NewID, new FieldList(), database);

      this.dataStorage.Items.Add(itemId, item);

      // act
      this.dataStorage.Reset();

      // assert
      this.dataStorage.Items.ContainsKey(itemId).Should().BeFalse();
      this.dataStorage.Items.ContainsKey(ItemIDs.RootID).Should().BeTrue();
      this.dataStorage.Items.ContainsKey(ItemIDs.ContentRoot).Should().BeTrue();
      this.dataStorage.Items.ContainsKey(ItemIDs.TemplateRoot).Should().BeTrue();
    }
  }
}