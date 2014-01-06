namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Items;
  using Xunit;

  public class FakeCreateItemCommandTest
  {
    [Fact]
    public void ShouldCreateItem()
    {
      // arrange
      var command = new OpenFakeCreateItemCommand();
      var destination = ItemHelper.CreateInstance("root");
      command.Initialize(ID.NewID, "home", ID.NewID, destination);
      command.Engine = new DataEngine(Database.GetDatabase("master"));

      // act
      var item = command.DoExecute();

      // assert
      item.Should().NotBeNull();
    }

    private class OpenFakeCreateItemCommand : FakeCreateItemCommand
    {
      public new Item DoExecute()
      {
        return base.DoExecute();
      }
    }
  }
}