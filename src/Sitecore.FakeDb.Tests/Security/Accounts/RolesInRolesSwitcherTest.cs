namespace Sitecore.FakeDb.Tests.Security.Accounts
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.FakeDb.Security.Accounts;
  using Sitecore.Security.Accounts;
  using Xunit;

  public class RolesInRolesSwitcherTest
  {
    [Fact]
    public void ShouldSwitchRolesInRolesProvider()
    {
      // arrange
      var roles = new[] { Role.FromName(@"sitecore/Editors") };

      var localProvider = Substitute.For<RolesInRolesProvider, IThreadLocalProvider<RolesInRolesProvider>>();
      localProvider.GetAllRoles(true).Returns(roles);

      // act
      using (new RolesInRolesSwitcher(localProvider))
      {
        // assert
        RolesInRolesManager.GetAllRoles(true).Should().BeSameAs(roles);
      }

      RolesInRolesManager.GetAllRoles(true).Should().BeEmpty();
    }
  }
}