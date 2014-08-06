namespace Sitecore.FakeDb.Pipelines.ReleaseFakeDb
{
  using Sitecore.FakeDb.Security.AccessControl;
  using Sitecore.Pipelines;
  using Sitecore.Security.AccessControl;

  public class ReleaseAuthorizationProvider
  {
    public virtual void Process(PipelineArgs args)
    {
      if (AuthorizationManager.Provider is FakeAuthorizationProvider)
      {
        ((FakeAuthorizationProvider)AuthorizationManager.Provider).AccessRulesStorage.Value = null;
      }
    }
  }
}