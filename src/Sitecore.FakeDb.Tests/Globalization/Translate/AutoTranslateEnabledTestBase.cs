namespace Sitecore.FakeDb.Tests.Globalization.Translate
{
  using System;

  public abstract class AutoTranslateEnabledTestBase : IDisposable
  {
    private readonly Db db;

    protected AutoTranslateEnabledTestBase()
    {
      this.db = new Db();
      this.db.Configuration.Settings["Sitecore.FakeDb.AutoTranslate"] = "true";
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