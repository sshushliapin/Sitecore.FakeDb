namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class FakeSaveItemCommandTest
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new OpenFakeSaveItemCommand();

      // act
      var instance = command.CreateInstance();

      // assert
      instance.Should().BeOfType<FakeSaveItemCommand>();
    }

    [Fact]
    public void ShouldUpdateExistingItemInDataStorage()
    {
      // arrange
      var itemId = ID.NewID;
      var originalItem = ItemHelper.CreateInstance("original item", itemId);

      var database = new FakeDatabase("master");
      database.DataStorage.Items.Add(itemId, originalItem);

      var updatedItem = ItemHelper.CreateInstance("updated item", itemId);
      updatedItem["Title"] = "updated title";

      var command = new OpenFakeSaveItemCommand();
      command.Initialize(updatedItem);
      command.Engine = new DataEngine(database);

      // act
      command.DoExecute();

      // assert
      database.DataStorage.Items[itemId].Name.Should().Be("updated item");
      database.DataStorage.Items[itemId]["Title"].Should().Be("updated title");
    }

    private class OpenFakeSaveItemCommand : FakeSaveItemCommand
    {
      public new SaveItemCommand CreateInstance()
      {
        return base.CreateInstance();
      }

      public new void DoExecute()
      {
        base.DoExecute();
      }
    }
  }
}