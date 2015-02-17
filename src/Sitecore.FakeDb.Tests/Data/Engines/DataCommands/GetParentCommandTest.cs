namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class GetParentCommandTest : CommandTestBase
  {
    private readonly OpenGetParentCommand command;

    public GetParentCommandTest()
    {
      this.command = new OpenGetParentCommand { Engine = new DataEngine(this.database) };
      this.command.Initialize(this.dataStorage);
    }

    [Fact]
    public void ShouldCreateInstance()
    {
      // act & assert
      this.command.CreateInstance().Should().BeOfType<GetParentCommand>();
    }

    [Fact]
    public void ShouldReturnRootItem()
    {
      // arrange
      var parentId = ID.NewID;
      var childId = ID.NewID;

      var parentItem = ItemHelper.CreateInstance(this.database);
      var childItem = ItemHelper.CreateInstance(this.database, childId);

      this.dataStorage.GetFakeItem(childId).Returns(new DbItem("child", childId) { ParentID = parentId });
      this.dataStorage.GetSitecoreItem(parentId, parentItem.Language).Returns(parentItem);

      this.command.Initialize(childItem);

      // act
      var result = this.command.DoExecute();

      // assert
      result.Should().Be(parentItem);
    }

    [Fact]
    public void ShouldReturnNullIfNoParentFound()
    {
      // arrange
      var item = ItemHelper.CreateInstance(this.database);
      this.command.Initialize(item);

      // act
      var parent = this.command.DoExecute();

      // assert
      parent.Should().BeNull();
      this.dataStorage.DidNotReceiveWithAnyArgs().GetSitecoreItem(null, null);
    }

    [Fact]
    public void ShouldNotTryToLocateParentForSitecoreRoot()
    {
      // arrange
      var rootId = ItemIDs.RootID;
      var item = ItemHelper.CreateInstance(this.database, rootId);

      dataStorage.GetFakeItem(rootId).Returns(new DbItem("sitecore", rootId));

      this.command.Initialize(item);

      // act
      var parent = this.command.DoExecute();

      // assert
      parent.Should().BeNull();
      dataStorage.DidNotReceiveWithAnyArgs().GetSitecoreItem(null, null);
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