namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;
  using SaveItemCommand = Sitecore.FakeDb.Data.Engines.DataCommands.SaveItemCommand;

  public class SaveItemCommandTest
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new OpenSaveItemCommand();

      // act
      var instance = command.CreateInstance();

      // assert
      instance.Should().BeOfType<SaveItemCommand>();
    }

    [Fact]
    public void ShouldUpdateExistingItemInDataStorage()
    {
      // arrange
      var itemId = ID.NewID;

      var originalItem = ItemHelper.CreateInstance("original item", itemId);

      var database = new FakeDatabase("master");
      database.DataStorage.Items.Add(itemId, originalItem);

      var fieldId = ID.NewID;
      var fields = new FieldList { { fieldId, "updated title" } };
      var updatedItem = ItemHelper.CreateInstance("updated item", itemId, fields);

      var command = new OpenSaveItemCommand();
      command.Initialize(updatedItem);
      command.Engine = new DataEngine(database);

      // act
      command.DoExecute();

      // assert
      database.DataStorage.Items[itemId].Name.Should().Be("updated item");
      database.DataStorage.Items[itemId][fieldId].Should().Be("updated title");
    }

    private class OpenSaveItemCommand : SaveItemCommand
    {
      public new Sitecore.Data.Engines.DataCommands.SaveItemCommand CreateInstance()
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