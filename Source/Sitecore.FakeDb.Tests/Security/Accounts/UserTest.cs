namespace Sitecore.FakeDb.Tests.Security.Accounts
{
  using System.Web.Security;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.FakeDb.Security.Web;
  using Xunit;
  using Sitecore.Security.Accounts;
  using Xunit.Extensions;
  using System;
  using Sitecore.FakeDb.Security.Accounts;

  public class UserTest
  {
    private const string UserName = "John";

    private MembershipProvider localProvider = Substitute.For<MembershipProvider, IThreadLocalProvider<MembershipProvider>>();

    [Fact]
    public void ShouldCreateUser()
    {
      // arrange
      var status = MembershipCreateStatus.Success;

      // act
      using (new MembershipSwitcher(localProvider))
      {
        User.Create(UserName, "******");
      }

      localProvider
        .Received()
        .CreateUser(UserName, "******", Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
          Arg.Any<bool>(), Arg.Any<object>(), out status);
    }

    [Fact]
    public void ShouldDeleteUser()
    {
      // arrange
      var user = User.FromName(UserName, false);
      using (new MembershipSwitcher(this.localProvider))
      {
        // act
        user.Delete();

        // assert
        this.localProvider.Received().DeleteUser(UserName, true);
      }
    }

    [Fact]
    public void ShouldReturnTrueIfUserExists()
    {
      // arrange
      var user = new FakeMembershipUser();
      localProvider.GetUser(UserName, true).Returns(user);

      // act
      using (new MembershipSwitcher(localProvider))
      {
        // assert
        User.Exists(UserName).Should().BeTrue();
      }
    }

    [Fact]
    public void ShouldReturnFalseIfUserDoesNotExist()
    {
      // arrange
      localProvider.GetUser(UserName, true).ReturnsForAnyArgs(Arg.Any<MembershipUser>());

      // act
      using (new MembershipSwitcher(localProvider))
      {
        // assert
        User.Exists(UserName).Should().BeFalse();
      }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldCheckIfUserIsInRoleUsingRolesInRolesManager(bool isInRole)
    {
      // arrange
      var user = User.FromName(UserName, true);
      var role = Role.FromName(@"sitecore\Editor");

      var rolesProvider = Substitute.For<RolesInRolesProvider>();
      rolesProvider.IsUserInRole(user, role, true).Returns(isInRole);

      using (new RolesInRolesSwitcher(rolesProvider))
      {
        //act & assert
        user.IsInRole(@"sitecore\Editor").Should().Be(isInRole);
      }
    }
  }
}