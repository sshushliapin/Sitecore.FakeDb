namespace Sitecore.FakeDb.Tests.Security.AccessControl
{
  using FluentAssertions;
  using NSubstitute;
  using Ploeh.AutoFixture;
  using Sitecore.Common;
  using Sitecore.FakeDb.Security.AccessControl;
  using Sitecore.Security.AccessControl;
  using Sitecore.Security.Accounts;
  using Xunit;

  public class SwitchingAuthorizationProviderTest
  {
    private readonly AuthorizationProvider providerMock;

    private readonly SwitchingAuthorizationProvider mockableProvider;

    public SwitchingAuthorizationProviderTest()
    {
      this.providerMock = Substitute.For<AuthorizationProvider>();
      this.mockableProvider = new SwitchingAuthorizationProvider();
    }

    [Fact]
    public void ShouldGetShitchedProvider()
    {
      // act
      using (new Switcher<AuthorizationProvider, AuthorizationProvider>(this.providerMock))
      {
        // assert
        this.mockableProvider.CurrentProvider.Should().Be(this.providerMock);
      }
    }

    [Fact]
    public void ShouldGetAccessFromUsingMockedProvider()
    {
      // arrange
      var fixture = new Fixture();

      var entity = Substitute.For<ISecurable>();
      var account = fixture.Create<User>();
      var accessRight = fixture.Create<AccessRight>();
      var accessResult = fixture.Create<AccessResult>();

      this.providerMock.GetAccess(entity, account, accessRight).Returns(accessResult);

      // act
      using (new Switcher<AuthorizationProvider, AuthorizationProvider>(this.providerMock))
      {
        // assert
        this.mockableProvider.GetAccess(entity, account, accessRight).Should().Be(accessResult);
      }
    }
  }
}