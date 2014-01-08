namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Xunit;

  public class ResolvePathCommandTest
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new OpenResolvePathCommand();

      // act & assert
      command.CreateInstance().Should().BeOfType<ResolvePathCommand>();
    }

    [Fact]
    public void ShouldResolvePath()
    {
      // arrange
      const string Path = "/sitecore/content";

      var command = new OpenResolvePathCommand { Engine = new DataEngine(Database.GetDatabase("master")) };
      command.Initialize(Path);

      // act
      var id = command.DoExecute();

      // assert
      id.Should().Be(ItemIDs.ContentRoot);
    }

    private class OpenResolvePathCommand : ResolvePathCommand
    {
      public new Sitecore.Data.Engines.DataCommands.ResolvePathCommand CreateInstance()
      {
        return base.CreateInstance();
      }

      public new ID DoExecute()
      {
        return base.DoExecute();
      }
    }
  }
}