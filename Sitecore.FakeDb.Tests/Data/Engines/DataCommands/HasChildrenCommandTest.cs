namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.FakeDb.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class HasChildrenCommandTest
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new OpenHasChildrenCommand();

      // act & assert
      command.CreateInstance().Should().BeOfType<HasChildrenCommand>();
    }

    [Fact]
    public void ShouldReturnTrueIfHasChildren()
    { // arrange
      var database = new FakeDatabase("master");

      var itemId = ID.NewID;
      var sitecoreItem = ItemHelper.CreateInstance(itemId, database);
      var fakeItemWithChildren = new DbItem("parent", itemId) { new DbItem("child") };

      var dataStorage = Substitute.For<DataStorage>();
      dataStorage.GetSitecoreItem(itemId).Returns(sitecoreItem);
      dataStorage.GetFakeItem(itemId).Returns(fakeItemWithChildren);
      database.DataStorage = dataStorage;

      var command = new OpenHasChildrenCommand { Engine = new DataEngine(database) };
      command.Initialize(sitecoreItem);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalseIfNoChildren()
    { // arrange
      var database = new FakeDatabase("master");

      var itemId = ID.NewID;
      var sitecoreItem = ItemHelper.CreateInstance(itemId, database);
      var fakeItemWithoutChildren = new DbItem("item", itemId);

      var dataStorage = Substitute.For<DataStorage>();
      dataStorage.GetSitecoreItem(itemId).Returns(sitecoreItem);
      dataStorage.GetFakeItem(itemId).Returns(fakeItemWithoutChildren);
      database.DataStorage = dataStorage;

      var command = new OpenHasChildrenCommand { Engine = new DataEngine(database) };
      command.Initialize(sitecoreItem);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().BeFalse();
    }

    private class OpenHasChildrenCommand : HasChildrenCommand
    {
      public new Sitecore.Data.Engines.DataCommands.HasChildrenCommand CreateInstance()
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