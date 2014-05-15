namespace Sitecore.FakeDb.Pipelines.ReleaseFakeDb
{
  public class ReleasePipelineWatcher
  {
    public virtual void Process(DbArgs args)
    {
      args.Db.PipelineWatcher.Dispose();
    }
  }
}