namespace Sitecore.FakeDb.Pipelines.InitFakeDb
{
  public class InitDataProviders : InitDbProcessor
  {
    public override void Process(InitDbArgs args)
    {
      foreach (var provider in args.Database.GetDataProviders())
      {
        this.SetDataStorage(provider, args.DataStorage);
      }
    }
  }
}