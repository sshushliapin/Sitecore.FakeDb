namespace Sitecore.FakeDb.Tests.Pipelines
{
  using FluentAssertions;
  using Sitecore.Pipelines;
  using Xunit;

  public class CorePipelineFactoryTest
  {
    private const string PipelineName = "mypipeline";

    [Fact]
    public void ShouldGetNullIfNoPipelineConfigured()
    {
      // arrange
      using (new Db())
      {
        // act & assert
        CorePipelineFactory.GetPipeline(PipelineName, string.Empty).Should().BeNull();
      }
    }

    [Fact]
    public void ShouldGetPipelineByName()
    {
      // arrange
      using (var db = new Db())
      {
        db.PipelineWatcher.Expects(PipelineName);

        // act & assert
        CorePipelineFactory.GetPipeline(PipelineName, string.Empty).Should().NotBeNull();
      }
    }
  }
}