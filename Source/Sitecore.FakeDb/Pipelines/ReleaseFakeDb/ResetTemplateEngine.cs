namespace Sitecore.FakeDb.Pipelines.ReleaseFakeDb
{
  public class ResetTemplateEngine
  {
    public void Process(ReleaseDbArgs args)
    {
      args.Db.Database.Engines.TemplateEngine.Reset();
    }
  }
}