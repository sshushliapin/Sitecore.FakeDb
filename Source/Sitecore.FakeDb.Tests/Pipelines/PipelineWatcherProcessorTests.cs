namespace Sitecore.FakeDb.Tests.Pipelines
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.FakeDb.Data.Engines;
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

    [Fact]
    public void ShouldExecuteDataStorageProcessorIfSet()
    {
      // arrange
      var dataStorage = new DataStorage();
      var processor = Substitute.For<IPipelineProcessor>();
      var args = new PipelineArgs();

      dataStorage.Pipelines.Add("mypipeline", processor);

      var watcherProcessor = new PipelineWatcherProcessor("mypipeline") { DataStorage = dataStorage };

      // act
      watcherProcessor.Process(args);

      // assert
      processor.Received().Process(args);
    }

    [Fact]
    public void ShouldRetrieveDataStorageFromDataProvider()
    {
      // arrange
      using (var db = new Db())
      {
        // act
        var processor = new PipelineWatcherProcessor("mypipeline");

        // assert
        processor.DataStorage.Should().BeSameAs(db.DataStorage);
      }
    }
  }
}