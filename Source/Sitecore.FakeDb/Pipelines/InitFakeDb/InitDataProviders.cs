namespace Sitecore.FakeDb.Pipelines.InitFakeDb
{
  public class InitDataProviders : InitDbProcessor
  {
    public override void Process(InitDbArgs args)
    {
      foreach (var dataProvider in args.Database.GetDataProviders())
      {
        var requireDataStorageProvider = dataProvider as IRequireDataStorage;
        if (requireDataStorageProvider != null)
        {
          requireDataStorageProvider.SetDataStorage(args.DataStorage);
        }
      }
    }
  }
}