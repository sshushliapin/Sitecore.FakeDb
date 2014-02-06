namespace Sitecore.FakeDb.Security.Authentication
{
  using Sitecore.Security.Accounts;
  using Sitecore.Security.Authentication;

  public class StubAuthenticationProvider : AuthenticationProvider
  {
    public override bool Login(User user)
    {
      return true;
    }

    public override bool Login(string userName, string password, bool persistent)
    {
      return true;
    }

    public override bool Login(string userName, bool persistent)
    {
      return true;
    }

    public override void Logout()
    {
    }

    public override void SetActiveUser(string userName)
    {
    }

    public override void SetActiveUser(User user)
    {
    }
  }
}