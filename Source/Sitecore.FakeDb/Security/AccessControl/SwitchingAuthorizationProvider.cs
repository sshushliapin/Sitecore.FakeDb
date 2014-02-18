namespace Sitecore.FakeDb.Security.AccessControl
{
  using System.Collections.Specialized;
  using Sitecore.Common;
  using Sitecore.Security.AccessControl;
  using Sitecore.Security.Accounts;

  public class SwitchingAuthorizationProvider : AuthorizationProvider
  {
    public override void Initialize(string name, NameValueCollection config)
    {
      // TODO: Move to configuration.
      var provider = new AuthorizationProviderStub();
      new Switcher<AuthorizationProvider, AuthorizationProvider>(provider);

      base.Initialize(name, config);
    }

    public string DefaultProviderName { get; private set; }

    public override AccessResult GetAccess(ISecurable entity, Account account, AccessRight accessRight)
    {
      return this.CurrentProvider.GetAccess(entity, account, accessRight);
    }

    public override AccessRuleCollection GetAccessRules(ISecurable entity)
    {
      throw new System.NotImplementedException();
    }

    public override void SetAccessRules(ISecurable entity, AccessRuleCollection rules)
    {
      throw new System.NotImplementedException();
    }

    protected override AccessResult GetAccessCore(ISecurable entity, Account account, AccessRight accessRight)
    {
      throw new System.NotImplementedException();
    }

    public AuthorizationProvider CurrentProvider
    {
      get { return Switcher<AuthorizationProvider, AuthorizationProvider>.CurrentValue; }
    }
  }
}