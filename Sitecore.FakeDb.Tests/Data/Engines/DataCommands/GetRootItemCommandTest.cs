namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.Globalization;
  using Xunit;

  public class GetRootItemCommandTest
  {
    private readonly OpenGetRootItemCommand command;

    public GetRootItemCommandTest()
    {
      this.command = new OpenGetRootItemCommand { Engine = new DataEngine(Database.GetDatabase("master")) };
    }

    [Fact]
    public void ShouldCreateInstance()
    {
      // act & assert
      command.CreateInstance().Should().BeOfType<GetRootItemCommand>();
    }

    [Fact]
    public void ShouldReturnRootItem()
    {
      // arrange
      this.command.Initialize(Language.Invariant, Version.Latest);

      // act
      var item = this.command.DoExecute();

      // assert
      item.ID.Should().Be(ItemIDs.RootID);
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