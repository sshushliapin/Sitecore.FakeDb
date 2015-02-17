namespace Sitecore.FakeDb.Security.AccessControl
{
  using Sitecore.Security.AccessControl;

  public class FakeAccessRightProvider : AccessRightProvider
  {
    public override AccessRight GetAccessRight(string accessRightName)
    {
      return new AccessRight(accessRightName);
    }
  }
}