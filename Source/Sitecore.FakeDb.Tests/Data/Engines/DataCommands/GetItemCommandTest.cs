namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class GetItemCommandTest : CommandTestBase
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new OpenGetItemCommand();
      command.Initialize(this.dataStorage);

      // act & assert
      command.CreateInstance().Should().BeOfType<GetItemCommand>();
    }

    [Fact]
    public void ShouldGetItemFromDataStorage()
    {
      // arrange
      var itemId = ID.NewID;
      var item = ItemHelper.CreateInstance(this.database);

      this.dataStorage.GetSitecoreItem(itemId, item.Language, item.Version).Returns(item);

      var command = new OpenGetItemCommand { Engine = new DataEngine(this.database) };
      command.Initialize(itemId, item.Language, item.Version);
      command.Initialize(this.dataStorage);

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