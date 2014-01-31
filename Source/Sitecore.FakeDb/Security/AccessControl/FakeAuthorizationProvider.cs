namespace Sitecore.FakeDb.Security.AccessControl
{
  using Sitecore.Security.AccessControl;
  using Sitecore.Security.Accounts;

  public class FakeAuthorizationProvider : AuthorizationProvider
  {
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
      return new AccessResult(AccessPermission.Allow, new AccessExplanation("Everything is allowed by design."));
    }
  }
}