namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using System.Linq;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.Globalization;
  using Xunit;

  public class AddVersionCommandTest : CommandTestBase
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new OpenAddVersionCommand();
      command.Initialize(this.dataStorage);

      // act & assert
      command.CreateInstance().Should().BeOfType<AddVersionCommand>();
    }

    [Fact]
    public void ShouldAddVersionToFakeDbFieldsUsingItemLanguage()
    {
      // arrange
      var itemId = ID.NewID;
      var dbitem = new DbItem("item") { Fields = { new DbField("Title") { { "en", "Hello!" }, { "da", "Hej!" } } } };
      this.dataStorage.GetFakeItem(itemId).Returns(dbitem);

      var item = ItemHelper.CreateInstance(this.database, itemId);

      var command = new OpenAddVersionCommand();
      command.Initialize(item);
      command.Initialize(this.dataStorage);

      // act
      command.DoExecute();

      // assert
      dbitem.Fields.Single().Values["en"][1].Should().Be("Hello!");
      dbitem.Fields.Single().Values["en"][2].Should().Be("Hello!");
      dbitem.Fields.Single().Values["da"][1].Should().Be("Hej!");
      dbitem.Fields.Single().Values["da"].ContainsKey(2).Should().BeFalse();
    }

    [Fact]
    public void ShouldGetNewItemVersion()
    {
      // arrange
      var itemId = ID.NewID;
      var dbitem = new DbItem("home") { { "Title", "Hello!" } };
      this.dataStorage.GetFakeItem(itemId).Returns(dbitem);

      var originalItem = ItemHelper.CreateInstance(this.database, itemId);
      var itemWithNewVersion = ItemHelper.CreateInstance(this.database, itemId);
      this.dataStorage.GetSitecoreItem(itemId, Language.Parse("en"), Version.Parse(2)).Returns(itemWithNewVersion);

      var command = new OpenAddVersionCommand();
      command.Initialize(originalItem);
      command.Initialize(this.dataStorage);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().BeSameAs(itemWithNewVersion);
    }

    [Fact]
    public void ShouldAddVersionIfNoVersionExistsInSpecificLanguage()
    {
      // arrange
      var itemId = ID.NewID;
      var dbitem = new DbItem("item") { Fields = { new DbField("Title") } };
      this.dataStorage.GetFakeItem(itemId).Returns(dbitem);

      var item = ItemHelper.CreateInstance(this.database, itemId);

      var command = new OpenAddVersionCommand();
      command.Initialize(item);
      command.Initialize(this.dataStorage);

      // act
      command.DoExecute();

      // assert
      dbitem.Fields.Single().Values["en"][1].Should().BeEmpty();
    }

    [Fact]
    public void ShouldIncreaseFakeItemVersionCount()
    {
      // arrange
      var itemId = ID.NewID;
      var dbitem = new DbItem("item");
      this.dataStorage.GetFakeItem(itemId).Returns(dbitem);

      var item = ItemHelper.CreateInstance(this.database, itemId);

      var command = new OpenAddVersionCommand();
      command.Initialize(item);
      command.Initialize(this.dataStorage);

      // act
      command.DoExecute();

      // assert
      dbitem.VersionsCount["en"].Should().Be(2);
    }

    private class OpenAddVersionCommand : AddVersionCommand
    {
      public new Sitecore.Data.Engines.DataCommands.AddVersionCommand CreateInstance()
      {
        return base.CreateInstance();
      }

      public new Item DoExecute()
      {
        return base.DoExecute();
      }
    }
  }
}