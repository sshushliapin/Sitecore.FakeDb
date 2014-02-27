namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Engines;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Xunit;
  using Xunit.Extensions;

  public class ResolvePathCommandTest : CommandTestBase
  {
    private readonly OpenResolvePathCommand command;

    public ResolvePathCommandTest()
    {
      this.command = new OpenResolvePathCommand(this.dataStorage) { Engine = new DataEngine(this.database) };
    }

    [Fact]
    public void ShouldCreateInstance()
    {
      // act & assert
      this.command.CreateInstance().Should().BeOfType<ResolvePathCommand>();
    }

    [Theory]
    [InlineData("/sitecore/content/home")]
    [InlineData("/Sitecore/Content/Home")]
    public void ShouldResolvePath(string path)
    {
      // arrange
      var itemId = ID.NewID;
      var item = new DbItem("home", itemId) { FullPath = "/sitecore/content/home" };

      this.dataStorage.FakeItems.Add(itemId, item);

      this.command.Initialize(path);

      // act
      var id = this.command.DoExecute();

      // assert
      id.Should().Be(itemId);
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
      public OpenResolvePathCommand(DataStorage dataStorage)
        : base(dataStorage)
      {
      }

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