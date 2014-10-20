namespace Sitecore.FakeDb.Security.Authentication
{
  using System.Collections.Specialized;
  using Sitecore.Common;
  using Sitecore.Security.Accounts;
  using Sitecore.Security.Authentication;

  public class SwitchingAuthenticationProvider : Sitecore.Security.Authentication.SwitchingAuthenticationProvider
  {
    private readonly User defaultActiveUser = User.FromName(@"default\Anonymous", false);

    private string defaultProvider;

    public override void Initialize(string name, NameValueCollection config)
    {
      base.Initialize(name, config);

      this.defaultProvider = config["defaultProvider"];
    }

    public override User GetActiveUser()
    {
      var currentProvider = Switcher<AuthenticationProvider>.CurrentValue;

      if (currentProvider == null && !string.IsNullOrEmpty(this.defaultProvider))
      {
        var provider = AuthenticationManager.Providers[this.defaultProvider];
        if (provider != null)
        {
          return provider.GetActiveUser();
        }
      }

      if (currentProvider == null)
      {
        return this.defaultActiveUser;
      }

      return currentProvider.GetActiveUser();
    }
  }
}