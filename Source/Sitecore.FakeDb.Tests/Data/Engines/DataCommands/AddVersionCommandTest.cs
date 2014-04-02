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
      var createdCommand = Substitute.For<AddVersionCommand>();
      this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.AddVersionCommand, AddVersionCommand>().Returns(createdCommand);

      var command = new OpenAddVersionCommand();
      command.Initialize(this.innerCommand);

      // act & assert
      command.CreateInstance().Should().Be(createdCommand);
    }

    [Fact]
    public void ShouldAddVersionToFakeDbFieldsUsingItemLanguage()
    {
      // arrange
      var itemId = ID.NewID;
      var fieldId = ID.NewID;
      var dbitem = new DbItem("item") { Fields = { new DbField(fieldId, "Title") { { "en", "Hello!" }, { "da", "Hej!" } } } };
      this.dataStorage.GetFakeItem(itemId).Returns(dbitem);

      var item = ItemHelper.CreateInstance(itemId, this.database);

      var command = new OpenAddVersionCommand();
      command.Initialize(item);
      command.Initialize(this.innerCommand);

      // act
      command.DoExecute();

      // assert
      dbitem.Fields[fieldId].Values["en"][1].Should().Be("Hello!");
      dbitem.Fields[fieldId].Values["en"][2].Should().Be("Hello!");
      dbitem.Fields[fieldId].Values["da"][1].Should().Be("Hej!");
      dbitem.Fields[fieldId].Values["da"].ContainsKey(2).Should().BeFalse();
    }

    [Fact]
    public void ShouldGetNewItemVersion()
    {
      // arrange
      var itemId = ID.NewID;
      var dbitem = new DbItem("home") { { "Title", "Hello!" } };
      this.dataStorage.GetFakeItem(itemId).Returns(dbitem);

      var originalItem = ItemHelper.CreateInstance(itemId, this.database);
      var itemWithNewVersion = ItemHelper.CreateInstance(itemId, this.database);
      this.dataStorage.GetSitecoreItem(itemId, Language.Parse("en"), Version.Parse(2)).Returns(itemWithNewVersion);

      var command = new OpenAddVersionCommand();
      command.Initialize(originalItem);
      command.Initialize(this.innerCommand);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().BeSameAs(itemWithNewVersion);
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