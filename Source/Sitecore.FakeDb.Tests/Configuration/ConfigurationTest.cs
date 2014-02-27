namespace Sitecore.FakeDb.Tests.Configuration
{
  using FluentAssertions;
  using Sitecore.Configuration;
  using Sitecore.FakeDb.Data.DataProviders;
  using Sitecore.FakeDb.Data.Engines;
  using Xunit;

  public class ConfigurationTest
  {
    [Fact]
    public void ShouldGetDataStorage()
    {
      Factory.CreateObject("dataStorage", true).Should().BeOfType<DataStorage>();
    }

    [Fact]
    public void ShouldGetDataStorageAsSingleton()
    {
      Factory.CreateObject("dataStorage", true).Should().BeSameAs(Factory.CreateObject("dataStorage", true));
    }

    [Fact]
    public void ShouldSetDataStorageIntoProvider()
    {
      var provider = (FakeDataProvider)Factory.CreateObject("dataProviders/main", true);
      provider.DataStorage.Should().NotBeNull();
    }

    [Fact]
    public void CacheShouldBeDisabled()
    {
      Settings.Caching.Enabled.Should().BeFalse();
    }
  }
}