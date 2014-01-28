namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Collections;
  using Sitecore.Data.Engines;
  using Sitecore.FakeDb.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class GetChildrenCommandTest
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new OpenGetChildrenCommand();

      // act & assert
      command.CreateInstance().Should().BeOfType<GetChildrenCommand>();
    }

    [Fact]
    public void ShouldReturnItemChildren()
    {
      // arrange
      var dataStorage = Substitute.For<DataStorage>();
      var database = new FakeDatabase("master") { DataStorage = dataStorage };

      var dbchild1 = new DbItem("child1");
      var dbchild2 = new DbItem("child2");
      var dbitem = new DbItem("item") { dbchild1, dbchild2 };

      var child1 = ItemHelper.CreateInstance();
      var child2 = ItemHelper.CreateInstance();
      var item = ItemHelper.CreateInstance(dbitem.ID, database);

      dataStorage.GetFakeItem(dbitem.ID).Returns(dbitem);
      dataStorage.GetSitecoreItem(dbchild1.ID).Returns(child1);
      dataStorage.GetSitecoreItem(dbchild2.ID).Returns(child2);

      var command = new OpenGetChildrenCommand { Engine = new DataEngine(database) };
      command.Initialize(item);

      // act
      var children = command.DoExecute();

      // assert
      children[0].Should().Be(child1);
      children[1].Should().Be(child2);
    }

    private class OpenGetChildrenCommand : GetChildrenCommand
    {
      public new Sitecore.Data.Engines.DataCommands.GetChildrenCommand CreateInstance()
      {
        return base.CreateInstance();
      }

      public new ItemList DoExecute()
      {
        return base.DoExecute();
      }
    }
  }
}