namespace Sitecore.FakeDb.Security.Web
{
  using System.Web.Security;

  public class FakeRoleProvider : RoleProvider, IBehavioral<RoleProvider>
  {
    private readonly string[] emptyRoles = { };

    private readonly string[] emptyUsers = { };

    public override string ApplicationName { get; set; }

    public virtual RoleProvider Behavior { get; set; }

    public override void AddUsersToRoles(string[] usernames, string[] roleNames)
    {
    }

    public override void CreateRole(string roleName)
    {
    }

    public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
    {
      return false;
    }

    public override string[] FindUsersInRole(string roleName, string usernameToMatch)
    {
      return this.emptyUsers;
    }

    public override string[] GetAllRoles()
    {
      return this.emptyRoles;
    }

    public override string[] GetRolesForUser(string username)
    {
      return this.emptyRoles;
    }

    public override string[] GetUsersInRole(string roleName)
    {
      return this.emptyUsers;
    }

    public override bool IsUserInRole(string username, string roleName)
    {
      return false;
    }

    public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
    {
    }

    public override bool RoleExists(string roleName)
    {
      return false;
    }
  }
}