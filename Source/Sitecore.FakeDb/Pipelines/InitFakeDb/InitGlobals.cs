namespace Sitecore.FakeDb.Pipelines.InitFakeDb
{
  public class InitGlobals: InitDbProcessor
  {
    public override void Process(InitDbArgs args)
    {
      Sitecore.Globals.Load();
    }
  }
}
