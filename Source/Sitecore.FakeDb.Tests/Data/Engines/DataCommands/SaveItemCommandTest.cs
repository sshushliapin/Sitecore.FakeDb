namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using Sitecore.Common;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;
  using SaveItemCommand = Sitecore.FakeDb.Data.Engines.DataCommands.SaveItemCommand;

  public class SaveItemCommandTest : CommandTestBase
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new OpenSaveItemCommand(this.dataStorage);

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

      dataStorage.FakeItems.Add(itemId, new DbItem("original item"));

      var fieldId = ID.NewID;
      var fields = new FieldList { { fieldId, "updated title" } };
      var updatedItem = ItemHelper.CreateInstance("updated item", itemId, ID.NewID, fields, database);

      var command = new OpenSaveItemCommand(this.dataStorage) { Engine = new DataEngine(this.database) };
      command.Initialize(updatedItem);

      using (new Switcher<DataStorage>(dataStorage))
      {
        // act
        command.DoExecute();

        // assert
        dataStorage.FakeItems[itemId].Name.Should().Be("updated item");
        dataStorage.FakeItems[itemId].Fields[fieldId].Value.Should().Be("updated title");
      }
    }

    private class OpenSaveItemCommand : SaveItemCommand
    {
      public OpenSaveItemCommand(DataStorage dataStorage)
        : base(dataStorage)
      {
      }

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