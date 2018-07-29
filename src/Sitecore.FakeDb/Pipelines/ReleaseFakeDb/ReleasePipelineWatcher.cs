namespace Sitecore.FakeDb.Pipelines.ReleaseFakeDb
{
    public class ReleasePipelineWatcher
    {
        public virtual void Process(ReleaseDbArgs args)
        {
            args.Db.PipelineWatcher.Dispose();
        }
    }
}