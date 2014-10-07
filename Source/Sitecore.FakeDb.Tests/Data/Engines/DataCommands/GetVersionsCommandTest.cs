namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Collections;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.Globalization;
  using Xunit;

  public class GetVersionsCommandTest : CommandTestBase
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var createdCommand = Substitute.For<GetVersionsCommand>();
      this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.GetVersionsCommand, GetVersionsCommand>().Returns(createdCommand);

      var command = new OpenGetVersionsCommand();
      command.Initialize(this.innerCommand);

      // act & assert
      command.CreateInstance().Should().Be(createdCommand);
    }

    [Fact]
    public void ShouldGetVersionCollectionWinthSingleVersion()
    {
      // arrange
      var itemId = ID.NewID;
      this.dataStorage.GetFakeItem(itemId).Returns(new DbItem("item"));

      var item = ItemHelper.CreateInstance(itemId, this.database);
      var language = Language.Parse("en");

      var command = new OpenGetVersionsCommand();
      command.Initialize(item, language);
      command.Initialize(this.innerCommand);

      // act
      var versionCollection = command.DoExecute();

      // assert
      versionCollection.Should().ContainSingle(v => v.Number == 1);
    }

    [Fact]
    public void ShouldGetItemVersionsForLanguage()
    {
      // arrange
      var itemId = ID.NewID;
      var versionedItem = new DbItem("item") { Fields = { new DbField("Title") { { "en", 1, "value1" }, { "en", 2, "value2" } } } };
      this.dataStorage.GetFakeItem(itemId).Returns(versionedItem);

      var item = ItemHelper.CreateInstance(itemId, this.database);
      var language = Language.Parse("en");

      var command = new OpenGetVersionsCommand();
      command.Initialize(item, language);
      command.Initialize(this.innerCommand);

      // act
      var versionCollection = command.DoExecute();

      // assert
      versionCollection.Count.Should().Be(2);
      versionCollection.Should().BeEquivalentTo(new[] { new Version(1), new Version(2) });
    }

    [Fact]
    public void ShouldGetItemVersionsCount()
    {
      // arrange
      var itemId = ID.NewID;
      var versionedItem = new DbItem("item");
      versionedItem.VersionsCount.Add("en", 2);

      this.dataStorage.GetFakeItem(itemId).Returns(versionedItem);

      var item = ItemHelper.CreateInstance(itemId, this.database);
      var language = Language.Parse("en");

      var command = new OpenGetVersionsCommand();
      command.Initialize(item, language);
      command.Initialize(this.innerCommand);

      // act
      var versionCollection = command.DoExecute();

      // assert
      versionCollection.Count.Should().Be(2);
      versionCollection.Should().BeEquivalentTo(new[] { new Version(1), new Version(2) });
    }

    private class OpenGetVersionsCommand : GetVersionsCommand
    {
      public new Sitecore.Data.Engines.DataCommands.GetVersionsCommand CreateInstance()
      {
        return base.CreateInstance();
      }

      public new VersionCollection DoExecute()
      {
        return base.DoExecute();
      }
    }
  }
}