namespace Sitecore.FakeDb.Security.AccessControl
{
  using System;
  using Sitecore.Security.AccessControl;

  public class AuthorizationSwitcher : ThreadLocalProviderSwitcher<AuthorizationProvider>
  {
    [Obsolete("Starting from Sitecore 8.2, the " +
              "Sitecore.Security.AccessControl.AuthorizationProvider " +
              "class is marked as obsolete and will be removed " +
              "in the next major release. Please use new abstract " +
              "type Sitecore.Abstractions.BaseAuthorizationManager " +
              "from the Sitecore.Kernel assembly.")]
    public AuthorizationSwitcher(AuthorizationProvider localProvider)
      : base((IThreadLocalProvider<AuthorizationProvider>)AuthorizationManager.Provider, localProvider)
    {
    }
  }
}
