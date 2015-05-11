namespace Sitecore.FakeDb.Tests.Configuration
{
  using System;
  using FluentAssertions;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data;
  using Sitecore.FakeDb.Data.IDTables;
  using Xunit;
  using Xunit.Extensions;

  public class ConfigurationTest
  {
    [Theory]
    [InlineData("AddFromTemplatePrototype", typeof(FakeDb.Data.Engines.DataCommands.AddFromTemplateCommand))]
    [InlineData("AddVersionPrototype", typeof(FakeDb.Data.Engines.DataCommands.AddVersionCommand))]
    [InlineData("BlobStreamExistsPrototype", typeof(FakeDb.Data.Engines.DataCommands.BlobStreamExistsCommand))]
    [InlineData("CopyItemPrototype", typeof(FakeDb.Data.Engines.DataCommands.CopyItemCommand))]
    [InlineData("CreateItemPrototype", typeof(FakeDb.Data.Engines.DataCommands.CreateItemCommand))]
    [InlineData("DeletePrototype", typeof(FakeDb.Data.Engines.DataCommands.DeleteItemCommand))]
    [InlineData("GetBlobStreamPrototype", typeof(FakeDb.Data.Engines.DataCommands.GetBlobStreamCommand))]
    [InlineData("GetChildrenPrototype", typeof(FakeDb.Data.Engines.DataCommands.GetChildrenCommand))]
    [InlineData("GetItemPrototype", typeof(FakeDb.Data.Engines.DataCommands.GetItemCommand))]
    [InlineData("GetParentPrototype", typeof(FakeDb.Data.Engines.DataCommands.GetParentCommand))]
    [InlineData("GetRootItemPrototype", typeof(FakeDb.Data.Engines.DataCommands.GetRootItemCommand))]
    [InlineData("GetVersionsPrototype", typeof(FakeDb.Data.Engines.DataCommands.GetVersionsCommand))]
    [InlineData("HasChildrenPrototype", typeof(FakeDb.Data.Engines.DataCommands.HasChildrenCommand))]
    [InlineData("MoveItemPrototype", typeof(FakeDb.Data.Engines.DataCommands.MoveItemCommand))]
    [InlineData("RemoveDataPrototype", typeof(FakeDb.Data.Engines.DataCommands.RemoveDataCommand))]
    [InlineData("RemoveVersionPrototype", typeof(FakeDb.Data.Engines.DataCommands.RemoveVersionCommand))]
    [InlineData("ResolvePathPrototype", typeof(FakeDb.Data.Engines.DataCommands.ResolvePathCommand))]
    [InlineData("SaveItemPrototype", typeof(FakeDb.Data.Engines.DataCommands.SaveItemCommand))]
    [InlineData("SetBlobStreamPrototype", typeof(FakeDb.Data.Engines.DataCommands.SetBlobStreamCommand))]
    public void ShouldRegisterFakeCommand(string propertyName, Type propertyType)
    {
      // arrange
      foreach (var databaseName in new[] { "master", "web", "core" })
      {
        var database = Database.GetDatabase(databaseName);
        var commands = database.Engines.DataEngine.Commands;

        // act
        var propertyInfo = commands.GetType().GetProperty(propertyName);
        var command = propertyInfo.GetValue(commands);

        // assert
        command.Should().BeOfType(propertyType, "Database: \"{0}\"", databaseName);
      }
    }

    [Fact]
    public void CacheShouldBeDisabled()
    {
      // assert
      Settings.Caching.Enabled.Should().BeFalse();
    }

    [Fact]
    public void ShouldGetIDTableProvider()
    {
      // assert
      Factory.GetIDTable().Should().BeOfType<FakeIDTableProvider>();
    }

    [Fact]
    public void ShouldGetFakeStandardValuesProvider()
    {
      // assert
      StandardValuesManager.Provider.Should().BeOfType<FakeStandardValuesProvider>();
    }

    [Fact]
    public void ShouldDisableAllDataProviderCaches()
    {
      // assert
      Factory.GetDatabase("master").GetDataProviders()[0].CacheOptions.DisableAll.Should().BeTrue();
    }

    [Fact]
    public void ShouldSupportAutoIncludeFiles()
    {
      // arrange & act
      using (new Db())
      {
        // assert
        Settings.GetSetting("Sitecore.FakeDb.AutoInclude.Suported").Should().Be("Yes");
      }
    }
  }
}