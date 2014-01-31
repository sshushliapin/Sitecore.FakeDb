namespace Sitecore.FakeDb.Tests.Data.Engines
{
  using System.Linq;
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Xunit;

  // TODO: Get rid of the copy-paste.
  public class ItemCreatorTest
  {
    private readonly FakeDatabase database;

    private readonly ID itemId = ID.NewID;

    private readonly ID templateId = ID.NewID;

    private readonly ItemCreator itemCreator;

    private readonly Item destination;

    public ItemCreatorTest()
    {
      this.database = new FakeDatabase("master");
      this.destination = this.database.GetItem("/sitecore");

      this.itemCreator = new ItemCreator();
    }

    [Fact]
    public void ShouldCreateItemInstance()
    {
      // arrange
      this.database.DataStorage.FakeTemplates.Add(this.templateId, new DbTemplate());

      // act
      var item = this.itemCreator.Create("home", this.itemId, this.templateId, this.database, this.destination);

      // assert
      item.Should().NotBeNull();
      item.Name.Should().Be("home");
      item.ID.Should().Be(this.itemId);
      item.TemplateID.Should().Be(this.templateId);
    }

    [Fact]
    public void ShouldPutItemInstanceIntoDataStorage()
    {
      // arrange
      this.database.DataStorage.FakeTemplates.Add(this.templateId, new DbTemplate());

      // act
      this.itemCreator.Create("home", this.itemId, this.templateId, this.database, this.destination);

      // assert
      this.database.DataStorage.FakeItems.Should().ContainKey(this.itemId);
    }

    [Fact]
    public void ShouldSetItemChildren()
    {
      // arrange
      this.database.DataStorage.FakeTemplates.Add(this.templateId, new DbTemplate());

      // act
      this.itemCreator.Create("home", this.itemId, this.templateId, this.database, this.destination);

      // assert
      this.database.DataStorage.FakeItems[this.destination.ID].Children.Single().ID.Should().Be(this.itemId);
    }
  }
}