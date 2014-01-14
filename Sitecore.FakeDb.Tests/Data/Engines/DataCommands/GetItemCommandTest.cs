namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.Globalization;
  using Xunit;

  public class GetItemCommandTest
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new OpenGetItemCommand();

      // act & assert
      command.CreateInstance().Should().BeOfType<GetItemCommand>();
    }

    [Fact]
    public void ShouldGetItemFromDataStorage()
    {
      // arrange
      var itemId = ID.NewID;
      var database = new FakeDatabase("master");

      var item = ItemHelper.CreateInstance(database);

      var dataStorage = Substitute.For<DataStorage>(database);
      dataStorage.GetSitecoreItem(itemId).Returns(item);
      database.DataStorage = dataStorage;

      var command = new OpenGetItemCommand { Engine = new DataEngine(database) };
      command.Initialize(itemId, Language.Invariant, Version.Latest);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().Be(item);
    }

    private class OpenGetItemCommand : GetItemCommand
    {
      public new Sitecore.Data.Engines.DataCommands.GetItemCommand CreateInstance()
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