namespace Sitecore.FakeDb.Security.Authentication
{
  using System;
  using System.Threading;
  using Sitecore.Security.Accounts;
  using Sitecore.Security.Authentication;

  public class FakeAuthenticationProvider : AuthenticationProvider, IThreadLocalProvider<AuthenticationProvider>
  {
    private readonly ThreadLocal<AuthenticationProvider> localProvider = new ThreadLocal<AuthenticationProvider>();

    private readonly ThreadLocal<User> activeUser = new ThreadLocal<User>();

    private readonly User defaultActiveUser = User.FromName(@"default\Anonymous", false);

    private readonly User defaultVirtualUser = User.FromName(@"default\Virtual", false);

    private bool disposed;

    public virtual ThreadLocal<AuthenticationProvider> LocalProvider
    {
      get { return this.localProvider; }
    }

    public override User BuildVirtualUser(string userName, bool isAuthenticated)
    {
      return this.IsLocalProviderSet() ? this.localProvider.Value.BuildVirtualUser(userName, isAuthenticated) : this.defaultVirtualUser;
    }

    public override bool CheckLegacyPassword(User user, string password)
    {
      return this.IsLocalProviderSet() && this.localProvider.Value.CheckLegacyPassword(user, password);
    }

    public override User GetActiveUser()
    {
      if (this.IsLocalProviderSet())
      {
        return this.localProvider.Value.GetActiveUser();
      }

      return this.activeUser.Value ?? this.defaultActiveUser;
    }

    public virtual bool IsLocalProviderSet()
    {
      return this.localProvider.Value != null;
    }

    public override bool Login(User user)
    {
      return this.IsLocalProviderSet() && this.localProvider.Value.Login(user);
    }

    public override bool Login(string userName, bool persistent)
    {
      return this.IsLocalProviderSet() && this.localProvider.Value.Login(userName, persistent);
    }

    public override bool Login(string userName, string password, bool persistent)
    {
      return this.IsLocalProviderSet() && this.localProvider.Value.Login(userName, password, persistent);
    }

    public override void Logout()
    {
      if (this.IsLocalProviderSet())
      {
        this.localProvider.Value.Logout();
      }
    }

    public override void SetActiveUser(string userName)
    {
      if (this.IsLocalProviderSet())
      {
        this.localProvider.Value.SetActiveUser(userName);
      }
      else
      {
        this.activeUser.Value = User.FromName(userName, false);
      }
    }

    public override void SetActiveUser(User user)
    {
      if (this.IsLocalProviderSet())
      {
        this.localProvider.Value.SetActiveUser(user);
      }
      else
      {
        this.activeUser.Value = user;
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