namespace Sitecore.FakeDb.Tests.Configuration
{
  using System.Xml;
  using FluentAssertions;
  using Sitecore.FakeDb.Configuration;
  using Xunit;

  public class SettingsTest
  {
    [Fact]
    public void ShouldReadSettingsFromSitecoreSection()
    {
      // arrange
      var config = new XmlDocument();
      config.LoadXml("<sitecore><settings><setting name=\"MySetting\" value=\"MyValue\" /></settings></sitecore>");

      // act
      var settings = new Settings(config);

      // assert
      settings["MySetting"].Should().Be("MyValue");
    }

    [Fact]
    public void ShouldSetSettingsInSitecoreSection()
    {
      // arrange
      var config = new XmlDocument();
      config.LoadXml("<sitecore><settings><setting name=\"MySetting\" value=\"MyValue\" /></settings></sitecore>");

      // act
      var settings = new Settings(config);
      settings["MySetting"] = "MyNewValue";

      // assert
      config.OuterXml.Should().Be("<sitecore><settings><setting name=\"MySetting\" value=\"MyNewValue\" /></settings></sitecore>");
    }

    [Fact]
    public void ShouldAddNewSettingsInSitecoreSection()
    {
      // arrange
      var config = new XmlDocument();
      config.LoadXml("<sitecore><settings></settings></sitecore>");

      // act
      var settings = new Settings(config);
      settings["MySetting"] = "MyValue";

      // assert
      config.OuterXml.Should().Be("<sitecore><settings><setting name=\"MySetting\" value=\"MyValue\" /></settings></sitecore>");
    }
  }
}