namespace Sitecore.FakeDb.Tests.Security.Accounts
{
  using System;
  using System.Collections.Generic;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.FakeDb.Security.Accounts;
  using Sitecore.Security.Accounts;
  using Sitecore.Security.Domains;
  using Xunit;

  public class FakeRolesInRolesProviderTest : IDisposable
  {
    private const string RoleName = @"sitecore\Editors";

    private const string UserName = @"sitecore\John";

    private readonly FakeRolesInRolesProvider provider;

    private readonly RolesInRolesProvider localProvider;

    private readonly IEnumerable<Role> targetRoles = new List<Role>();

    private readonly IEnumerable<Role> resultRoles = new List<Role>();

    private readonly IEnumerable<Role> memberRoles = new List<Role>();

    private readonly Role role = Role.FromName("Role");

    private readonly Role targetRole = Role.FromName("Target Role");

    private readonly Role memberRole = Role.FromName("Member Role");

    private readonly Role resultRole = Role.FromName("Result Role");

    private readonly User user = User.FromName("User", true);

    public FakeRolesInRolesProviderTest()
    {
      this.localProvider = Substitute.For<RolesInRolesProvider>();

      this.provider = new FakeRolesInRolesProvider();
      this.provider.LocalProvider.Value = this.localProvider;
    }

    [Fact]
    public void ShouldNotThrowIfNoBefaviourSet()
    {
      // arrange
      var stubProvider = new FakeRolesInRolesProvider();

      // act & assert
      stubProvider.AddRolesToRoles(null, null);
      stubProvider.FindRolesInRole(null, null, false);
      stubProvider.FindUsersInRole(null, null, false);
      stubProvider.GetAllRoles(false);
      stubProvider.GetCreatorOwnerRole();
      stubProvider.GetEveryoneRole();
      stubProvider.GetEveryoneRoles();
      stubProvider.GetGlobalRoles();
      stubProvider.GetRoleMembers(null, true);
      stubProvider.GetRolesForRole(null, true);
      stubProvider.GetRolesForUser(null, true);
      stubProvider.GetRolesInRole(null, true);
      stubProvider.GetSystemRoles();
      stubProvider.GetUsersInRole(null, true);
      stubProvider.IsCreatorOwnerRole(null);
      stubProvider.IsEveryoneRole("Everyone");
      stubProvider.IsEveryoneRole("Everyone", Domain.GetDomain("exranet"));
      stubProvider.IsGlobalRole(null);
      stubProvider.IsRoleInRole(null, null, true);
      stubProvider.IsSystemRole(null);
      stubProvider.IsUserInRole(null, null, true);
      stubProvider.RemoveRoleRelations(null);
      stubProvider.RemoveRolesFromRoles(null, null);
    }

    public void ShouldGetDefaultValuesIfNoBefaviourSet()
    {
      // arrange
      var stubProvider = new FakeRolesInRolesProvider();

      // act & assert
      stubProvider.FindRolesInRole(null, null, false).Should().BeEmpty();
      stubProvider.FindUsersInRole(null, null, false).Should().BeEmpty();
      stubProvider.GetAllRoles(false).Should().BeEmpty();
      stubProvider.GetCreatorOwnerRole().Should().Be(Role.FromName("Creator-Owner"));
      stubProvider.GetEveryoneRole().Should().Be(Role.FromName("Everyone"));
      stubProvider.GetEveryoneRoles().Should().BeEmpty();
      stubProvider.GetGlobalRoles().Should().BeEmpty();
      stubProvider.GetRoleMembers(null, true).Should().BeEmpty();
      stubProvider.GetRolesForRole(null, true).Should().BeEmpty();
      stubProvider.GetRolesForUser(null, true).Should().BeEmpty();
      stubProvider.GetRolesInRole(null, true).Should().BeEmpty();
      stubProvider.GetSystemRoles().Should().BeEmpty();
      stubProvider.GetUsersInRole(null, true).Should().BeEmpty();
      stubProvider.IsCreatorOwnerRole(null).Should().BeFalse();
      stubProvider.IsEveryoneRole("Everyone").Should().BeTrue();
      stubProvider.IsEveryoneRole("Everyone", Domain.GetDomain("extranet")).Should().BeTrue();
      stubProvider.IsGlobalRole(null).Should().BeFalse();
      stubProvider.IsRoleInRole(null, null, true).Should().BeFalse();
      stubProvider.IsSystemRole(null).Should().BeFalse();
      stubProvider.IsUserInRole(null, null, true).Should().BeFalse();
    }

