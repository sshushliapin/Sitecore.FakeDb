namespace Sitecore.FakeDb.Pipelines.ReleaseFakeDb
{
  public class ResetDataProviders
  {
    public void Process(ReleaseDbArgs args)
    {
      foreach (var provider in args.Db.Database.GetDataProviders())
      {
        var rds = provider as IRequireDataStorage;
        if (rds == null)
        {
          continue;
        }

        rds.SetDataStorage(null);
      }
    }
  }
}