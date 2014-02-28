namespace Sitecore.FakeDb.Tests.Data
{
  using System;
  using FluentAssertions;
  using Sitecore.Data;
  using Xunit.Extensions;

  public class FakeDatabaseTest
  {
    [Theory]
    [InlineData("AddFromTemplatePrototype", typeof(Sitecore.FakeDb.Data.Engines.DataCommands.AddFromTemplateCommand))]
    [InlineData("AddVersionPrototype", typeof(Sitecore.FakeDb.Data.Engines.DataCommands.AddVersionCommand))]
    //[InlineData("BlobStreamExistsPrototype", typeof(Sitecore.FakeDb.Data.Engines.DataCommands.BlobStreamExistsCommand))]
    //[InlineData("CopyItemPrototype", typeof(Sitecore.FakeDb.Data.Engines.DataCommands.CopyItemCommand))]
    [InlineData("CreateItemPrototype", typeof(Sitecore.FakeDb.Data.Engines.DataCommands.CreateItemCommand))]
    [InlineData("DeletePrototype", typeof(Sitecore.FakeDb.Data.Engines.DataCommands.DeleteItemCommand))]
    //[InlineData("GetBlobStreamPrototype", typeof(Sitecore.FakeDb.Data.Engines.DataCommands.GetBlobStreamCommand))]
    [InlineData("GetChildrenPrototype", typeof(Sitecore.FakeDb.Data.Engines.DataCommands.GetChildrenCommand))]
    [InlineData("GetItemPrototype", typeof(Sitecore.FakeDb.Data.Engines.DataCommands.GetItemCommand))]
    [InlineData("GetParentPrototype", typeof(Sitecore.FakeDb.Data.Engines.DataCommands.GetParentCommand))]
    [InlineData("GetRootItemPrototype", typeof(Sitecore.FakeDb.Data.Engines.DataCommands.GetRootItemCommand))]
    [InlineData("GetVersionsPrototype", typeof(Sitecore.FakeDb.Data.Engines.DataCommands.GetVersionsCommand))]
    [InlineData("HasChildrenPrototype", typeof(Sitecore.FakeDb.Data.Engines.DataCommands.HasChildrenCommand))]
    //[InlineData("MoveItemPrototype", typeof(Sitecore.FakeDb.Data.Engines.DataCommands.MoveItemCommand))]
    //[InlineData("RemoveDataPrototype", typeof(Sitecore.FakeDb.Data.Engines.DataCommands.RemoveDataCommand))]
    //[InlineData("RemoveVersionPrototype", typeof(Sitecore.FakeDb.Data.Engines.DataCommands.RemoveVersionCommand))]
    [InlineData("ResolvePathPrototype", typeof(Sitecore.FakeDb.Data.Engines.DataCommands.ResolvePathCommand))]
    [InlineData("SaveItemPrototype", typeof(Sitecore.FakeDb.Data.Engines.DataCommands.SaveItemCommand))]
    //[InlineData("SetBlobStreamPrototype", typeof(Sitecore.FakeDb.Data.Engines.DataCommands.SetBlobStreamCommand))]
    public void ShouldRegisterFakeCommand(string propertyName, Type propertyType)
    {
      // arrange
      var database = Database.GetDatabase("master");
      var commands = database.Engines.DataEngine.Commands;

      // act
      var propertyInfo = commands.GetType().GetProperty(propertyName);
      var command = propertyInfo.GetValue(commands);

      // assert
      command.Should().BeOfType(propertyType);
    }
  }
}