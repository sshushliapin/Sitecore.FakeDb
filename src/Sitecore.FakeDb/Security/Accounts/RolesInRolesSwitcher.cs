namespace Sitecore.FakeDb.Security.Accounts
{
  using System;
  using Sitecore.Security.Accounts;

  [Obsolete("Starting from Sitecore 8.2, the " +
            "Sitecore.Security.Accounts.RolesInRolesProvider " +
            "class is marked as obsolete and will be removed " +
            "in the next major release. Please use new abstract " +
            "type Sitecore.Abstractions.BaseRolesInRolesManager " +
            "from the Sitecore.Kernel assembly.")]
  public class RolesInRolesSwitcher : ThreadLocalProviderSwitcher<RolesInRolesProvider>
  {
    public RolesInRolesSwitcher(RolesInRolesProvider localProvider)
      : base((IThreadLocalProvider<RolesInRolesProvider>)RolesInRolesManager.Provider, localProvider)
    {
    }
  }
}