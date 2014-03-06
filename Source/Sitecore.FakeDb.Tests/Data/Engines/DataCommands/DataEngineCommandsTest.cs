namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Xunit;

  public class DataEngineCommandTest
  {
    [Fact]
    public void ShouuldReceiveDataStorage()
    {
      // arrange
      var dataStorage = Substitute.For<DataStorage>();

      // act
      var command = new DataEngineCommand(dataStorage);

      // assert
      command.DataStorage.Should().Be(dataStorage);
    }

    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new DataEngineCommand(Substitute.For<DataStorage>());

      // act
      var newCommand = command.CreateInstance<Sitecore.Data.Engines.DataCommands.GetItemCommand, GetItemCommand>();

      // assert
      newCommand.Should().NotBeNull();
      newCommand.Should().BeAssignableTo<Sitecore.Data.Engines.DataCommands.GetItemCommand>();
      newCommand.Should().BeOfType<GetItemCommand>();
    }
  }
}