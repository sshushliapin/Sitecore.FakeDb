namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Xunit;

  public class DataEngineCommandTest
  {
    private DataStorage dataStorage = Substitute.For<DataStorage>(Database.GetDatabase("master"));

    [Fact]
    public void ShouuldReceiveDataStorage()
    {
      // act
      var command = new DataEngineCommand(dataStorage);

      // assert
      command.DataStorage.Should().Be(dataStorage);
    }

    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new DataEngineCommand(this.dataStorage);

      // act
      var newCommand = command.CreateInstance<Sitecore.Data.Engines.DataCommands.GetItemCommand, GetItemCommand>();

      // assert
      newCommand.Should().NotBeNull();
      newCommand.Should().BeAssignableTo<Sitecore.Data.Engines.DataCommands.GetItemCommand>();
      newCommand.Should().BeOfType<GetItemCommand>();
    }
  }
}