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
      var childId = ID.NewID;
      var parentId = ID.NewID;

      var database = new FakeDatabase("master");
      var dataStorage = database.GetDataStorage();

      dataStorage.FakeItems.Add(childId, new FItem("child", childId) { ParentID = parentId });
      dataStorage.Items.Add(childId, ItemHelper.CreateInstance("child", childId, ID.NewID, new FieldList(), database));

      dataStorage.FakeItems.Add(parentId, new FItem("parent", parentId));
      dataStorage.Items.Add(parentId, ItemHelper.CreateInstance("parent", parentId, ID.NewID, new FieldList(), database));

      var command = new OpenGetParentCommand { Engine = new DataEngine(database) };
      command.Initialize(dataStorage.Items[childId]);

      // act
      var parent = command.DoExecute();

      // assert
      parent.ID.Should().Be(parentId);
    }

    [Fact]
    public void ShouldReturnNullIfNoParentFound()
    {
      // arrange
      var database = new FakeDatabase("master");

      var itemId = ID.NewID;
      var itemWithoutParent = ItemHelper.CreateInstance("item without parent", itemId, database);

      database.DataStorage = Substitute.For<DataStorage>();
      database.DataStorage.GetFakeItem(itemId).Returns(new FItem("item"));

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