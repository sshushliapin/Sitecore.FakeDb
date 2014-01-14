namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Xunit;

  public class CreateItemCommandTest
  {
    private readonly FakeDatabase database;

    private readonly OpenCreateItemCommand command;

    private readonly ID itemId;

    private readonly ID templateId;

    public CreateItemCommandTest()
    {
      // TODO: Use 'new'
      this.database = (FakeDatabase)Database.GetDatabase("master");

      var datastorage = Substitute.For<DataStorage>(this.database);
      datastorage.GetFieldList(Arg.Any<ID>()).ReturnsForAnyArgs(new FieldList());
      this.database.DataStorage = datastorage;

      this.command = new OpenCreateItemCommand { Engine = new DataEngine(this.database) };

      this.itemId = ID.NewID;
      this.templateId = ID.NewID;
    }

    [Fact]
    public void ShouldCreateItemInstance()
    {
      // arrange
      var destination = this.database.GetItem("/sitecore");

      this.command.Initialize(this.itemId, "home", this.templateId, destination);

      // act
      var item = this.command.DoExecute();

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
      var dataStorage = this.command.Database.GetDataStorage();

      this.command.Initialize(this.itemId, "home", this.templateId, destination);

      // act
      this.command.DoExecute();

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

      this.command.Initialize(this.itemId, "home", this.templateId, destination);

      // act
      var item = this.command.DoExecute();

      // assert
      item.Fields[fieldId1].Should().NotBeNull();
      item.Fields[fieldId1].Value.Should().Be("f1");

      item.Fields[fieldId2].Should().NotBeNull();
      item.Fields[fieldId2].Value.Should().Be("f2");
    }

    private class OpenCreateItemCommand : CreateItemCommand
    {
      public new Item DoExecute()
      {
        return base.DoExecute();
      }
    }
  }
}