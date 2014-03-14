namespace Sitecore.FakeDb.Tests.Configuration
{
  using System.Xml;
  using FluentAssertions;
  using Sitecore.FakeDb.Configuration;
  using Xunit;

  public class DbConfigurationTest
  {
    [Fact]
    public void ShouldPassConfigSectionToSettings()
    {
      // arrange
      var config = new XmlDocument();

      // act
      var configuration = new DbConfiguration(config);

      // assert
      configuration.Settings.ConfigSection.Should().BeEquivalentTo(config);
    }
  }
}