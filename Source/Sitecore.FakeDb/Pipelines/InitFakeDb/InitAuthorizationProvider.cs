namespace Sitecore.FakeDb.Pipelines.InitFakeDb
{
  using Sitecore.Security.AccessControl;
  
  public class InitAuthorizationProvider : InitDbProcessor
  {
    public override void Process(InitDbArgs args)
    {
      this.SetDataStorage(AuthorizationManager.Provider, args.DataStorage);
    }
  }
}