namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using Sitecore.Common;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Xunit;

  public class DataEngineCommandTest
  {
    [Theory, DefaultAutoData]
    public void ShouldGetDataStorageFromSwither(DataEngineCommand sut, DataStorageSwitcher switcher)
    {
      sut.DataStorage.Should().Be(Switcher<DataStorage>.CurrentValue);
    }
  }
}