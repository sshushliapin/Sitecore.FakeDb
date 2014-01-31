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
      var database = Substitute.For<FakeDatabase>("master");
      database.DataStorage = Substitute.For<DataStorage>();

      var itemId = ID.NewID;
      var item = ItemHelper.CreateInstance();

      database.DataStorage.GetSitecoreItem(itemId).Returns(item);

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