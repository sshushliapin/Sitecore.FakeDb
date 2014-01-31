namespace Sitecore.FakeDb.Tests.Security
{
  using FluentAssertions;
  using Sitecore.FakeDb.Security.AccessControl;
  using Sitecore.Security.AccessControl;
  using Sitecore.Security.Accounts;
  using Xunit;

  public class FakeAuthorizationProviderTest
  {
    [Fact]
    public void ShouldGetAccess()
    {
      // arrange
      var provider = new OpenFakeAuthorizationProvider();

      // act
      provider.GetAccessCore(null, null, null).ShouldBeEquivalentTo(new AccessResult(AccessPermission.Allow, new AccessExplanation("Everything is allowed by design.")));
    }

    private class OpenFakeAuthorizationProvider : FakeAuthorizationProvider
    {
      public new AccessResult GetAccessCore(ISecurable entity, Account account, AccessRight accessRight)
      {
        return base.GetAccessCore(entity, account, accessRight);
      }
    }
  }
}