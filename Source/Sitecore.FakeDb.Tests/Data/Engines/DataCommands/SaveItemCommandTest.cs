namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using System;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.Globalization;
  using Sitecore.StringExtensions;
  using Xunit;
  using Xunit.Extensions;
  using SaveItemCommand = Sitecore.FakeDb.Data.Engines.DataCommands.SaveItemCommand;

  public class SaveItemCommandTest : CommandTestBase
  {
    private readonly OpenSaveItemCommand command;

    public SaveItemCommandTest()
    {
      this.command = new OpenSaveItemCommand { Engine = new DataEngine(this.database) };
      this.command.Initialize(this.innerCommand);
    }

    [Fact]
    public void ShouldCreateInstance()
    {
      // arsange
      var createdCommand = Substitute.For<SaveItemCommand>();
      this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.SaveItemCommand, SaveItemCommand>().Returns(createdCommand);

      // act & assert
      this.command.CreateInstance().Should().Be(createdCommand);
    }

    [Fact]
    public void ShouldUpdateExistingItemInDataStorage()
    {
      // arrange
      var itemId = ID.NewID;
      var fieldId = ID.NewID;

      var originalItem = new DbItem("original item", itemId) { new DbField("Title", fieldId) { Value = "original title" } };
      this.dataStorage.GetFakeItem(itemId).Returns(originalItem);
      this.dataStorage.FakeItems.Add(itemId, originalItem);

      var fields = new FieldList { { fieldId, "updated title" } };
      var updatedItem = ItemHelper.CreateInstance("updated item", itemId, ID.NewID, fields, database, Language.Current);

      this.command.Initialize(updatedItem);

      // act
      this.command.DoExecute();

      // assert
      dataStorage.FakeItems[itemId].Name.Should().Be("updated item");
      dataStorage.FakeItems[itemId].Fields[fieldId].Value.Should().Be("updated title");
    }

    [Fact]
    public void ShouldThrowExceptionIfNoFieldFoundInOriginalItem()
    {
      // arrange
      var itemId = ID.NewID;
      var fieldId = ID.NewID;

      var originalItem = new DbItem("original item", itemId) { new DbField("Title") };
      this.dataStorage.GetFakeItem(itemId).Returns(originalItem);

      var fields = new FieldList { { fieldId, "updated title" } };
      var updatedItem = ItemHelper.CreateInstance("updated item", itemId, ID.NewID, fields, database, Language.Current);

      this.command.Initialize(updatedItem);

      // act
      Action action = () => this.command.DoExecute();

      // assert
      action.ShouldThrow<InvalidOperationException>().WithMessage("Item field not found. Item: 'updated item', '{0}'; field: '{1}'.".FormatWith(itemId, fieldId));
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