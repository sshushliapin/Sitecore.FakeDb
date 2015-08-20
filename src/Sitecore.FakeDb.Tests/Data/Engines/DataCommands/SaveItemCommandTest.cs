namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using System;
  using System.Reflection;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.Reflection;
  using Sitecore.StringExtensions;
  using Xunit;
  using SaveItemCommand = Sitecore.FakeDb.Data.Engines.DataCommands.SaveItemCommand;

  public class SaveItemCommandTest
  {
    [Theory, DefaultAutoData]
    public void ShouldUpdateExistingItemInDataStorage(SaveItemCommand sut, ID itemId, ID templateId, ID fieldId)
    {
      // arrange
      var originalItem = new DbItem("original item", itemId) { new DbField("Title", fieldId) { Value = "original title" } };
      sut.DataStorage.GetFakeItem(itemId).Returns(originalItem);
      sut.DataStorage.GetFakeTemplate(null).ReturnsForAnyArgs(new DbTemplate("Sample", templateId));

      var fields = new FieldList { { fieldId, "updated title" } };
      var updatedItem = ItemHelper.CreateInstance(sut.Database, "updated item", itemId, ID.NewID, ID.Null, fields);

      sut.Initialize(updatedItem);

      // act
      ReflectionUtil.CallMethod(sut, "DoExecute");

      // assert
      originalItem.Name.Should().Be("updated item");
      originalItem.Fields[fieldId].Value.Should().Be("updated title");
    }

    [Theory, DefaultAutoData]
    public void ShouldThrowExceptionIfNoTemplateFound(SaveItemCommand sut, ID itemId, ID templateId)
    {
      // arrange
      var originalItem = new DbItem("original item", itemId, templateId);
      sut.DataStorage.GetFakeItem(itemId).Returns(originalItem);
      sut.DataStorage.GetFakeTemplate(templateId).Returns(x => null);

      var updatedItem = ItemHelper.CreateInstance(sut.Database, "updated item", itemId, templateId);

      sut.Initialize(updatedItem);

      // act
      Action action = () => ReflectionUtil.CallMethod(sut, "DoExecute");

      // assert
      action
        .ShouldThrow<TargetInvocationException>()
        .WithInnerException<InvalidOperationException>()
        .WithInnerMessage("Item template not found. Item: 'updated item', '{0}'; template: '{1}'.".FormatWith(itemId, templateId));
    }

    [Theory, DefaultAutoData]
    public void ShouldThrowExceptionIfNoFieldFoundInOriginalItem(SaveItemCommand sut, ID itemId, ID templateId, ID fieldId)
    {
      // arrange
      var originalItem = new DbItem("original item", itemId) { new DbField("Title") };
      sut.DataStorage.GetFakeItem(itemId).Returns(originalItem);
      sut.DataStorage.GetFakeTemplate(null).ReturnsForAnyArgs(new DbTemplate("Sample", templateId));

      var fields = new FieldList { { fieldId, "updated title" } };
      var updatedItem = ItemHelper.CreateInstance(sut.Database, "updated item", itemId, ID.NewID, ID.Null, fields);

      sut.Initialize(updatedItem);

      // act
      Action action = () => ReflectionUtil.CallMethod(sut, "DoExecute");

      // assert
      action
        .ShouldThrow<TargetInvocationException>()
        .WithInnerException<InvalidOperationException>()
        .WithInnerMessage("Item field not found. Item: 'updated item', '{0}'; field: '{1}'.".FormatWith(itemId, fieldId));
    }

    [Theory]
    [InlineDefaultAutoData("/sitecore/content/original item", "/sitecore/content/updated item")]
    [InlineDefaultAutoData("/sitecore/content/original item/original item", "/sitecore/content/original item/updated item")]
    public void ShouldUpdateItemPathAfterRename(string originalPath, string expectedPath, SaveItemCommand sut, ID itemId, ID templateId)
    {
      // arrange
      var originalItem = new DbItem("original item") { FullPath = originalPath };
      sut.DataStorage.GetFakeItem(itemId).Returns(originalItem);
      sut.DataStorage.GetFakeTemplate(null).ReturnsForAnyArgs(new DbTemplate("Sample", templateId));

      var updatedItem = ItemHelper.CreateInstance(sut.Database, "updated item", itemId);
      sut.Initialize(updatedItem);

      // act
      ReflectionUtil.CallMethod(sut, "DoExecute");

      // assertt
      originalItem.FullPath.Should().Be(expectedPath);
    }

    [Theory, DefaultAutoData]
    public void ShouldAddMissingFieldToItemIfFieldExistsInTemplate(SaveItemCommand sut, ID itemId, ID templateId, ID fieldId)
    {
      // arrange
      var template = new DbTemplate("Sample", templateId) { fieldId };
      var originalItem = new DbItem("original item", itemId, templateId);

      sut.DataStorage.GetFakeItem(itemId).Returns(originalItem);
      sut.DataStorage.GetFakeTemplate(templateId).Returns(template);

      var fields = new FieldList { { fieldId, "updated title" } };
      var updatedItem = ItemHelper.CreateInstance(sut.Database, "updated item", itemId, ID.NewID, ID.Null, fields);

      sut.Initialize(updatedItem);

      // act
      ReflectionUtil.CallMethod(sut, "DoExecute");

      // assert
      originalItem.Name.Should().Be("updated item");
      originalItem.Fields[fieldId].Value.Should().Be("updated title");
    }
  }
}