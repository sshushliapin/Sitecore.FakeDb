namespace Sitecore.FakeDb.Tests.Data.Engines
{
  using System;
  using System.Linq;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class ItemCreatorTest : IDisposable
  {
    private readonly Database database;

    private readonly DataStorage dataStorage;

    private readonly ID itemId = ID.NewID;

    private readonly ID templateId = ID.NewID;

    private readonly ItemCreator itemCreator;

    private readonly Item destination;

    public ItemCreatorTest()
    {
      this.database = Database.GetDatabase("master");
      this.dataStorage = Substitute.For<DataStorage>();

      this.destination = ItemHelper.CreateInstance(this.database);

      this.dataStorage.GetFakeItem(this.destination.ID).Returns(new DbItem("destination"));
      this.dataStorage.GetFieldList(this.templateId).Returns(new FieldList());

      this.itemCreator = new ItemCreator(this.dataStorage);
    }

    [Fact]
    public void ShouldCreateItemInstance()
    {
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
      // act
      this.itemCreator.Create("home", this.itemId, this.templateId, this.database, this.destination);

      // assert
      this.dataStorage.FakeItems.Should().ContainKey(this.itemId);
    }

    [Fact]
    public void ShouldSetItemChildren()
    {
      // arrange
      var item = new DbItem("destination");
      this.dataStorage.GetFakeItem(this.destination.ID).Returns(item);

      // act
      this.itemCreator.Create("home", this.itemId, this.templateId, this.database, this.destination);

      // assert
      item.Children.Single().ID.Should().Be(this.itemId);
    }

    [Fact]
    public void ShouldReturnItemIfAlreadyExists()
    {
      // arrange
      var item = new DbItem("home");
      this.dataStorage.GetFakeItem(this.itemId).Returns(item);

      // act
      var item1 = this.itemCreator.Create("home", this.itemId, this.templateId, this.database, this.destination);
      var item2 = this.itemCreator.Create("home", this.itemId, this.templateId, this.database, this.destination);

      // assert
      item1.Should().Be(item2);
    }

    public void Dispose()
    {
      Factory.Reset();
    }
  }
}