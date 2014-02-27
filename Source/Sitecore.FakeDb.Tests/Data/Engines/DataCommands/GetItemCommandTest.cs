namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Common;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class GetItemCommandTest : CommandTestBase
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new OpenGetItemCommand(this.dataStorage);

      // act & assert
      command.CreateInstance().Should().BeOfType<GetItemCommand>();
    }

    [Fact]
    public void ShouldGetItemFromDataStorage()
    {
      // arrange
      var itemId = ID.NewID;
      var item = ItemHelper.CreateInstance();

      this.dataStorage.GetSitecoreItem(itemId, item.Language).Returns(item);

      var command = new OpenGetItemCommand(this.dataStorage) { Engine = new DataEngine(this.database) };
      command.Initialize(itemId, item.Language, Version.Latest);

      // act
      var result = command.DoExecute();

      // assert
      result.Should().Be(item);
    }

    private class OpenGetItemCommand : GetItemCommand
    {
      public OpenGetItemCommand(DataStorage dataStorage)
        : base(dataStorage)
      {
      }

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