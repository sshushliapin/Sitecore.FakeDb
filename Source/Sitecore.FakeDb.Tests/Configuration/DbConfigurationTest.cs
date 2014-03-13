namespace Sitecore.FakeDb.Tests.Configuration
{
  using FluentAssertions;
  using Sitecore.FakeDb.Configuration;
  using Xunit;

  public class DbConfigurationTest
  {
    [Fact]
    public void ShouldSetSettings()
    {
      // arrange
      var configuration = new DbConfiguration();

      // assert
      configuration.Settings.Should().BeOfType<Settings>();
    }
  }
}