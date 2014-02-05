namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.FakeDb.Data;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class DeleteItemCommandTest
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new OpenDeleteItemCommand();

      // act & assert
      command.CreateInstance().Should().BeOfType<DeleteItemCommand>();
    }

    [Fact]
    public void ShouldDeleteItemFromDataStorageAndReturnTrue()
    {
      // arrange
      var database = Substitute.For<FakeDatabase>("master");

      var itemId = ID.NewID;
      database.DataStorage.FakeItems.Add(itemId, new DbItem("item"));

      var command = new OpenDeleteItemCommand { Engine = new DataEngine(database) };
      command.Initialize(ItemHelper.CreateInstance(itemId), ID.NewID);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().BeTrue();
      database.DataStorage.FakeItems.Should().NotContainKey(itemId);
    }

    [Fact]
    public void ShouldDeleteItemDescendants()
    {
      // arrange
      var database = Substitute.For<FakeDatabase>("master");

      var itemId = ID.NewID;
      var desc1Id = ID.NewID;
      var desc2Id = ID.NewID;

      var item = new DbItem("item", itemId);
      var desc1 = new DbItem("descendant1", desc1Id);
      var desc2 = new DbItem("descendant2", desc2Id);

      item.Children.Add(desc1);
      desc1.Children.Add(desc2);

      database.DataStorage.FakeItems.Add(itemId, item);
      database.DataStorage.FakeItems.Add(desc1Id, desc1);
      database.DataStorage.FakeItems.Add(desc2Id, desc2);

      var command = new OpenDeleteItemCommand { Engine = new DataEngine(database) };
      command.Initialize(ItemHelper.CreateInstance(itemId), ID.NewID);

      // act
      command.DoExecute();

      // assert
      database.DataStorage.FakeItems.Should().NotContainKey(desc1Id);
      database.DataStorage.FakeItems.Should().NotContainKey(desc2Id);
    }

    [Fact]
    public void ShouldReturnFalseIfNoItemFound()
    {
      // arrange
      var database = Substitute.For<FakeDatabase>("master");

      var command = new OpenDeleteItemCommand { Engine = new DataEngine(database) };
      command.Initialize(ItemHelper.CreateInstance(), ID.NewID);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().BeFalse();
    }

    private class OpenDeleteItemCommand : DeleteItemCommand
    {
      public new Sitecore.Data.Engines.DataCommands.DeleteItemCommand CreateInstance()
      {
        return base.CreateInstance();
      }

      public new bool DoExecute()
      {
        return base.DoExecute();
      }
    }
  }
}