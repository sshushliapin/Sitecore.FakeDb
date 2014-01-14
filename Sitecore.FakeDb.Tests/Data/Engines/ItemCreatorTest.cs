namespace Sitecore.FakeDb.Tests.Data.Engines
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Xunit;

  public class ItemCreatorTest
  {
    private readonly FakeDatabase database;

    private readonly ID itemId;

    private readonly ID templateId;

    private readonly ItemCreator itemCreator;

    public ItemCreatorTest()
    {
      // TODO: Use 'new'
      this.database = (FakeDatabase)Database.GetDatabase("master");

      var datastorage = Substitute.For<DataStorage>(this.database);
      datastorage.GetFieldList(Arg.Any<ID>()).ReturnsForAnyArgs(new FieldList());
      this.database.DataStorage = datastorage;

      this.itemId = ID.NewID;
      this.templateId = ID.NewID;

      this.itemCreator = new ItemCreator();
    }

    [Fact]
    public void ShouldCreateItemInstance()
    {
      // arrange
      var destination = this.database.GetItem("/sitecore");

      // act
      var item = this.itemCreator.Create("home", this.itemId, this.templateId, this.database, destination);

      // assert
      item.Should().NotBeNull();
      item.Name.Should().Be("home");
      item.ID.Should().Be(this.itemId);
      item.TemplateID.Should().Be(this.templateId);
      item.Paths.FullPath.Should().Be("/sitecore/home");
    }

    [Fact]
    public void ShouldPutItemInstanceIntoDataStorage()
    {
      // arrange
      var destination = this.database.GetItem("/sitecore");
      var dataStorage = this.database.GetDataStorage();

      // act
      this.itemCreator.Create("home", this.itemId, this.templateId, this.database, destination);

      // assert
      dataStorage.FakeItems.Should().ContainKey(itemId);
      dataStorage.Items.Should().ContainKey(itemId);
    }

    [Fact]
    public void ShouldInitializeFields()
    {
      // arrange
      var destination = this.database.GetItem("/sitecore");

      var fieldId1 = ID.NewID;
      var fieldId2 = ID.NewID;

      this.database.DataStorage.GetFieldList(this.templateId).Returns(new FieldList { { fieldId1, "f1" }, { fieldId2, "f2" } });

      // act
      var item = this.itemCreator.Create("home", this.itemId, this.templateId, this.database, destination);

      // assert
      item.Fields[fieldId1].Should().NotBeNull();
      item.Fields[fieldId1].Value.Should().Be("f1");

      item.Fields[fieldId2].Should().NotBeNull();
      item.Fields[fieldId2].Value.Should().Be("f2");
    }
  }
}