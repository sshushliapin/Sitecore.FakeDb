namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Xunit;

  public class CreateItemCommandTest
  {
    private readonly Database database;

    private readonly OpenCreateItemCommand command;

    private readonly ID itemId;

    private readonly ID templateId;

    public CreateItemCommandTest()
    {
      this.database = Database.GetDatabase("master");
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

    private class OpenCreateItemCommand : CreateItemCommand
    {
      public new Item DoExecute()
      {
        return base.DoExecute();
      }
    }
  }
}