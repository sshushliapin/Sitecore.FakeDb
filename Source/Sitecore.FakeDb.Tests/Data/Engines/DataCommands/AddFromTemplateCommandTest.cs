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
      var command = new OpenAddFromTemplateCommand(this.dataStorage);

      // act & assert
      command.CreateInstance().Should().BeOfType<AddFromTemplateCommand>();
    }

    [Fact]
    public void ShouldCreateDefaultCreator()
    {
      // arrange
      var command = new AddFromTemplateCommand(this.dataStorage);

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

      var item = ItemHelper.CreateInstance();
      var destination = ItemHelper.CreateInstance();

      var itemCreator = Substitute.For<ItemCreator>(this.dataStorage);
      itemCreator.Create("home", itemId, templateId, database, destination).Returns(item);

      var command = new OpenAddFromTemplateCommand(this.dataStorage) { Engine = new DataEngine(database), ItemCreator = itemCreator };
      command.Initialize("home", templateId, destination, itemId);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().Be(item);
    }

    private class OpenAddFromTemplateCommand : AddFromTemplateCommand
    {
      public OpenAddFromTemplateCommand(DataStorage dataStorage)
        : base(dataStorage)
      {
      }

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