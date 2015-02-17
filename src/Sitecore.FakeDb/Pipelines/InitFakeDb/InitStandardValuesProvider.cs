namespace Sitecore.FakeDb.Pipelines.InitFakeDb
{
  using Sitecore.Data;

  public class InitStandardValuesProvider : InitDbProcessor
  {
    public override void Process(InitDbArgs args)
    {
      this.SetDataStorage(StandardValuesManager.Provider, args.DataStorage);
    }
  }
}