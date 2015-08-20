namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes;
  using Sitecore.Reflection;
  using Xunit;
  using MoveItemCommand = Sitecore.FakeDb.Data.Engines.DataCommands.MoveItemCommand;

  public class MoveItemCommandTest
  {
    [Theory, DefaultAutoData]
    public void ShouldMoveItemToNewDestination(MoveItemCommand sut, GetParentCommandPrototype getParentCommand, Item item, Item destination, ID parentId)
    {
      // arrange
      getParentCommand.Initialize(sut.DataStorage);
      sut.Database.Engines.DataEngine.Commands.GetParentPrototype = getParentCommand;

      var fakeItem = new DbItem("item", item.ID) { ParentID = parentId };
      var fakeParent = new DbItem("parent", parentId) { Children = { fakeItem } };
      var fakeDestination = new DbItem("destination", destination.ID) { FullPath = "/new destination path" };

      sut.DataStorage.GetFakeItem(item.ID).Returns(fakeItem);
      sut.DataStorage.GetFakeItem(parentId).Returns(fakeParent);
      sut.DataStorage.GetFakeItem(destination.ID).Returns(fakeDestination);

      sut.Initialize(item, destination);

      // act
      var result = (bool)ReflectionUtil.CallMethod(sut, "DoExecute");

      // assert
      result.Should().BeTrue();
      fakeItem.ParentID.Should().Be(destination.ID);
      fakeItem.FullPath.Should().Be("/new destination path/item");
      fakeParent.Children.Should().NotContain(fakeItem);
      fakeDestination.Children.Should().Contain(fakeItem);
    }
  }
}