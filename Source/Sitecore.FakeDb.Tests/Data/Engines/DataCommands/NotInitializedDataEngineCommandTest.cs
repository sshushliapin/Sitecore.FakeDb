namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using System;
  using FluentAssertions;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Xunit;

  public class NotInitializedDataEngineCommandTest
  {
    [Fact]
    public void ShouldThrowExceptionWhenCreatingInstance()
    {
      // arrange
      var command = DataEngineCommand.NotInitialized;

      // act
      Action action = () => command.CreateInstance<Sitecore.Data.Engines.DataCommands.GetItemCommand, GetItemCommand>();

      // assert
      action.ShouldThrow<InvalidOperationException>().WithMessage("Sitecore.FakeDb.Db instance has not been initialized.");
    }

    [Fact]
    public void ShouldThrowExceptionWhenGettinfDataStorage()
    {
      // arrange
      var command = DataEngineCommand.NotInitialized;

      // act & assert
      Assert.Throws<InvalidOperationException>(() => command.DataStorage).Message.Should().Be("Sitecore.FakeDb.Db instance has not been initialized.");
    }
  }
}