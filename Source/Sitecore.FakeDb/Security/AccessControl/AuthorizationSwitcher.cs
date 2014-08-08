namespace Sitecore.FakeDb.Security.AccessControl
{
  using Sitecore.Security.AccessControl;

  // TODO:[Medium] Naming conflict with the default one
  public class AuthorizationSwitcher : ThreadLocalProviderSwitcher<AuthorizationProvider>
  {
    public AuthorizationSwitcher(AuthorizationProvider localProvider)
      : base((IThreadLocalProvider<AuthorizationProvider>)AuthorizationManager.Provider, localProvider)
    {
    }
  }
}
