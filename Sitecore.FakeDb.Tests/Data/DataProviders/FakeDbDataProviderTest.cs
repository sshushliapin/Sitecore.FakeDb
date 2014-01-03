namespace Sitecore.FakeDb.Tests.Data.DataProviders
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.DataProviders;
  using Xunit;

  public class FakeDbDataProviderTest
  {
    private readonly FakeDbDataProvider provider;

    public FakeDbDataProviderTest()
    {
      provider = new FakeDbDataProvider();
    }

    [Fact]
    public void ShouldCreateItem()
    {
      // arrange
      var itemId = ID.NewID;
      const string ItemName = "item";
      var templateId = ID.NewID;
      var itemDefinition = new ItemDefinition(itemId, ItemName, templateId, ID.Null);

      // act
      var isCreated = provider.CreateItem(itemId, ItemName, templateId, null, null);

      // assert
      isCreated.Should().BeTrue();
      provider.DataStorage.ItemDefinitions[itemId].ShouldBeEquivalentTo(itemDefinition);
    }

    [Fact]
    public void ShouldGetItemDefinitionById()
    {
      // arrange
      var itemId = ID.NewID;
      var itemDefinition = new ItemDefinition(itemId, "home", ID.NewID, ID.Null);

      this.provider.DataStorage.ItemDefinitions.Add(itemId, itemDefinition);

      // act
      var result = this.provider.GetItemDefinition(itemId, null);

      // assert
      result.Should().Be(itemDefinition);
    }

    [Fact]
    public void ShouldGetNullIfNoItemFound()
    {
      // arrange
      var nonexistentItemId = ID.NewID;

      // act
      this.provider.GetItemDefinition(nonexistentItemId, null).Should().BeNull();
    }
  }
}