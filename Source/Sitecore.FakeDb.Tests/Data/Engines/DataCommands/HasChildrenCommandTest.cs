namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class HasChildrenCommandTest : CommandTestBase
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new OpenHasChildrenCommand(this.dataStorage);

      // act & assert
      command.CreateInstance().Should().BeOfType<HasChildrenCommand>();
    }

    [Fact]
    public void ShouldReturnTrueIfHasChildren()
    {
      // arrange
      var itemId = ID.NewID;
      var item = ItemHelper.CreateInstance(itemId, this.database);
      var fakeItemWithChildren = new DbItem("parent", itemId) { new DbItem("child") };

      this.dataStorage.GetSitecoreItem(itemId, item.Language).Returns(item);
      this.dataStorage.GetFakeItem(itemId).Returns(fakeItemWithChildren);

      var command = new OpenHasChildrenCommand(this.dataStorage) { Engine = new DataEngine(this.database) };
      command.Initialize(item);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalseIfNoChildren()
    {
      // arrange
      var itemId = ID.NewID;
      var sitecoreItem = ItemHelper.CreateInstance(itemId, this.database);
      var fakeItemWithoutChildren = new DbItem("item", itemId);

      this.dataStorage.GetSitecoreItem(itemId, sitecoreItem.Language).Returns(sitecoreItem);
      this.dataStorage.GetFakeItem(itemId).Returns(fakeItemWithoutChildren);

      var command = new OpenHasChildrenCommand(this.dataStorage) { Engine = new DataEngine(this.database) };
      command.Initialize(sitecoreItem);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().BeFalse();
    }

    private class OpenHasChildrenCommand : HasChildrenCommand
    {
      public OpenHasChildrenCommand(DataStorage dataStorage)
        : base(dataStorage)
      {
      }

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