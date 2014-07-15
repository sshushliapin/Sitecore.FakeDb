namespace Sitecore.FakeDb.Security.Authentication
{
  using System.Threading;
  using Sitecore.Diagnostics;
  using Sitecore.Security.Accounts;
  using Sitecore.Security.Authentication;

  public class FakeAuthenticationProvider : AuthenticationProvider, IBehavioral<AuthenticationProvider>
  {
    private readonly ThreadLocal<AuthenticationProvider> behavior = new ThreadLocal<AuthenticationProvider>();

    private readonly User defaultVirtualUser = User.FromName(@"default\Virtual", false);

    private readonly User defaultActiveUser = User.FromName(@"default\Anonymous", false);

    private readonly ThreadLocal<User> activeUser = new ThreadLocal<User>();

    public AuthenticationProvider Behavior
    {
      get { return this.behavior.Value; }
      set { this.behavior.Value = value; }
    }

    public override User BuildVirtualUser(string userName, bool isAuthenticated)
    {
      if (this.Behavior != null)
      {
        return this.Behavior.BuildVirtualUser(userName, isAuthenticated);
      }

      return this.defaultVirtualUser;
    }

    public override bool CheckLegacyPassword(User user, string password)
    {
      if (this.Behavior != null)
      {
        return this.Behavior.CheckLegacyPassword(user, password);
      }

      return false;
    }

    public override User GetActiveUser()
    {
      if (this.Behavior != null)
      {
        return this.Behavior.GetActiveUser();
      }

      if (this.activeUser.Value != null)
      {
        return this.activeUser.Value;
      }

      return this.defaultActiveUser;
    }

    public override bool Login(User user)
    {
      if (this.Behavior != null)
      {
        return this.Behavior.Login(user);
      }

      return false;
    }

    public override bool Login(string userName, bool persistent)
    {
      if (this.Behavior != null)
      {
        return this.Behavior.Login(userName, persistent);
      }

      return false;
    }

    public override bool Login(string userName, string password, bool persistent)
    {
      if (this.Behavior != null)
      {
        return this.Behavior.Login(userName, password, persistent);
      }

      return false;
    }

    public override void Logout()
    {
      if (this.Behavior != null)
      {
        this.Behavior.Logout();
      }
    }

    public override void SetActiveUser(string userName)
    {
      if (this.Behavior != null)
      {
        this.Behavior.SetActiveUser(userName);
      }
      else
      {
        this.activeUser.Value = User.FromName(userName, false);
      }
    }

    public override void SetActiveUser(User user)
    {
      if (this.Behavior != null)
      {
        this.Behavior.SetActiveUser(user);
      }
      else
      {
        this.activeUser.Value = user;
      }
    }
  }
}