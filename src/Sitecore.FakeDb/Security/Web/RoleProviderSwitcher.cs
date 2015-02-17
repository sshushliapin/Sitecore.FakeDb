namespace Sitecore.FakeDb.Security.Web
{
  using System.Web.Security;

  public class RoleProviderSwitcher : ThreadLocalProviderSwitcher<RoleProvider>
  {
    public RoleProviderSwitcher(RoleProvider localProvider)
      : base((IThreadLocalProvider<RoleProvider>)Roles.Provider, localProvider)
    {
    }
  }
}