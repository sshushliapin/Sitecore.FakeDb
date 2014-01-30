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
    {
      // arrange
      var database = Substitute.For<FakeDatabase>("master");
      database.DataStorage = Substitute.For<DataStorage>();

      var itemId = ID.NewID;
      var sitecoreItem = ItemHelper.CreateInstance(itemId);
      var fakeItemWithChildren = new DbItem("parent", itemId) { new DbItem("child") };

      database.DataStorage.GetSitecoreItem(itemId).Returns(sitecoreItem);
      database.DataStorage.GetFakeItem(itemId).Returns(fakeItemWithChildren);

      var command = new OpenHasChildrenCommand { Engine = new DataEngine(database) };
      command.Initialize(sitecoreItem);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalseIfNoChildren()
    {
      // arrange
      var database = Substitute.For<FakeDatabase>("master");
      database.DataStorage = Substitute.For<DataStorage>();

      var itemId = ID.NewID;
      var sitecoreItem = ItemHelper.CreateInstance(itemId);
      var fakeItemWithoutChildren = new DbItem("item", itemId);

      database.DataStorage.GetSitecoreItem(itemId).Returns(sitecoreItem);
      database.DataStorage.GetFakeItem(itemId).Returns(fakeItemWithoutChildren);

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