    [Theory]
    [InlineData("Everyone", true)]
    [InlineData("Somebody", false)]
    public void ShouldCheckIfEveryoneRole(string roleName, bool expectedResult)
    {
      // arrange
      var stubProvider = new FakeRolesInRolesProvider();

      // act & assert
      stubProvider.IsEveryoneRole(roleName).Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(@"extranet\Everyone", true)]
    [InlineData(@"extranet\Somebody", false)]
    public void ShouldCheckIfDomainEveryoneRole(string roleName, bool expectedResult)
    {
      // arrange
      var stubProvider = new FakeRolesInRolesProvider();
      var domain = Domain.GetDomain("extranet");

      // act & assert
      stubProvider.IsEveryoneRole(roleName, domain).Should().Be(expectedResult);
    }

    [Fact]
    public void ShouldBeThreadLocalProvider()
    {
      // act & assert
      this.provider.Should().BeAssignableTo<IThreadLocalProvider<RolesInRolesProvider>>();
    }

    [Fact]
    public void ShouldAddRolesToRoles()
    {
      // act
      this.provider.AddRolesToRoles(this.memberRoles, this.targetRoles);

      // assert
      this.localProvider.Received().AddRolesToRoles(this.memberRoles, this.targetRoles);
    }

    [Fact]
    public void ShouldFindRolesInRole()
    {
      // arrange
      this.localProvider.FindRolesInRole(this.targetRole, RoleName, true).Returns(this.resultRoles);

      // act & assert
      this.provider.FindRolesInRole(this.targetRole, RoleName, true).Should().BeSameAs(this.resultRoles);
    }

    [Fact]
    public void ShouldFindUsersInRole()
    {
      // arrange
      this.localProvider.FindUsersInRole(this.targetRole, UserName, true).Returns(this.resultRoles);

      // act & assert
      this.provider.FindUsersInRole(this.targetRole, UserName, true).Should().BeSameAs(this.resultRoles);
    }

    [Fact]
    public void ShouldGetAllRoles()
    {
      // arrange
      this.localProvider.GetAllRoles(true).Returns(this.resultRoles);

      // act & assert
      this.provider.GetAllRoles(true).Should().BeSameAs(this.resultRoles);
    }

    [Fact]
    public void ShouldGetCreatorOwnerRole()
    {
      // arrange
      this.localProvider.GetCreatorOwnerRole().Returns(this.resultRole);

      // act & assert
      this.provider.GetCreatorOwnerRole().Should().BeSameAs(this.resultRole);
    }

    [Fact]
    public void ShouldGetEveryoneRole()
    {
      // arrange
      this.localProvider.GetEveryoneRole().Returns(this.resultRole);

      // act & assert
      this.provider.GetEveryoneRole().Should().BeSameAs(this.resultRole);
    }

    [Fact]
    public void ShouldGetEveryoneRoles()
    {
      // arrange
      this.localProvider.GetEveryoneRoles().Returns(this.resultRoles);

      // act & assert
      this.provider.GetEveryoneRoles().Should().BeSameAs(this.resultRoles);
    }

    [Fact]
    public void ShouldGetGlobalRoles()
    {
      // arrange
      this.localProvider.GetGlobalRoles().Returns(this.resultRoles);

      // act & assert
      this.provider.GetGlobalRoles().Should().BeSameAs(this.resultRoles);
    }

    [Fact]
    public void ShouldGetRoleMembers()
    {
      // arrange
      this.localProvider.GetRoleMembers(this.role, true).Returns(this.resultRoles);

      // act & assert
      this.provider.GetRoleMembers(this.role, true).Should().BeSameAs(this.resultRoles);
    }

