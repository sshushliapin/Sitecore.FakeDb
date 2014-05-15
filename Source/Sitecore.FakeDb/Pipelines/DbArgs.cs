namespace Sitecore.FakeDb.Pipelines
{
  using Sitecore.Diagnostics;
  using Sitecore.Pipelines;

  public class DbArgs : PipelineArgs
  {
    private readonly Db db;

    public DbArgs(Db db)
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