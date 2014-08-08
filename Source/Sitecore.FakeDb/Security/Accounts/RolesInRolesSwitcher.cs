namespace Sitecore.FakeDb.Security.Accounts
{
  using Sitecore.Security.Accounts;

  public class RolesInRolesSwitcher : ThreadLocalProviderSwitcher<RolesInRolesProvider>
  {
    public RolesInRolesSwitcher(RolesInRolesProvider localProvider)
      : base((IThreadLocalProvider<RolesInRolesProvider>)RolesInRolesManager.Provider, localProvider)
    {
    }
  }
}