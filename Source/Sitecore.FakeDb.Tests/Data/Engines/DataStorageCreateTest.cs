namespace Sitecore.FakeDb.Tests.Data.Engines
{
  using System;
  using System.Linq;
  using FluentAssertions;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class DataStorageCreateTest : IDisposable
  {
    private readonly Database database;

    private readonly DataStorage dataStorage;

    private readonly ID itemId = ID.NewID;

    private readonly ID templateId = ID.NewID;

    private readonly Item destination;

    public DataStorageCreateTest()
    {
      this.database = Database.GetDatabase("master");
      this.dataStorage = new DataStorage(this.database);

      this.destination = ItemHelper.CreateInstance(this.database);
      this.dataStorage.FakeItems.Add(this.destination.ID, new DbItem("destination"));
    }

    [Fact]
    public void ShouldCreateItemInstance()
    {
      // act
      this.dataStorage.Create("home", this.itemId, this.templateId, this.destination);

      // assert
      var item = this.dataStorage.GetSitecoreItem(this.itemId);
      item.Should().NotBeNull();
      item.Name.Should().Be("home");
      item.ID.Should().Be(this.itemId);
      item.TemplateID.Should().Be(this.templateId);
    }

    [Fact]
    public void ShouldPutItemInstanceIntoDataStorage()
    {
      // act
      this.dataStorage.Create("home", this.itemId, this.templateId, this.destination);

      // assert
      this.dataStorage.FakeItems.Should().ContainKey(this.itemId);
    }

    [Fact]
    public void ShouldSetItemChildren()
    {
      // arrange
      var item = new DbItem("destination");
      this.dataStorage.FakeItems[this.destination.ID] = item;

      // act
      this.dataStorage.Create("home", this.itemId, this.templateId, this.destination);

      // assert
      item.Children.Single().ID.Should().Be(this.itemId);
    }

    [Fact]
    public void ShouldThrowIfNoParentFound()
    {
      // arrange
      var parentId = new ID("eb09ce25-f03b-48bd-bdf1-0794b94aaf72");
      var missingParent = ItemHelper.CreateInstance(this.database, parentId);

      // act
      Action action = () => this.dataStorage.Create("home", this.itemId, this.templateId, missingParent);

      // assert
      action.ShouldThrow<InvalidOperationException>().WithMessage("Parent item \"{eb09ce25-f03b-48bd-bdf1-0794b94aaf72}\" not found.");
    }

    public void Dispose()
    {
      Factory.Reset();
    }
  }
}