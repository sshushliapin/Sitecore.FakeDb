namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.FakeDb.Data;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Xunit;

  public class ResolvePathCommandTest
  {
    private readonly OpenResolvePathCommand command;

    public ResolvePathCommandTest()
    {
      this.command = new OpenResolvePathCommand { Engine = new DataEngine(new FakeDatabase("master")) };
    }

    [Fact]
    public void ShouldCreateInstance()
    {
      // act & assert
      command.CreateInstance().Should().BeOfType<ResolvePathCommand>();
    }

    [Fact]
    public void ShouldResolvePath()
    {
      // arrange
      const string Path = "/sitecore/content";
      command.Initialize(Path);

      // act
      var id = command.DoExecute();

      // assert
      id.Should().Be(ItemIDs.ContentRoot);
    }

    [Fact]
    public void ShouldReturnNullIfNoItemFound()
    {
      const string Path = "/sitecore/content/some path";
      this.command.Initialize(Path);

      // act
      var id = this.command.DoExecute();

      // assert
      id.Should().BeNull();
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