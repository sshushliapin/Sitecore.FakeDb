namespace Sitecore.FakeDb.Tests.Security
{
  using System.Web.Security;
  using FluentAssertions;
  using Sitecore.FakeDb.Security.Web;
  using Xunit;

  public class FakeRoleProviderTest
  {
    private readonly FakeRoleProvider provider;

    public FakeRoleProviderTest()
    {
      this.provider = new FakeRoleProvider();
    }

    [Fact]
    public void ShouldDoNothingIfNoBehaviourSet()
    {
      // act & assert
      Assert.Null(this.provider.ApplicationName);
      Assert.DoesNotThrow(() => this.provider.AddUsersToRoles(null, null));
      Assert.DoesNotThrow(() => this.provider.CreateRole(null));
      this.provider.DeleteRole(null, false).Should().BeFalse();
      this.provider.FindUsersInRole(null, null).Should().BeEmpty();
      this.provider.GetAllRoles().Should().BeEmpty();
      this.provider.GetRolesForUser(null).Should().BeEmpty();
      this.provider.GetUsersInRole(null).Should().BeEmpty();
      this.provider.IsUserInRole(null, null).Should().BeFalse();
      Assert.DoesNotThrow(() => this.provider.RemoveUsersFromRoles(null, null));
      this.provider.RoleExists(null).Should().BeFalse();
    }

    [Fact]
    public void ShouldBeBehavioral()
    {
      // act & assert
      this.provider.Should().BeAssignableTo<IBehavioral<RoleProvider>>();
    }
  }
}