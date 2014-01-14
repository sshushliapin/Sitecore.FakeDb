namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class AddFromTemplateCommandTest
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new OpenAddFromTemplateCommand();

      // act & assert
      command.CreateInstance().Should().BeOfType<AddFromTemplateCommand>();
    }

    [Fact]
    public void ShouldCreateDefaultCreator()
    {
      // arrange
      var command = new AddFromTemplateCommand();

      // act & assert
      command.ItemCreator.Should().NotBeNull();
    }

    [Fact]
    public void ShouldCreateItem()
    {
      // arrange
      var item = ItemHelper.CreateInstance("home");
      var itemId = ID.NewID;
      var templateId = ID.NewID;
      var database = Substitute.For<Database>("master");
      var destination = ItemHelper.CreateInstance("parent");

      var itemCreator = Substitute.For<ItemCreator>();
      itemCreator.Create("home", itemId, templateId, database, destination).Returns(item);

      var command = new OpenAddFromTemplateCommand
      {
        Engine = new DataEngine(database),
        ItemCreator = itemCreator
      };
      command.Initialize("home", templateId, destination, itemId);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().Be(item);
    }

    private class OpenAddFromTemplateCommand : AddFromTemplateCommand
    {
      public new Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand CreateInstance()
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