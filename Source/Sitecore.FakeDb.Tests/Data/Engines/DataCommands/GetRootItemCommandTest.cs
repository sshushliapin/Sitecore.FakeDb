namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;
  using Version = Sitecore.Data.Version;

  public class GetRootItemCommandTest : CommandTestBase
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var createdCommand = Substitute.For<GetRootItemCommand>();
      this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.GetRootItemCommand, GetRootItemCommand>().Returns(createdCommand);

      var command = new OpenGetRootItemCommand();
      command.Initialize(this.innerCommand);

      // act & assert
      command.CreateInstance().Should().Be(createdCommand);
    }

    [Fact]
    public void ShouldReturnRootItem()
    {
      // arrange
      var rootItem = ItemHelper.CreateInstance(this.database);

      dataStorage.GetSitecoreItem(ItemIDs.RootID, rootItem.Language).Returns(rootItem);

      var command = new OpenGetRootItemCommand { Engine = new DataEngine(this.database) };
      command.Initialize(LanguageManager.DefaultLanguage, Version.Latest);
      command.Initialize(this.innerCommand);

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