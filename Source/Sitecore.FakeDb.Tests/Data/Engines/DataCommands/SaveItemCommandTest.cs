namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.FakeDb.Data;
  using Sitecore.FakeDb.Data.Engines;
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
      var database = Substitute.For<FakeDatabase>("master");
      database.DataStorage = Substitute.For<DataStorage>();

      var itemId = ID.NewID;

      database.DataStorage.FakeItems.Add(itemId, new DbItem("original item"));

      var fieldId = ID.NewID;
      var fields = new FieldList { { fieldId, "updated title" } };
      var updatedItem = ItemHelper.CreateInstance("updated item", itemId, ID.NewID, fields, database);

      var command = new OpenSaveItemCommand();
      command.Initialize(updatedItem);
      command.Engine = new DataEngine(database);

      // act
      command.DoExecute();

      // assert
      database.DataStorage.FakeItems[itemId].Name.Should().Be("updated item");
      database.DataStorage.FakeItems[itemId].Fields[fieldId].Value.Should().Be("updated title");
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