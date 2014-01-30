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
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class GetParentCommandTest
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new OpenGetParentCommand();

      // act & assert
      command.CreateInstance().Should().BeOfType<GetParentCommand>();
    }

    [Fact]
    public void ShouldReturnRootItem()
    {
      // arrange
      var database = Substitute.For<FakeDatabase>("master");
      database.DataStorage = Substitute.For<DataStorage>();

      var parentId = ID.NewID;
      var childId = ID.NewID;

      var parentItem = ItemHelper.CreateInstance();
      var childItem = ItemHelper.CreateInstance(childId);

      database.DataStorage.GetFakeItem(childId).Returns(new DbItem("child", childId) { ParentID = parentId });
      database.DataStorage.GetSitecoreItem(parentId).Returns(parentItem);

      var command = new OpenGetParentCommand { Engine = new DataEngine(database) };
      command.Initialize(childItem);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().Be(parentItem);
    }

    [Fact]
    public void ShouldReturnNullIfNoParentFound()
    {
      // arrange
      var database = Substitute.For<FakeDatabase>("master");
      database.DataStorage = Substitute.For<DataStorage>();

      var itemWithoutParent = ItemHelper.CreateInstance();

      var command = new OpenGetParentCommand { Engine = new DataEngine(database) };
      command.Initialize(itemWithoutParent);

      // act
      var parent = command.DoExecute();

      // assert
      parent.Should().BeNull();
    }

    private class OpenGetParentCommand : GetParentCommand
    {
      public new Sitecore.Data.Engines.DataCommands.GetParentCommand CreateInstance()
      {
        return base.CreateInstance();
      }

      public new Item DoExecute()
      {
        return base.DoExecute();
      }
    }
  }
}