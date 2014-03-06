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

  public class AddFromTemplateCommandTest : CommandTestBase
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var createdCommand = Substitute.For<AddFromTemplateCommand>();
      this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand, AddFromTemplateCommand>().Returns(createdCommand);

      var command = new OpenAddFromTemplateCommand();
      command.Initialize(this.innerCommand);

      // act & assert
      command.CreateInstance().Should().Be(createdCommand);
    }

    [Fact]
    public void ShouldCreateDefaultCreator()
    {
      // arrange
      var command = new AddFromTemplateCommand();
      command.Initialize(this.innerCommand);

      // act & assert
      command.ItemCreator.Should().NotBeNull();
      command.ItemCreator.DataStorage.Should().Be(this.dataStorage);
    }

    [Fact]
    public void ShouldCreateItem()
    {
      // arrange
      var itemId = ID.NewID;
      var templateId = ID.NewID;

      var item = ItemHelper.CreateInstance(this.database);
      var destination = ItemHelper.CreateInstance(this.database);

      var itemCreator = Substitute.For<ItemCreator>(this.dataStorage);
      itemCreator.Create("home", itemId, templateId, database, destination).Returns(item);

      var command = new OpenAddFromTemplateCommand { Engine = new DataEngine(database), ItemCreator = itemCreator };
      command.Initialize("home", templateId, destination, itemId);
      command.Initialize(this.innerCommand);

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