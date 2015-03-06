namespace Sitecore.FakeDb.Tests.Globalization.Translate
{
  using System;

  public abstract class AutoTranslateTestBase : IDisposable
  {
    private readonly Db db;

    protected AutoTranslateTestBase()
    {
      this.db = new Db();
    }

    protected Db Db
    {
      get { return this.db; }
    }

    public void Dispose()
    {
      this.db.Dispose();
    }
  }
}