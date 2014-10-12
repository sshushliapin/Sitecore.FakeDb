namespace Sitecore.FakeDb.Tests.Pipelines
{
  using NSubstitute;
  using Sitecore.FakeDb.Pipelines;
  using Sitecore.Pipelines;
  using Xunit;

  public class CorePipelineTest
  {
    [Fact]
    public void ShouldCallRegisteredPipeline()
    {
      // arrange
      using (var db = new Db())
      {
        var processor = Substitute.For<IPipelineProcessor>();
        var args = new PipelineArgs();

        db.PipelineWatcher.Register("mypipeline", processor);

        // act
        CorePipeline.Run("mypipeline", args);

        // assert
        processor.Received().Process(args);
      }
    }

    [Fact]
    public void ShouldCallLatestRegisteredPipeline()
    {
      // arrange
      using (var db = new Db())
      {
        var processor1 = Substitute.For<IPipelineProcessor>();
        var processor2 = Substitute.For<IPipelineProcessor>();
        var args = new PipelineArgs();

        db.PipelineWatcher.Register("mypipeline", processor1);
        db.PipelineWatcher.Register("mypipeline", processor2);

        // act
        CorePipeline.Run("mypipeline", args);

        // assert
        processor1.DidNotReceiveWithAnyArgs().Process(args);
        processor2.Received().Process(args);
      }
    }
  }
}