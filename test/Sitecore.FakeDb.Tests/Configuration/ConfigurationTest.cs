namespace Sitecore.FakeDb.Tests.Configuration
{
  using System;
  using FluentAssertions;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data;
  using Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes;
  using Sitecore.FakeDb.Data.IDTables;
  using Xunit;

  public class ConfigurationTest
  {
    [Theory]
    [InlineData("AddFromTemplatePrototype", typeof(AddFromTemplateCommandPrototype))]
    [InlineData("AddVersionPrototype", typeof(AddVersionCommandProtoype))]
    [InlineData("CopyItemPrototype", typeof(CopyItemCommandPrototype))]
    [InlineData("CreateItemPrototype", typeof(CreateItemCommandPrototype))]
    [InlineData("DeletePrototype", typeof(DeleteItemCommandPrototype))]
    [InlineData("GetItemPrototype", typeof(GetItemCommandPrototype))]
    [InlineData("GetVersionsPrototype", typeof(GetVersionsCommandPrototype))]
    [InlineData("MoveItemPrototype", typeof(MoveItemCommandPrototype))]
    [InlineData("RemoveDataPrototype", typeof(RemoveDataCommandPrototype))]
    [InlineData("RemoveVersionPrototype", typeof(RemoveVersionCommandPrototype))]
    [InlineData("SaveItemPrototype", typeof(SaveItemCommandPrototype))]
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
    public void ShouldGetIdTableProvider()
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
    public void ShouldLoadAutoIncludeFiles()
    {
      // arrange & act
      using (new Db())
      {
        // assert
        Settings.GetSetting("Sitecore.FakeDb.AutoInclude.Suported").Should().Be("Yes");
      }
    }

    [Fact]
    public void ShouldLoadAutoIncludeFilesIfNoDbContextCreated()
    {
      // arrange & act
      Settings.GetSetting("Sitecore.FakeDb.AutoInclude.Suported").Should().Be("Yes");
    }
  }
}