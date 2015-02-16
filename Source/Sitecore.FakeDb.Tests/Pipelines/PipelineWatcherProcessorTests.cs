namespace Sitecore.FakeDb.Tests.Pipelines
{
  using FluentAssertions;
  using Sitecore.FakeDb.Pipelines;
  using Sitecore.Pipelines;
  using Xunit;

  public class PipelineWatcherProcessorTests
  {
    private readonly PipelineArgs pipelineArgs = new PipelineArgs();

    [Fact]
    public void ShouldRaisePipelineRunEventIfSubscribed()
    {
      // arrange
      var processor = new PipelineWatcherProcessor("mypipeline");
      processor.MonitorEvents();

      // act
      processor.Process(this.pipelineArgs);

      // assert
      processor.ShouldRaise("PipelineRun")
        .WithSender(processor)
        .WithArgs<PipelineRunEventArgs>(a => a.PipelineName == "mypipeline" && a.PipelineArgs == this.pipelineArgs);
    }

    [Fact]
    public void ShouldNotFirePipelineRunEventIfNotSubscribed()
    {
      // arrange
      var processor = new PipelineWatcherProcessor("mypipeline");

      // act
      processor.Process(new PipelineArgs());
    }
  }
}