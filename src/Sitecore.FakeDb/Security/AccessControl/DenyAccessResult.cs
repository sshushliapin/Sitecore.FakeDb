namespace Sitecore.FakeDb.Security.AccessControl
{
  using Sitecore.Security.AccessControl;

  public class DenyAccessResult : AccessResult
  {
    public DenyAccessResult()
      : base(AccessPermission.Deny, new AccessExplanation("Deny"))
    {
    }
  }
}
