namespace Sitecore.FakeDb.Pipelines.InitFakeDb
{
  using Sitecore.FakeDb.Data.Engines;

  public abstract class InitDbProcessor
  {
    public abstract void Process(InitDbArgs args);

    protected virtual void SetDataStorage(object obj, DataStorage dataStorage)
    {
      var rds = obj as IRequireDataStorage;
      if (rds != null)
      {
        rds.SetDataStorage(dataStorage);
      }
      else
      {
        // we should know if it happens
        throw new System.Exception(obj.GetType().FullName);
      }
    }
  }
}