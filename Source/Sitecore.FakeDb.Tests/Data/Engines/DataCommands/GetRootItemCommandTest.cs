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

  public class GetRootItemCommandTest
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new OpenGetRootItemCommand();

      // act & assert
      command.CreateInstance().Should().BeOfType<GetRootItemCommand>();
    }

    [Fact]
    public void ShouldReturnRootItem()
    {
      // arrange
      var database = Substitute.For<FakeDatabase>("master");
      database.DataStorage = Substitute.For<DataStorage>();

      var rootItem = ItemHelper.CreateInstance();

      database.DataStorage.GetSitecoreItem(ItemIDs.RootID, rootItem.Language).Returns(rootItem);

      var command = new OpenGetRootItemCommand { Engine = new DataEngine(database) };
      command.Initialize(Language.Invariant, Version.Latest);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().Be(rootItem);
    }

    private class OpenGetRootItemCommand : GetRootItemCommand
    {
      public new Sitecore.Data.Engines.DataCommands.GetRootItemCommand CreateInstance()
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