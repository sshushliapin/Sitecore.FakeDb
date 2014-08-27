namespace Sitecore.FakeDb.Pipelines.ReleaseFakeDb
{
  using Sitecore.Pipelines;

  public class ReleasePipelineWatcher
  {
    public virtual void Process(DbArgs args)
    {
      args.Db.PipelineWatcher.Dispose();
      CorePipelineFactory.ClearCache();
    }
  }
}