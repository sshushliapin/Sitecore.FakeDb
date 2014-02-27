namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.Globalization;
  using Xunit;
  using Version = Sitecore.Data.Version;

  public class GetRootItemCommandTest : CommandTestBase
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new OpenGetRootItemCommand(this.dataStorage);

      // act & assert
      command.CreateInstance().Should().BeOfType<GetRootItemCommand>();
    }

    [Fact]
    public void ShouldReturnRootItem()
    {
      // arrange
      var rootItem = ItemHelper.CreateInstance();

      dataStorage.GetSitecoreItem(ItemIDs.RootID, rootItem.Language).Returns(rootItem);

      var command = new OpenGetRootItemCommand(this.dataStorage) { Engine = new DataEngine(this.database) };
      command.Initialize(Language.Invariant, Version.Latest);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().Be(rootItem);
    }

    private class OpenGetRootItemCommand : GetRootItemCommand
    {
      public OpenGetRootItemCommand(DataStorage dataStorage)
        : base(dataStorage)
      {
      }

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