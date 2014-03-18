namespace Sitecore.FakeDb.Tests.Pipelines
{
  using System;
  using System.Xml;
  using FluentAssertions;
  using Sitecore.Configuration;
  using Sitecore.FakeDb.Pipelines;
  using Sitecore.Pipelines;
  using Xunit;

  public class PipelineRunMarkerTests : IDisposable
  {
    [Fact]
    public void ShouldMarkPipelineAsRun()
    {
      // arrange
      var config = new XmlDocument();
      config.LoadXml("<sitecore />");

      var processor = new PipelineRunMarker("myproduct.mypipeline") { Config = config };

      // act
      processor.Process(new PipelineArgs());

      // assert
      config.SelectSingleNode("/sitecore/pipelines/myproduct.mypipeline[@isRun = 'true']").Should().NotBeNull();
    }

    [Fact]
    public void ShouldReturnFactoryConfigurationIfNotSet()
    {
      // arrange
      var processor = new PipelineRunMarker("pipeline");

      // act & assert
      processor.Config.Should().BeEquivalentTo(Factory.GetConfiguration());
    }

    public void Dispose()
    {
      Factory.Reset();
    }
  }
}