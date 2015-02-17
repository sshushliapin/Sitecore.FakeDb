namespace Sitecore.FakeDb.Pipelines.ReleaseFakeDb
{
  using Sitecore.Diagnostics;
  using Sitecore.Pipelines;

  public class ReleaseDbArgs : PipelineArgs
  {
    private readonly Db db;

    public ReleaseDbArgs(Db db)
    {
      Assert.ArgumentNotNull(db, "db");

      this.db = db;
    }

    public Db Db
    {
      get { return this.db; }
    }
  }
}