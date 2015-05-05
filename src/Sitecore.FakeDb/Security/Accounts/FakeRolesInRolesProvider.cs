namespace Sitecore.FakeDb.Security.Accounts
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using Sitecore.Security.Accounts;
  using Sitecore.Security.Domains;

  public class FakeRolesInRolesProvider : RolesInRolesProvider, IThreadLocalProvider<RolesInRolesProvider>
  {
    private readonly ThreadLocal<RolesInRolesProvider> localProvider = new ThreadLocal<RolesInRolesProvider>();

    private bool disposed;

    public virtual ThreadLocal<RolesInRolesProvider> LocalProvider
    {
      get { return this.localProvider; }
    }

    public override void AddRolesToRoles(IEnumerable<Role> memberRoles, IEnumerable<Role> targetRoles)
    {
      if (this.IsLocalProviderSet())
      {
        this.LocalProvider.Value.AddRolesToRoles(memberRoles, targetRoles);
      }
    }

    public override IEnumerable<Role> FindRolesInRole(Role targetRole, string roleNameToMatch, bool includeIndirectMembership)
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.FindRolesInRole(targetRole, roleNameToMatch, includeIndirectMembership) : Enumerable.Empty<Role>();
    }

    protected override IEnumerable<Role> FindRolesInRole(string targetRoleName, string roleNameToMatch)
    {
      return Enumerable.Empty<Role>();
    }

    // TODO:[Minor] Shouldn't it be IEnumerable<Users>? Register CMS issue.
    public override IEnumerable<Role> FindUsersInRole(Role targetRole, string userNameToMatch, bool includeIndirectMembership)
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.FindUsersInRole(targetRole, userNameToMatch, includeIndirectMembership) : Enumerable.Empty<Role>();
    }

    public override IEnumerable<Role> GetAllRoles(bool includeSystemRoles)
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.GetAllRoles(includeSystemRoles) : Enumerable.Empty<Role>();
    }

    public override Role GetCreatorOwnerRole()
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.GetCreatorOwnerRole() : base.GetCreatorOwnerRole();
    }

    public override Role GetEveryoneRole()
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.GetEveryoneRole() : base.GetEveryoneRole();
    }

    public override Role GetEveryoneRole(Domain domain)
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.GetEveryoneRole(domain) : base.GetEveryoneRole(domain);
    }

    public override IEnumerable<Role> GetEveryoneRoles()
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.GetEveryoneRoles() : Enumerable.Empty<Role>();
    }

    public override IEnumerable<Role> GetGlobalRoles()
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.GetGlobalRoles() : Enumerable.Empty<Role>();
    }

    public override IEnumerable<Account> GetRoleMembers(Role role, bool includeIndirectMembership)
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.GetRoleMembers(role, includeIndirectMembership) : Enumerable.Empty<Account>();
    }

    protected override IEnumerable<Role> GetRolesForRole(string memberRoleName)
    {
      return Enumerable.Empty<Role>();
    }

    public override IEnumerable<Role> GetRolesForRole(Role memberRole, bool includeIndirectMembership)
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.GetRolesForRole(memberRole, includeIndirectMembership) : Enumerable.Empty<Role>();
    }

    public override IEnumerable<Role> GetRolesForUser(User user, bool includeIndirectMembership)
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.GetRolesForUser(user, includeIndirectMembership) : Enumerable.Empty<Role>();
    }

    protected override IEnumerable<Role> GetRolesInRole(string targetRoleName)
    {
      return Enumerable.Empty<Role>();
    }

    public override IEnumerable<Role> GetRolesInRole(Role targetRole, bool includeIndirectMembership)
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.GetRolesInRole(targetRole, includeIndirectMembership) : Enumerable.Empty<Role>();
    }

    public override IEnumerable<Role> GetSystemRoles()
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.GetSystemRoles() : Enumerable.Empty<Role>();
    }

    public override IEnumerable<User> GetUsersInRole(Role targetRole, bool includeIndirectMembership)
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.GetUsersInRole(targetRole, includeIndirectMembership) : Enumerable.Empty<User>();
    }

    public override bool IsCreatorOwnerRole(string accountName)
    {
      return this.IsLocalProviderSet() && this.LocalProvider.Value.IsCreatorOwnerRole(accountName);
    }

    public override bool IsEveryoneRole(string accountName)
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.IsEveryoneRole(accountName) : base.IsEveryoneRole(accountName);
    }

    public override bool IsEveryoneRole(string accountName, Domain domain)
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.IsEveryoneRole(accountName, domain) : base.IsEveryoneRole(accountName, domain);
    }

    public override bool IsGlobalRole(Role role)
    {
      return this.IsLocalProviderSet() && this.LocalProvider.Value.IsGlobalRole(role);
    }

    public virtual bool IsLocalProviderSet()
    {
      return this.localProvider.Value != null;
    }

    protected override bool IsRoleInRole(string memberRoleName, string targetRoleName)
    {
      return false;
    }

    public override bool IsRoleInRole(Role memberRole, Role targetRole, bool includeIndirectMembership)
    {
      return this.IsLocalProviderSet() && this.LocalProvider.Value.IsRoleInRole(memberRole, targetRole, includeIndirectMembership);
    }

    public override bool IsSystemRole(string accountName)
    {
      return this.IsLocalProviderSet() && this.LocalProvider.Value.IsSystemRole(accountName);
    }

    public override bool IsUserInRole(User user, Role targetRole, bool includeIndirectMemberships)
    {
      return this.IsLocalProviderSet() && this.LocalProvider.Value.IsUserInRole(user, targetRole, includeIndirectMemberships);
    }

    public override void RemoveRoleRelations(string roleName)
    {
      if (this.IsLocalProviderSet())
      {
        this.LocalProvider.Value.RemoveRoleRelations(roleName);
      }
    }

    public override void RemoveRolesFromRoles(IEnumerable<Role> memberRoles, IEnumerable<Role> targetRoles)
    {
      if (this.IsLocalProviderSet())
      {
        this.LocalProvider.Value.RemoveRolesFromRoles(memberRoles, targetRoles);
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
      {
        return;
      }

      if (!disposing)
      {
        return;
      }

      this.localProvider.Dispose();

      this.disposed = true;
    }
  }
}