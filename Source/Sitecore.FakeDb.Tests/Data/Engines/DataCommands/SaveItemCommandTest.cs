namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Managers;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Items;
  using System;
  using Xunit;
  using Xunit.Extensions;
  using SaveItemCommand = Sitecore.FakeDb.Data.Engines.DataCommands.SaveItemCommand;

  public class SaveItemCommandTest : CommandTestBase
  {
    private readonly OpenSaveItemCommand command;

    public SaveItemCommandTest()
    {
      this.command = new OpenSaveItemCommand(this.dataStorage) { Engine = new DataEngine(this.database) };
    }

    [Fact]
    public void ShouldCreateInstance()
    {
      // act
      var instance = this.command.CreateInstance();

      // assert
      instance.Should().BeOfType<SaveItemCommand>();
    }

    [Fact]
    public void ShouldUpdateExistingItemInDataStorage()
    {
      // arrange
      var itemId = ID.NewID;

      var originalItem = new DbItem("original item");
      this.dataStorage.GetFakeItem(itemId).Returns(originalItem);
      this.dataStorage.FakeItems.Add(itemId, originalItem);

      var fieldId = ID.NewID;
      var fields = new FieldList { { fieldId, "updated title" } };
      var updatedItem = ItemHelper.CreateInstance("updated item", itemId, ID.NewID, fields, database, LanguageManager.DefaultLanguage);

      this.command.Initialize(updatedItem);

      // act
      this.command.DoExecute();

      // assert
      dataStorage.FakeItems[itemId].Name.Should().Be("updated item");
      dataStorage.FakeItems[itemId].Fields[fieldId].Value.Should().Be("updated title");
    }

    [Theory]
    [InlineData("/sitecore/content/original item", "/sitecore/content/updated item")]
    [InlineData("/sitecore/content/original item/original item", "/sitecore/content/original item/updated item")]
    public void ShouldUpdateItemPathAfterRename(string originalPath, string expectedPath)
    {
      // arrange
      var itemId = ID.NewID;

      var originalItem = new DbItem("original item") { FullPath = originalPath };
      this.dataStorage.GetFakeItem(itemId).Returns(originalItem);
      this.dataStorage.FakeItems.Add(itemId, originalItem);

      var updatedItem = ItemHelper.CreateInstance("updated item", itemId, this.database);

      this.command.Initialize(updatedItem);

      // act
      this.command.DoExecute();

      // assertt
      dataStorage.FakeItems[itemId].FullPath.Should().Be(expectedPath);
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