namespace Sitecore.FakeDb.Tests.Security
{
  using System;
  using System.Web.Security;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.FakeDb.Security.Web;
  using Xunit;

  public class FakeRoleProviderTest : IDisposable
  {
    private const string RoleName = @"sitecore\Editors";

    private const string UserName = @"sitecore\John";

    private readonly FakeRoleProvider provider;

    private readonly RoleProvider localProvider;

    private readonly string[] roles;

    private readonly string[] users;

    public FakeRoleProviderTest()
    {
      this.localProvider = Substitute.For<RoleProvider>();
      this.provider = new FakeRoleProvider();
      this.provider.LocalProvider.Value = this.localProvider;

      this.roles = new[] { @"sitecore\Editors", @"sitecore\Authors" };
      this.users = new[] { @"sitecore\John", @"sitecore\Jane" };
    }

    [Fact]
    public void ShouldDoNothingIfNoBehaviourSet()
    {
      // arrange 
      var stubProvider = new FakeRoleProvider();

      // act & assert
      Assert.Null(stubProvider.ApplicationName);
      stubProvider.AddUsersToRoles(null, null);
      stubProvider.CreateRole(null);
      stubProvider.DeleteRole(null, false).Should().BeFalse();
      stubProvider.FindUsersInRole(null, null).Should().BeEmpty();
      stubProvider.GetAllRoles().Should().BeEmpty();
      stubProvider.GetRolesForUser(null).Should().BeEmpty();
      stubProvider.GetUsersInRole(null).Should().BeEmpty();
      stubProvider.IsUserInRole(null, null).Should().BeFalse();
      stubProvider.RemoveUsersFromRoles(null, null);
      stubProvider.RoleExists(null).Should().BeFalse();
    }

    [Fact]
    public void ShouldBeThreadLocal()
    {
      // act & assert
      this.provider.Should().BeAssignableTo<IThreadLocalProvider<RoleProvider>>();
    }

    [Fact]
    public void ShouldCreateRole()
    {
      // act
      this.provider.CreateRole(RoleName);

      // assert
      this.localProvider.Received().CreateRole(RoleName);
    }

    [Fact]
    public void ShouldDeleteRole()
    {
      // arrange
      this.localProvider.DeleteRole(RoleName, true).Returns(true);

      // act & assert
      this.provider.DeleteRole(RoleName, true).Should().BeTrue();
    }

    [Fact]
    public void ShouldFindUsersInRole()
    {
      // arrange
      this.localProvider.FindUsersInRole(RoleName, "J").Returns(this.users);

      // act & assert
      this.provider.FindUsersInRole(RoleName, "J").Should().BeSameAs(this.users);
    }

    [Fact]
    public void ShouldGetAllRoles()
    {
      // arrange
      this.localProvider.GetAllRoles().Returns(this.roles);

      // act & assert
      this.provider.GetAllRoles().Should().BeSameAs(this.roles);
    }

    [Fact]
    public void ShouldGetRolesForUser()
    {
      // arrange
      this.localProvider.GetRolesForUser(UserName).Returns(this.roles);

      // act & assert
      this.provider.GetRolesForUser(UserName).Should().BeSameAs(this.roles);
    }

    [Fact]
    public void ShouldGetUsersInRole()
    {
      // arrange
      this.localProvider.GetUsersInRole(RoleName).Returns(this.users);

      // act & assert
      this.provider.GetUsersInRole(RoleName).Should().BeSameAs(this.users);
    }

    [Fact]
    public void ShouldCallIsUserInRole()
    {
      // arrange
      this.localProvider.IsUserInRole(UserName, RoleName).Returns(true);

      // act & assert
      this.provider.IsUserInRole(UserName, RoleName).Should().BeTrue();
    }

    [Fact]
    public void ShouldRemoveUsersFromRoles()
    {
      // act
      this.provider.RemoveUsersFromRoles(this.users, this.roles);

      // assert
      this.localProvider.Received().RemoveUsersFromRoles(this.users, this.roles);
    }

    [Fact]
    public void ShouldCallRoleExists()
    {
      // arrange
      this.localProvider.RoleExists(RoleName).Returns(true);

      // act & assert
      this.provider.RoleExists(RoleName).Should().BeTrue();
    }

    public void Dispose()
    {
      this.provider.Dispose();
    }
  }
}