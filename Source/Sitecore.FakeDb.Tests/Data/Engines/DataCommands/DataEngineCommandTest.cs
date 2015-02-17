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
    private readonly DataStorage dataStorage = Substitute.For<DataStorage>(Database.GetDatabase("master"));

    [Fact]
    public void ShouldSetDataStorage()
    {
      // arrange
      var command = new DataEngineCommand();

      // act
      command.Initialize(this.dataStorage);

      // assert
      command.DataStorage.Should().Be(this.dataStorage);
    }

    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var command = new DataEngineCommand();
      command.Initialize(this.dataStorage);

      // act
      var newCommand = command.CreateInstance<Sitecore.Data.Engines.DataCommands.GetItemCommand, GetItemCommand>();

      // assert
      newCommand.Should().NotBeNull();
      newCommand.Should().BeAssignableTo<Sitecore.Data.Engines.DataCommands.GetItemCommand>();
      newCommand.Should().BeOfType<GetItemCommand>();
    }
  }
}