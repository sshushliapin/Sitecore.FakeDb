namespace Sitecore.FakeDb.Tests.Configuration
{
  using FluentAssertions;
  using Sitecore.Configuration;
  using Xunit;

  public class SettingsTest
  {
    [Fact]
    public void CacheShouldBeDisabled()
    {
      Settings.Caching.Enabled.Should().BeFalse();
    } 
  }
}