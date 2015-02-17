namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using System;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.StringExtensions;
  using Xunit;
  using Xunit.Extensions;
  using SaveItemCommand = Sitecore.FakeDb.Data.Engines.DataCommands.SaveItemCommand;

  public class SaveItemCommandTest : CommandTestBase
  {
    private readonly ID templateId;

    private readonly ID itemId;

    private readonly ID fieldId;

    private readonly OpenSaveItemCommand command;

    public SaveItemCommandTest()
    {
      this.templateId = ID.NewID;
      this.itemId = ID.NewID;
      this.fieldId = ID.NewID;

      this.dataStorage.GetFakeTemplate(null).ReturnsForAnyArgs(new DbTemplate("Sample", this.templateId));

      this.command = new OpenSaveItemCommand { Engine = new DataEngine(this.database) };
      this.command.Initialize(this.dataStorage);
    }

    [Fact]
    public void ShouldCreateInstance()
    {
      // act & assert
      this.command.CreateInstance().Should().BeOfType<SaveItemCommand>();
    }

    [Fact]
    public void ShouldUpdateExistingItemInDataStorage()
    {
      // arrange
      var originalItem = new DbItem("original item", this.itemId) { new DbField("Title", this.fieldId) { Value = "original title" } };
      this.dataStorage.GetFakeItem(this.itemId).Returns(originalItem);
      this.dataStorage.FakeItems.Add(this.itemId, originalItem);

      var fields = new FieldList { { this.fieldId, "updated title" } };
      var updatedItem = ItemHelper.CreateInstance(database, "updated item", this.itemId, ID.NewID, ID.Null, fields);

      this.command.Initialize(updatedItem);

      // act
      this.command.DoExecute();

      // assert
      dataStorage.FakeItems[this.itemId].Name.Should().Be("updated item");
      dataStorage.FakeItems[this.itemId].Fields[this.fieldId].Value.Should().Be("updated title");
    }

    [Fact]
    public void ShouldThrowExceptionIfNoTemplateFound()
    {
      // arrange
      var originalItem = new DbItem("original item", this.itemId, this.templateId);
      this.dataStorage.GetFakeItem(this.itemId).Returns(originalItem);
      this.dataStorage.GetFakeTemplate(this.templateId).Returns(x => null);

      var updatedItem = ItemHelper.CreateInstance(this.database, "updated item", this.itemId, this.templateId);

      this.command.Initialize(updatedItem);

      // act
      Action action = () => this.command.DoExecute();

      // assert
      action.ShouldThrow<InvalidOperationException>().WithMessage("Item template not found. Item: 'updated item', '{0}'; template: '{1}'.".FormatWith(this.itemId, this.templateId));
    }

    [Fact]
    public void ShouldThrowExceptionIfNoFieldFoundInOriginalItem()
    {
      // arrange
      var originalItem = new DbItem("original item", this.itemId) { new DbField("Title") };
      this.dataStorage.GetFakeItem(this.itemId).Returns(originalItem);

      var fields = new FieldList { { this.fieldId, "updated title" } };
      var updatedItem = ItemHelper.CreateInstance(this.database, "updated item", this.itemId, ID.NewID, ID.Null, fields);

      this.command.Initialize(updatedItem);

      // act
      Action action = () => this.command.DoExecute();

      // assert
      action.ShouldThrow<InvalidOperationException>().WithMessage("Item field not found. Item: 'updated item', '{0}'; field: '{1}'.".FormatWith(this.itemId, this.fieldId));
    }

    [Theory]
    [InlineData("/sitecore/content/original item", "/sitecore/content/updated item")]
    [InlineData("/sitecore/content/original item/original item", "/sitecore/content/original item/updated item")]
    public void ShouldUpdateItemPathAfterRename(string originalPath, string expectedPath)
    {
      // arrange
      var originalItem = new DbItem("original item") { FullPath = originalPath };
      this.dataStorage.GetFakeItem(this.itemId).Returns(originalItem);
      this.dataStorage.FakeItems.Add(this.itemId, originalItem);

      var updatedItem = ItemHelper.CreateInstance(this.database, "updated item", this.itemId);
      this.command.Initialize(updatedItem);

      // act
      this.command.DoExecute();

      // assertt
      this.dataStorage.FakeItems[this.itemId].FullPath.Should().Be(expectedPath);
    }

    [Fact]
    public void ShouldAddMissingFieldToItemIfFieldExistsInTemplate()
    {
      // arrange
      var template = new DbTemplate("Sample", this.templateId) { this.fieldId };
      var originalItem = new DbItem("original item", this.itemId, this.templateId);

      this.dataStorage.GetFakeItem(this.itemId).Returns(originalItem);
      this.dataStorage.GetFakeTemplate(this.templateId).Returns(template);
      this.dataStorage.FakeItems.Add(this.itemId, originalItem);

      var fields = new FieldList { { this.fieldId, "updated title" } };
      var updatedItem = ItemHelper.CreateInstance(this.database, "updated item", this.itemId, ID.NewID, ID.Null, fields);

      this.command.Initialize(updatedItem);

      // act
      this.command.DoExecute();

      // assert
      dataStorage.FakeItems[this.itemId].Name.Should().Be("updated item");
      dataStorage.FakeItems[this.itemId].Fields[this.fieldId].Value.Should().Be("updated title");
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