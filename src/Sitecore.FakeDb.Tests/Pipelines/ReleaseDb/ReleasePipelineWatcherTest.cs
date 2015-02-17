namespace Sitecore.FakeDb.Tests.Pipelines.ReleaseDb
{
  using System;
  using System.Xml;
  using NSubstitute;
  using Sitecore.FakeDb.Pipelines;
  using Sitecore.FakeDb.Pipelines.ReleaseFakeDb;
  using Xunit;

  public class ReleasePipelineWatcherTest
  {
    [Fact]
    public void ShoudDisposePipelineWatcher()
    {
      // arrange
      var watcher = Substitute.For<PipelineWatcher, IDisposable>(new XmlDocument());
      using (var db = new Db(watcher))
      {
        var processor = new ReleasePipelineWatcher();

        // act
        processor.Process(new ReleaseDbArgs(db));

        // assert
        watcher.Received().Dispose();
      }
    }
  }
}