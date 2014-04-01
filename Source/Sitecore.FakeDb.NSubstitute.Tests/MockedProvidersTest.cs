namespace Sitecore.FakeDb.NSubstitute.Tests
{
  using FluentAssertions;
  using global::NSubstitute;
  using Sitecore.Buckets.Managers;
  using Sitecore.Security.AccessControl;
  using Sitecore.Security.Accounts;
  using Sitecore.Security.Authentication;
  using Xunit;

  public class MockedProvidersTest
  {
    [Fact]
    public void ShouldMockAuthenticationProvider()
    {
      // arrange
      AuthenticationManager.Provider.Login("John", true).Returns(true);

      // act & assert
      AuthenticationManager.Login("John", true).Should().BeTrue();
      AuthenticationManager.Login("Robber", true).Should().BeFalse();
    }

    [Fact]
    public void ShouldMockAuthorizationProvider1()
    {
      // arrange
      var expectedEntity = Substitute.For<ISecurable>();
      var account = Substitute.For<Account>("John", AccountType.User);
      var accessRights = Substitute.For<AccessRight>("item:read");
      var result = Substitute.For<AccessResult>(AccessPermission.Allow, new AccessExplanation("OK"));

      AuthorizationManager.Provider.GetAccess(expectedEntity, account, accessRights).Returns(result);

      // act & assert
      AuthorizationManager.GetAccess(expectedEntity, account, accessRights).Should().BeSameAs(result);
    }

    [Fact]
    public void ShouldMockBucketsProvider()
    {
      // act
      BucketManager.AddSearchTabToItem(null);

      // assert
      BucketManager.Provider.Received().AddSearchTabToItem(null);
    }
  }
}