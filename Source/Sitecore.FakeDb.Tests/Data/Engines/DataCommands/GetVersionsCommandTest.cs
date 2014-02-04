namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using Sitecore.Collections;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Xunit;

  public class GetVersionsCommandTest
  {
    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new OpenGetVersionsCommand();

      // act & assert
      command.CreateInstance().Should().BeOfType<GetVersionsCommand>();
    }

    [Fact]
    public void ShouldGetVersionCollectionWinthSingleVersopm()
    {
      // arrange
      var command = new OpenGetVersionsCommand();

      // act
      var versionCollection = command.DoExecute();

      // assert
      versionCollection.Should().ContainSingle(v => v.Number == 1);
    }

    private class OpenGetVersionsCommand : GetVersionsCommand
    {
      public new Sitecore.Data.Engines.DataCommands.GetVersionsCommand CreateInstance()
      {
        return base.CreateInstance();
      }

      public new VersionCollection DoExecute()
      {
        return base.DoExecute();
      }
    }
  }
}