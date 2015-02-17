namespace Sitecore.FakeDb.Tests.Security
{
  using System.Web.Security;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.FakeDb.Security.Web;
  using Xunit;

  public class RoleProviderSwitcherTest
  {
    [Fact]
    public void ShouldSwitchRoleProvider()
    {
      // arrange
      var roles = new[] { @"sitecore/Editors" };

      var localProvider = Substitute.For<RoleProvider, IThreadLocalProvider<RoleProvider>>();
      localProvider.GetAllRoles().Returns(roles);

      // act
      using (new RoleProviderSwitcher(localProvider))
      {
        // assert
        Roles.GetAllRoles().Should().BeSameAs(roles);
      }

      Roles.GetAllRoles().Should().BeEmpty();
    }
  }
}