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
      var config = CreateSimpleConfig();
      var watcher = new PipelineWatcher(config);

      // act
      watcher.Expects("mypipeline");

      // assert
      config.SelectSingleNode("/sitecore/pipelines/mypipeline").Should().NotBeNull();
    }

    [Fact]
    public void ShouldRegisterPipelineRunMarkerProcessor()
    {
      // arrange
      var config = Factory.GetConfiguration();
      var watcher = new PipelineWatcher(config);

      // act
      watcher.Expects("mypipeline");

      // assert
      var processor = (PipelineWatcherProcessor)Factory.CreateObject("/sitecore/pipelines/mypipeline/processor", true);
      processor.Should().NotBeNull();
      processor.PipelineName.Should().Be("mypipeline");
    }

    [Fact]
    public void ShouldEnsurePipelineIsCalled()
    {
      // arrange
      var config = Factory.GetConfiguration();
      var watcher = new PipelineWatcher(config);
      watcher.Expects("mypipeline");

      // act
      CorePipeline.Run("mypipeline", new PipelineArgs());

      // assert
      watcher.EnsureExpectations();
    }

    [Fact]
    public void ShouldThrowExceptionIfNoExpectedPipelineCallReceived()
    {
      // arrange
      var config = Factory.GetConfiguration();
      var watcher = new PipelineWatcher(config);
      watcher.Expects("mypipeline");

      // act
      Action action = watcher.EnsureExpectations;

      // assert
      action.ShouldThrow<InvalidOperationException>().WithMessage("Expected to receive a pipeline call matching (pipelineName == \"mypipeline\"). Actually received no matching calls.");
    }

    [Fact]
    public void ShouldSubscribeAndUnsubscribeFromPipelineRunEvent()
    {
      // arrange
      var config = CreateSimpleConfig();
      var watcher = new ThrowablePipelineWatcher();

      var processor = new PipelineWatcherProcessor("pipeline");

      // act
      watcher.Dispose();

      // assert
      Assert.DoesNotThrow(() => processor.Process(new PipelineArgs()));
    }

    public void Dispose()
    {
      Factory.Reset();
    }

    private static XmlDocument CreateSimpleConfig()
    {
      var config = new XmlDocument();
      config.LoadXml("<sitecore />");

      return config;
    }

    private class ThrowablePipelineWatcher : PipelineWatcher
    {
      public ThrowablePipelineWatcher()
        : base(CreateSimpleConfig())
      {
      }

      protected override void OnPipelineRun(PipelineRunEventArgs e)
      {
        throw new NotSupportedException();
      }
    }
  }
}