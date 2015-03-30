namespace Sitecore.FakeDb.Security.Web
{
  using System;
  using System.Threading;
  using System.Web.Security;

  public class FakeRoleProvider : RoleProvider, IThreadLocalProvider<RoleProvider>
  {
    private readonly ThreadLocal<RoleProvider> localProvider = new ThreadLocal<RoleProvider>();

    private readonly string[] emptyRoles = { };

    private readonly string[] emptyUsers = { };

    private bool disposed;

    public override string ApplicationName { get; set; }

    public virtual ThreadLocal<RoleProvider> LocalProvider
    {
      get { return this.localProvider; }
    }

    public override void AddUsersToRoles(string[] usernames, string[] roleNames)
    {
    }

    public override void CreateRole(string roleName)
    {
      if (this.IsLocalProviderSet())
      {
        this.localProvider.Value.CreateRole(roleName);
      }
    }

    public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
    {
      return this.IsLocalProviderSet() && this.localProvider.Value.DeleteRole(roleName, throwOnPopulatedRole);
    }

    public override string[] FindUsersInRole(string roleName, string usernameToMatch)
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.FindUsersInRole(roleName, usernameToMatch) : this.emptyUsers;
    }

    public override string[] GetAllRoles()
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.GetAllRoles() : this.emptyRoles;
    }

    public override string[] GetRolesForUser(string username)
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.GetRolesForUser(username) : this.emptyRoles;
    }

    public override string[] GetUsersInRole(string roleName)
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.GetUsersInRole(roleName) : this.emptyUsers;
    }

    public virtual bool IsLocalProviderSet()
    {
      return this.localProvider.Value != null;
    }

    public override bool IsUserInRole(string username, string roleName)
    {
      return this.IsLocalProviderSet() && this.localProvider.Value.IsUserInRole(username, roleName);
    }

    public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
    {
      if (this.IsLocalProviderSet())
      {
        this.localProvider.Value.RemoveUsersFromRoles(usernames, roleNames);
      }
    }

    public override bool RoleExists(string roleName)
    {
      return this.IsLocalProviderSet() && this.localProvider.Value.RoleExists(roleName);
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