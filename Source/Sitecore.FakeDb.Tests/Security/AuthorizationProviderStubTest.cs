namespace Sitecore.FakeDb.Tests.Security
{
  using FluentAssertions;
  using NSubstitute;
  using Ploeh.AutoFixture;
  using Sitecore.FakeDb.Security.AccessControl;
  using Sitecore.Security.AccessControl;
  using Sitecore.Security.Accounts;
  using Xunit;

  public class AuthorizationProviderStubTest
  {
    [Fact]
    public void ShouldGetAccessPermissionAllow()
    {
      // arrange
      var provider = new AuthorizationProviderStub();

      var fixture = new Fixture();

      var entity = Substitute.For<ISecurable>();
      var account = fixture.Create<User>();
      var accessRight = fixture.Create<AccessRight>();

      // act
      provider.GetAccess(entity, account, accessRight).ShouldBeEquivalentTo(new AccessResult(AccessPermission.Allow, new AccessExplanation("Everything is allowed by design.")));
    }
  }
}