namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using System;
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Xunit;

  public class CreateItemCommandTest
  {
    private readonly Database database;

    private readonly OpenCreateItemCommand command;

    public CreateItemCommandTest()
    {
      this.database = Database.GetDatabase("master");
      this.command = new OpenCreateItemCommand { Engine = new DataEngine(this.database) };
    }

    [Fact]
    public void ShouldCreateItemInstance()
    {
      // arrange
      var destination = this.database.GetRootItem();
      this.command.Initialize(ID.NewID, "home", ID.NewID, destination);

      // act
      var item = this.command.DoExecute();

      // assert
      item.Should().NotBeNull();
    }

    [Fact]
    public void ShouldPutItemInstanceIntoDataStorage()
    {
      // arrange
      // TODO: Is the 'get root' returns '/sitecore/content'?
      var destination = this.database.GetRootItem();
      var itemId = ID.NewID;
      var templateId = ID.NewID;

      this.command.Initialize(itemId, "home", templateId, destination);
      var dataStorage = CommandHelper.GetDataStorage(this.command);

      // act
      this.command.DoExecute();

      // assert
      dataStorage.FakeItems.Should().ContainKey(itemId);
      dataStorage.FakeItems[itemId].Should().BeEquivalentTo(new FItem("home", itemId, templateId) { ParentID = ItemIDs.ContentRoot, FullPath = Constants.ContentPath });

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