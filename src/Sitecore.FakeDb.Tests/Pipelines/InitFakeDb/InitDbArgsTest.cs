namespace Sitecore.FakeDb.Tests.Pipelines.InitFakeDb
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Pipelines.InitFakeDb;
  using Xunit;

  public class InitDbArgsTest
  {
    [Fact]
    public void ShouldSetDatabaseAndDataStorage()
    {
      // arrange
      var database = Database.GetDatabase("master");
      var dataStorage = Substitute.For<DataStorage>(database);

      // act
      var args = new InitDbArgs(database, dataStorage);

      // assert
      args.Database.Should().Be(database);
      args.DataStorage.Should().Be(dataStorage);
    }
  }
}