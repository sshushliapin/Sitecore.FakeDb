namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;
  using MoveItemCommand = Sitecore.FakeDb.Data.Engines.DataCommands.MoveItemCommand;

  public class MoveItemCommandTest : CommandTestBase
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var createdCommand = Substitute.For<MoveItemCommand>();
      this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.MoveItemCommand, MoveItemCommand>().Returns(createdCommand);

      var command = new OpenMoveItemCommand();
      command.Initialize(this.innerCommand);

      // act & assert
      command.CreateInstance().Should().Be(createdCommand);
    }

    [Fact]
    public void ShouldMoveItemToNewDestination()
    {
      // arrange
      var itemId = ID.NewID;
      var parentId = ID.NewID;
      var destinationId = ID.Null;

      var item = ItemHelper.CreateInstance("item", itemId, this.database);
      var destination = ItemHelper.CreateInstance(destinationId, this.database);

      var getParentCommand = new FakeGetParentCommand();
      this.database.Engines.DataEngine.Commands.GetParentPrototype = getParentCommand;

      var fakeItem = new DbItem("item", itemId) { ParentID = parentId };
      var fakeParent = new DbItem("parent") { Children = { fakeItem } };
      var fakeDestination = new DbItem("destination", destinationId) { FullPath = "/new destination path" };

      this.dataStorage.GetFakeItem(itemId).Returns(fakeItem);
      this.dataStorage.GetFakeItem(parentId).Returns(fakeParent);
      this.dataStorage.GetFakeItem(destinationId).Returns(fakeDestination);

      var command = new OpenMoveItemCommand();
      command.Initialize(item, destination);
      command.Initialize(this.innerCommand);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().BeTrue();
      fakeItem.ParentID.Should().Be(destinationId);
      fakeItem.FullPath.Should().Be("/new destination path/item");
      fakeParent.Children.Should().NotContain(fakeItem);
      fakeDestination.Children.Should().Contain(fakeItem);
    }

    private class OpenMoveItemCommand : MoveItemCommand
    {
      public new Sitecore.Data.Engines.DataCommands.MoveItemCommand CreateInstance()
      {
        return base.CreateInstance();
      }

      public new bool DoExecute()
      {
        return base.DoExecute();
      }
    }

    private class FakeGetParentCommand : GetParentCommand
    {
    }
  }
}