namespace Sitecore.FakeDb.Tests.Security.AccessControl
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.FakeDb.Security.AccessControl;
  using Sitecore.Security.AccessControl;
  using Sitecore.Security.Accounts;
  using Xunit;

  public class AuthorizationSwitcherTest
  {
    private readonly AuthorizationProvider provider = Substitute.For<AuthorizationProvider>();

    [Fact]
    public void ShouldBeThreadLocalProviderSwitcher()
    {
      // arrange & act
      using (var switcher = new Sitecore.FakeDb.Security.AccessControl.AuthorizationSwitcher(this.provider))
      {
        // assert
        switcher.Should().BeAssignableTo<ThreadLocalProviderSwitcher<AuthorizationProvider>>();
      }
    }

    [Fact]
    public void ShouldSwitchLocalProvider()
    {
      // act
      using (new Sitecore.FakeDb.Security.AccessControl.AuthorizationSwitcher(this.provider))
      {
        // assert
        ((IThreadLocalProvider<AuthorizationProvider>)AuthorizationManager.Provider).LocalProvider.Value.Should().BeSameAs(provider);
      }
    }

    [Fact]
    public void ShouldSwitchAuthorizationProvider()
    {
      // arrange
      var editorUser = User.FromName(@"extranet\Editor", true);
      var anonymousUser = User.FromName(@"extranet\Anonymous", true);

      using (var db = new Db { new DbItem("home") })
      {
        var item = db.GetItem("/sitecore/content/home");

        this.provider.GetAccess(item, editorUser, AccessRight.ItemRead).Returns(new AllowAccessResult());
        this.provider.GetAccess(item, anonymousUser, AccessRight.ItemRead).Returns(new DenyAccessResult());

        // act
        using (new Sitecore.FakeDb.Security.AccessControl.AuthorizationSwitcher(this.provider))
        {
          // assert
          AuthorizationManager.IsAllowed(item, AccessRight.ItemRead, editorUser).Should().BeTrue();
          AuthorizationManager.IsAllowed(item, AccessRight.ItemRead, anonymousUser).Should().BeFalse();
        }
      }
    }
  }
}