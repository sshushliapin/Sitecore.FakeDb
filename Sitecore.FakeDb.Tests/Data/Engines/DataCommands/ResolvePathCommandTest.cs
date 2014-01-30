namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.FakeDb.Data;
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

      var database = Substitute.For<FakeDatabase>("master");
      var command = new OpenResolvePathCommand { Engine = new DataEngine(database) };
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

      var database = Substitute.For<FakeDatabase>("master");
      var command = new OpenResolvePathCommand { Engine = new DataEngine(database) };
      command.Initialize(Path);

      // act
      var id = command.DoExecute();

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