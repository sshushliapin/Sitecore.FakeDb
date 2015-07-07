namespace Sitecore.FakeDb.Security.AccessControl
{
  using Sitecore.Security.AccessControl;

  public class AuthorizationSwitcher : ThreadLocalProviderSwitcher<AuthorizationProvider>
  {
    public AuthorizationSwitcher(AuthorizationProvider localProvider)
      : base((IThreadLocalProvider<AuthorizationProvider>)AuthorizationManager.Provider, localProvider)
    {
    }
  }
}