    [Fact]
    public void ShouldGetRolesForRole()
    {
      // arrange
      this.localProvider.GetRolesForRole(this.role, true).Returns(this.resultRoles);

      // act & assert
      this.provider.GetRolesForRole(this.role, true).Should().BeSameAs(this.resultRoles);
    }

    [Fact]
    public void ShouldGetRolesForUser()
    {
      // arrange
      this.localProvider.GetRolesForUser(this.user, true).Returns(this.resultRoles);

      // act & assert
      this.provider.GetRolesForUser(this.user, true).Should().BeSameAs(this.resultRoles);
    }

    [Fact]
    public void ShouldGetRolesInRole()
    {
      // arrange
      this.localProvider.GetRolesInRole(this.role, true).Returns(this.resultRoles);

      // act & assert
      this.provider.GetRolesInRole(this.role, true).Should().BeSameAs(this.resultRoles);
    }

    [Fact]
    public void ShouldGetSystemRoles()
    {
      // arrange
      this.localProvider.GetSystemRoles().Returns(this.resultRoles);

      // act & assert
      this.provider.GetSystemRoles().Should().BeSameAs(this.resultRoles);
    }

    [Fact]
    public void ShouldGetUsersInRole()
    {
      // arrange
      var users = new List<User>();
      this.localProvider.GetUsersInRole(this.role, true).Returns(users);

      // act & assert
      this.provider.GetUsersInRole(this.role, true).Should().BeSameAs(users);
    }

    [Fact]
    public void ShouldCallIsCreatorOwnerRole()
    {
      // arrange
      this.localProvider.IsCreatorOwnerRole(RoleName).Returns(true);

      // act & assert
      this.provider.IsCreatorOwnerRole(RoleName).Should().BeTrue();
    }

    [Fact]
    public void ShouldCallIsEveryoneRole()
    {
      // arrange
      this.localProvider.IsEveryoneRole(RoleName).Returns(true);

      // act & assert
      this.provider.IsEveryoneRole(RoleName).Should().BeTrue();
    }

    [Fact]
    public void ShouldCallIsEveryoneRoleWithDomain()
    {
      // arrange
      var domain = new Domain();
      this.localProvider.IsEveryoneRole(RoleName, domain).Returns(true);

      // act & assert
      this.provider.IsEveryoneRole(RoleName, domain).Should().BeTrue();
    }

    [Fact]
    public void ShouldCallIsGlobalRole()
    {
      // arrange
      this.localProvider.IsGlobalRole(this.role).Returns(true);

      // act & assert
      this.provider.IsGlobalRole(this.role).Should().BeTrue();
    }

    [Fact]
    public void ShouldCallIsRoleInRole()
    {
      // arrange
      this.localProvider.IsRoleInRole(this.memberRole, this.targetRole, true).Returns(true);

      // act & assert
      this.provider.IsRoleInRole(this.memberRole, this.targetRole, true).Should().BeTrue();
    }

    [Fact]
    public void ShouldCallIsSystemRole()
    {
      // arrange
      this.localProvider.IsSystemRole(RoleName).Returns(true);

      // act & assert
      this.provider.IsSystemRole(RoleName).Should().BeTrue();
    }

    [Fact]
    public void ShouldCallIsUserInRole()
    {
      // arrange
      this.localProvider.IsUserInRole(this.user, this.targetRole, true).Returns(true);

      // act & assert
      this.provider.IsUserInRole(this.user, this.targetRole, true).Should().BeTrue();
    }

    [Fact]
    public void ShouldCallRemoveRoleRelations()
    {
      // act
      this.provider.RemoveRoleRelations(RoleName);

      // assert
      this.localProvider.Received().RemoveRoleRelations(RoleName);
    }

    [Fact]
    public void ShouldCallRemoveRolesFromRoles()
    {
      // act
      this.provider.RemoveRolesFromRoles(this.memberRoles, this.targetRoles);

      // assert
      this.localProvider.Received().RemoveRolesFromRoles(this.memberRoles, this.targetRoles);
    }

    public void Dispose()
    {
      this.provider.Dispose();
    }
  }
}
