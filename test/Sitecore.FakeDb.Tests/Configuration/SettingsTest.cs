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

    [Theory]
    [InlineData("<sitecore></sitecore>")]
    [InlineData("<sitecore><settings></settings></sitecore>")]
    [InlineData("<sitecore><settings><setting name=\"MySetting\" /></settings></sitecore>")]
    public void ShouldReturnEmptyStringByDefault(string xml)
    {
      // arrange
      var config = new XmlDocument();
      config.LoadXml(xml);

      // act
      var settings = new Settings(config);

      // assert
      settings["MySetting"].Should().BeEmpty();
    }

    [Theory]
    [InlineData("<sitecore></sitecore>")]
    [InlineData("<sitecore><settings></settings></sitecore>")]
    [InlineData("<sitecore><settings><setting name=\"MySetting\" /></settings></sitecore>")]
    [InlineData("<sitecore><settings><setting name=\"MySetting\" value=\"MyValue\" /></settings></sitecore>")]
    public void ShouldSetSetting(string xml)
    {
      // arrange
      var config = new XmlDocument();
      config.LoadXml(xml);

      // act
      var settings = new Settings(config);
      settings["MySetting"] = "MyNewValue";

      // assert
      config.OuterXml.Should().Be("<sitecore><settings><setting name=\"MySetting\" value=\"MyNewValue\" /></settings></sitecore>");
    }
  }
}