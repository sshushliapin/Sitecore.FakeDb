namespace Sitecore.FakeDb.Tests.Data.Engines
{
    using Sitecore.FakeDb.Data.Engines;
    using Xunit;

    public class DataStorageSwitcherTest
    {
        [Theory, DefaultAutoData]
        public void ShouldSwitchDataStorage(
            DataStorage expected)
        {
            using (new DataStorageSwitcher(expected))
            {
                var actual = DataStorageSwitcher.CurrentValue(expected.Database.Name);
                Assert.Same(expected, actual);
            }
        }
    }
}