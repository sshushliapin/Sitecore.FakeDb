namespace Sitecore.FakeDb.Tests.Pipelines
{
  using System;
  using System.Xml;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Configuration;
  using Sitecore.FakeDb.Pipelines;
  using Sitecore.Pipelines;
  using Xunit;
  using Xunit.Extensions;

  public class PipelineWatcherTest : IDisposable
  {
    private readonly PipelineWatcher watcher;

    public PipelineWatcherTest()
    {
      this.watcher = new PipelineWatcher(Factory.GetConfiguration());
    }

    [Fact]
    public void ShouldRegisterPipelineExpectedToBeCalled()
    {
      // arrange
      var config = CreateSimpleConfig();
      using (var w = new PipelineWatcher(config))
      {
        // act
        w.Expects("mypipeline");

        // assert
        config.SelectSingleNode("/sitecore/pipelines/mypipeline").Should().NotBeNull();
      }
    }

    [Fact]
    public void ShouldRegisterPipelineRunMarkerProcessor()
    {
      // act
      this.watcher.Expects("mypipeline");

      // assert
      var processor = (PipelineWatcherProcessor)Factory.CreateObject("/sitecore/pipelines/mypipeline/processor", true);
      processor.Should().NotBeNull();
      processor.PipelineName.Should().Be("mypipeline");
    }

    [Fact]
    public void ShouldEnsurePipelineWithSpecificNameIsCalled()
    {
      // arrange
      this.watcher.Expects("mypipeline");

      // act
      CorePipeline.Run("mypipeline", new PipelineArgs());

      // assert
      this.watcher.EnsureExpectations();
    }

    [Fact]
    public void ShouldEnsurePipelineWithArgsIsCalled()
    {
      // arrange
      var args = new PipelineArgs();
      this.watcher.Expects("mypipeline", args);

      // act
      CorePipeline.Run("mypipeline", args);

      // assert
      this.watcher.EnsureExpectations();
    }

    [Fact]
    public void ShouldEnsurePipelineWithSpecificArgsIsCalled()
    {
      // arrange
      var args = new MyPipelineArgs { Id = 1 };
      this.watcher.Expects("mypipeline", a => ((MyPipelineArgs)a).Id == 1);

      // act
      CorePipeline.Run("mypipeline", args);

      // assert
      this.watcher.EnsureExpectations();
    }

    [Fact]
    public void ShouldThrowExceptionIfNoExpectedPipelineCallReceived()
    {
      // arrange
      this.watcher.Expects("mypipeline");

      // act
      Action action = this.watcher.EnsureExpectations;

      // assert
      action.ShouldThrow<InvalidOperationException>().WithMessage("Expected to receive a pipeline call matching (pipelineName == \"mypipeline\"). Actually received no matching calls.");
    }

    [Theory]
    [InlineData("pipeline1", "Expected to receive a pipeline call matching (pipelineName == \"pipeline2\"). Actually received no matching calls.")]
    [InlineData("pipeline2", "Expected to receive a pipeline call matching (pipelineName == \"pipeline1\"). Actually received no matching calls.")]
    public void ShouldThrowExceptionIfOnePipelineFromExpectedListIsNotCalled(string pipelineName, string message)
    {
      // arrange
      this.watcher.Expects("pipeline1");
      this.watcher.Expects("pipeline2");

      CorePipeline.Run(pipelineName, new PipelineArgs());

      // act
      Action action = this.watcher.EnsureExpectations;

      // assert
      action.ShouldThrow<InvalidOperationException>().WithMessage(message);
    }

    [Fact]
    public void ShouldThrowExceptionIfNoExpectedPipelineCallWithArgsReceived()
    {
      // arrange
      var expectedArgs = new PipelineArgs();
      var someArgs = new PipelineArgs();

      this.watcher.Expects("mypipeline", expectedArgs);

      CorePipeline.Run("mypipeline", someArgs);

      // act
      Action action = this.watcher.EnsureExpectations;

      // assert
      action.ShouldThrow<InvalidOperationException>().WithMessage("Expected to receive a pipeline call matching (pipelineArgs). Actually received no matching calls.");
    }

    [Fact]
    public void ShouldThrowExceptionIfNoExpectedPipelineCallWithSpecificArgsReceived()
    {
      // arrange
      var args = new MyPipelineArgs { Id = 1 };
      this.watcher.Expects("mypipeline", a => ((MyPipelineArgs)a).Id == 2);

      CorePipeline.Run("mypipeline", args);

      // act
      Action action = this.watcher.EnsureExpectations;

      // assert
      action.ShouldThrow<InvalidOperationException>().WithMessage("Expected to receive a pipeline call matching (pipelineArgs). Actually received no matching calls.");
    }

    [Fact]
    public void ShouldSubscribeAndUnsubscribeFromPipelineRunEvent()
    {
      // arrange
      var w = new ThrowablePipelineWatcher();
      var processor = new PipelineWatcherProcessor("pipeline");

      // act
      w.Dispose();

      // assert
      processor.Process(new PipelineArgs());
    }

    [Fact]
    public void ShouldGoToThenBlockIfWhenIsDone()
    {
      // arrange
      var args = new PipelineArgs();
      args.CustomData["CanSet"] = true;

      this.watcher
        .WhenCall("pipeline")
        .WithArgs(a => (bool)a.CustomData["CanSet"])
        .Then(a => a.CustomData["Value"] = "1");

      // act
      CorePipeline.Run("pipeline", args);

      // assert
      args.CustomData["Value"].Should().Be("1");
    }

    [Fact]
    public void ShouldNotGoToThenBlockIfWhenIsNotDone()
    {
      // arrange
      var args = new PipelineArgs();
      args.CustomData["CanSet"] = false;

      this.watcher
        .WhenCall("pipeline")
        .WithArgs(a => (bool)a.CustomData["CanSet"])
        .Then(a => a.CustomData["Value"] = "1");

      // act
      CorePipeline.Run("pipeline", args);

      // assert
      args.CustomData["Value"].Should().NotBe("1");
    }

    [Fact]
    public void ShouldCallAndCheclListOfPipelinesWithArgs()
    {
      // arrange
      var args1 = new PipelineArgs();
      args1.CustomData["key1"] = "value1";

      var args2 = new PipelineArgs();
      args2.CustomData["key2"] = "value2";

      this.watcher
        .WhenCall("pipeline1")
        .WithArgs(a => a.CustomData.ContainsKey("key1"))
        .Then(a => a.CustomData["key1"] = "new value1");

      this.watcher
        .WhenCall("pipeline2")
        .WithArgs(a => a.CustomData.ContainsKey("key2"))
        .Then(a => a.CustomData["key2"] = "new value2");

      // act
      CorePipeline.Run("pipeline1", args1);
      CorePipeline.Run("pipeline2", args2);

      // assert
      args1.CustomData["key1"].Should().Be("new value1");
      args2.CustomData["key2"].Should().Be("new value2");
    }

    [Fact]
    public void ShouldCallThenBlockIfWithArgsIsMissing()
    {
      // arrange
      var args = new PipelineArgs();
      args.CustomData["CanSet"] = true;

      this.watcher
        .WhenCall("pipeline")
        .Then(a => a.CustomData["Value"] = "1");

      // act
      CorePipeline.Run("pipeline", args);

      // assert
      args.CustomData["Value"].Should().Be("1");
    }

    [Fact]
    public void ShouldRegisterProcessorInConfig()
    {
      // arrange
      var processor = Substitute.For<IPipelineProcessor>();

      // act
      this.watcher.Register("mypipeline", processor);

      // assert
      this.watcher.ConfigSection.SelectSingleNode("/sitecore/pipelines/mypipeline").Should().NotBeNull();
    }

    [Fact]
    public void ShouldExecuteRegisteredProcessor()
    {
      // arrange
      var processor = Substitute.For<IPipelineProcessor>();
      var args = new PipelineArgs();

      this.watcher.Register("mypipeline", processor);

      var watcherProcessor = new PipelineWatcherProcessor("mypipeline");

      // act
      watcherProcessor.Process(args);

      // assert
      processor.Received().Process(args);
    }

    public void Dispose()
    {
      this.watcher.Dispose();
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

    private class MyPipelineArgs : PipelineArgs
    {
      public int Id { get; set; }
    }
  }
}