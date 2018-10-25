namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
    using FluentAssertions;
    using Sitecore.FakeDb.Data.Engines;
    using Sitecore.FakeDb.Data.Engines.DataCommands;
    using Xunit;

    public class DataEngineCommandTest
    {
        [Theory, DefaultAutoData]
        public void ShouldGetDataStorageFromSwitcher(
            DataEngineCommand sut,
            DataStorage dataStorage)
        {
            using (new DataStorageSwitcher(dataStorage))
            {
                var databaseName = sut.DataStorage.Database.Name;
                sut.DataStorage.Should().Be(DataStorageSwitcher.CurrentValue(databaseName));
            }
        }
    }
}