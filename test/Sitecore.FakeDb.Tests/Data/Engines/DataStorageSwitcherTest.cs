namespace Sitecore.FakeDb.Tests.Data.Engines
{
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Common;
  using Sitecore.FakeDb.Data.Engines;
  using Xunit;

  public class DataStorageSwitcherTest
  {
    [Theory, DefaultAutoData]
    public void ShouldSwitchDataStorage([Frozen]DataStorage dataStorage, DataStorageSwitcher sut)
    {
      Assert.Same(dataStorage, DataStorageSwitcher.CurrentValue(dataStorage.Database.Name));
    }
  }
}