namespace Sitecore.FakeDb.Security.AccessControl
{
  using Sitecore.Security.AccessControl;

  public class AllowAccessResult : AccessResult
  {
    public AllowAccessResult()
      : base(AccessPermission.Allow, new AccessExplanation("Allow"))
    {
    }
  }
}
