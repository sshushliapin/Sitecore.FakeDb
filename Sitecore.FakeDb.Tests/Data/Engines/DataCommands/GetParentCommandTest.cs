namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class GetParentCommandTest
  {
    private readonly OpenGetParentCommand command;

    public GetParentCommandTest()
    {
      this.command = new OpenGetParentCommand { Engine = new DataEngine(Database.GetDatabase("master")) };
    }

    [Fact]
    public void ShouldCreateInstance()
    {
      // act & assert
      command.CreateInstance().Should().BeOfType<GetParentCommand>();
    }

    [Fact]
    public void ShouldReturnRootItem()
    {
      // arrange
      var dataStorage = CommandHelper.GetDataStorage(this.command);

      var childId = ID.NewID;
      var parentId = ID.NewID;

      dataStorage.FakeItems.Add(childId, new FItem("child", childId) { ParentID = parentId });
      dataStorage.Items.Add(childId, ItemHelper.CreateInstance("child", childId));

      dataStorage.FakeItems.Add(parentId, new FItem("parent", parentId));
      dataStorage.Items.Add(parentId, ItemHelper.CreateInstance("parent", parentId));

      command.Initialize(dataStorage.Items[childId]);

      // act
      var parent = command.Execute();

      // assert
      parent.ID.Should().Be(parentId);
    }

    [Fact]
    public void ShouldReturnNullIfNoParentFound()
    {
      // arrange
      var itemWithoutParent = ItemHelper.CreateInstance("item without parent");
      command.Initialize(itemWithoutParent);

      // act
      var parent = command.Execute();

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