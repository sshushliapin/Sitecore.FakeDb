namespace Sitecore.FakeDb.Tests.Data.DataProviders
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.DataProviders;
  using Xunit;

  public class FakeDbDataProviderTest
  {
    [Fact]
    public void ShouldCreateItem()
    {
      // arrange
      var provider = new FakeDbDataProvider();

      var itemId = ID.NewID;
      const string ItemName = "item";
      var templateId = ID.NewID;
      var itemDefinition = new ItemDefinition(itemId, ItemName, templateId, ID.Null);

      // act
      provider.CreateItem(itemId, ItemName, templateId, null, null);

      // assert
      provider.DataStorage.ItemDefinitions[itemId].ShouldBeEquivalentTo(itemDefinition);
    }

    [Fact]
    public void ShouldGetItemDefinitionById()
    {
      // arrange
      var provider = new FakeDbDataProvider();
      var itemId = ID.NewID;
      var itemDefinition = new ItemDefinition(itemId, "home", ID.NewID, ID.Null);

      provider.DataStorage.ItemDefinitions.Add(itemId, itemDefinition);

      // act
      var result = provider.GetItemDefinition(itemId, null);

      // assert
      result.Should().Be(itemDefinition);
    }
  }
}