namespace Sitecore.FakeDb.Pipelines.InitFakeDb
{
  using Sitecore.Pipelines;

  public class InitGlobals
  {
    public void Process(PipelineArgs args)
    {
      Globals.Load();
    }
  }
}