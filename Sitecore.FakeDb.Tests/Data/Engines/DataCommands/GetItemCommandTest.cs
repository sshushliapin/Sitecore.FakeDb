namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Xunit;

  public class GetItemCommandTest
  {
    private readonly OpenGetItemCommand command;

    public GetItemCommandTest()
    {
      this.command = new OpenGetItemCommand { Engine = new DataEngine(Database.GetDatabase("master")) };
    }

    [Fact]
    public void ShouldCreateInstance()
    {
      // act & assert
      command.CreateInstance().Should().BeOfType<GetItemCommand>();
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