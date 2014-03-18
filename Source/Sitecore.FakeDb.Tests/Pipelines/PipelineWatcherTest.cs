namespace Sitecore.FakeDb.Tests.Pipelines
{
  using System;
  using System.Xml;
  using FluentAssertions;
  using Sitecore.Configuration;
  using Sitecore.FakeDb.Pipelines;
  using Sitecore.Pipelines;
  using Xunit;

  public class PipelineWatcherTest : IDisposable
  {
    [Fact]
    public void ShouldRegisterPipelineExpectedToBeCalled()
    {
      // arrange
      var config = new XmlDocument();
      config.LoadXml("<sitecore></sitecore>");

      var watcher = new PipelineWatcher(config);

      // act
      watcher.Expects("myproduct.mypipeline");

      // assert
      config.SelectSingleNode("/sitecore/pipelines/myproduct.mypipeline").Should().NotBeNull();
    }

    [Fact]
    public void ShouldRegisterPipelineRunMarkerProcessor()
    {
      // arrange
      var config = Factory.GetConfiguration();
      config.LoadXml("<sitecore></sitecore>");

      var watcher = new PipelineWatcher(config);

      // act
      watcher.Expects("myproduct.mypipeline");

      // assert
      var processor = (PipelineRunMarker)Factory.CreateObject("/sitecore/pipelines/myproduct.mypipeline/processor", true);
      processor.Should().NotBeNull();
      processor.PipelineName.Should().Be("myproduct.mypipeline");
    }

    [Fact]
    public void ShouldEnsurePipelineIsCalled()
    {
      // arrange
      var config = Factory.GetConfiguration();
      var watcher = new PipelineWatcher(config);
      watcher.Expects("myproduct.mypipeline");

      // act
      CorePipeline.Run("myproduct.mypipeline", new PipelineArgs());

      // assert
      watcher.EnsureExpectations();
    }

    [Fact]
    public void ShouldThrowExceptionIfNoExpectedPipelineCallReceived()
    {
      // arrange
      var config = Factory.GetConfiguration();
      var watcher = new PipelineWatcher(config);
      watcher.Expects("myproduct.mypipeline");

      // act
      Action action = watcher.EnsureExpectations;

      // assert
      action.ShouldThrow<InvalidOperationException>().WithMessage("Expected to receive a pipeline call matching (pipelineName == \"myproduct.mypipeline\"). Actually received no matching calls.");
    }

    public void Dispose()
    {
      Factory.Reset();
    }
  }
}