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

  public class CreateItemCommandTest
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new OpenCreateItemCommand();

      // act & assert
      command.CreateInstance().Should().BeOfType<CreateItemCommand>();
    }

    [Fact]
    public void ShouldCreateDefaultCreator()
    {
      // arrange
      var command = new CreateItemCommand();

      // act & assert
      command.ItemCreator.Should().NotBeNull();
    }

    [Fact]
    public void ShouldCreateItem()
    {
      // arrange
      var database = Substitute.For<Database>("master");

      var itemId = ID.NewID;
      var templateId = ID.NewID;

      var item = ItemHelper.CreateInstance();
      var destination = ItemHelper.CreateInstance();

      var itemCreator = Substitute.For<ItemCreator>();
      itemCreator.Create("home", itemId, templateId, database, destination).Returns(item);

      var command = new OpenCreateItemCommand { Engine = new DataEngine(database), ItemCreator = itemCreator };
      command.Initialize(itemId, "home", templateId, destination);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().Be(item);
    }

    private class OpenCreateItemCommand : CreateItemCommand
    {
      public new Sitecore.Data.Engines.DataCommands.CreateItemCommand CreateInstance()
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