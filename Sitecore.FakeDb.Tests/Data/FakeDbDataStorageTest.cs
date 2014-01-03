namespace Sitecore.FakeDb.Tests.Data
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data;
  using Xunit;
  using Xunit.Extensions;

  public class FakeDbDataStorageTest
  {
    private const string RootId = "{11111111-1111-1111-1111-111111111111}";
    private const string ContentRoot = "{0DE95AE4-41AB-4D01-9EB0-67441B7C2450}";
    private const string TemplateRoot = "{3C1715FE-6A13-4FCF-845F-DE308BA9741D}";

    [Theory]
    [InlineData(RootId, "sitecore", "{C6576836-910C-4A3D-BA03-C277DBD3B827}")]
    [InlineData(ContentRoot, "content", "{E3E2D58C-DF95-4230-ADC9-279924CECE84}")]
    [InlineData(TemplateRoot, "templates", "{E3E2D58C-DF95-4230-ADC9-279924CECE84}")]
    public void ShouldInitializeDefaultSitecoreItems(string itemId, string itemName, string templateId)
    {
      // arrange
      var dataStorage = new FakeDbDataStorage();

      // assert
      dataStorage.ItemDefinitions[ID.Parse(itemId)].ShouldBeEquivalentTo(new ItemDefinition(ID.Parse(itemId), itemName, ID.Parse(templateId), ID.Null));
    }

    [Fact]
    public void ShouldResetItemDefinitionsToDefault()
    {
      // arrange
      var dataStorage = new FakeDbDataStorage();

      var itemId = ID.NewID;
      var newItemDefinition = new ItemDefinition(itemId, "new item", ID.NewID, ID.Null);

      dataStorage.ItemDefinitions.Add(itemId, newItemDefinition);

      // act
      dataStorage.Reset();

      // assert
      dataStorage.ItemDefinitions.ContainsKey(itemId).Should().BeFalse();
      dataStorage.ItemDefinitions.ContainsKey(ItemIDs.RootID).Should().BeTrue();
      dataStorage.ItemDefinitions.ContainsKey(ItemIDs.ContentRoot).Should().BeTrue();
      dataStorage.ItemDefinitions.ContainsKey(ItemIDs.TemplateRoot).Should().BeTrue();
    }
  }
}