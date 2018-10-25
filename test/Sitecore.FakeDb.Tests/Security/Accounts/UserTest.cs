namespace Sitecore.FakeDb.Tests.Security.Accounts
{
    using System.Web.Security;
    using FluentAssertions;
    using NSubstitute;
    using Sitecore.FakeDb.Security.Accounts;
    using Sitecore.FakeDb.Security.Web;
    using Sitecore.Security.Accounts;
    using Xunit;

    public class UserTest
    {
        private const string UserName = "John";

        private readonly MembershipProvider provider = Substitute.For<MembershipProvider, IThreadLocalProvider<MembershipProvider>>();

        [Fact]
        public void ShouldCreateUser()
        {
            // arrange
            MembershipCreateStatus status;

            // act
            using (new MembershipSwitcher(this.provider))
            {
                User.Create(UserName, "******");
            }

            this.provider.Received().CreateUser(UserName, "******", Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<object>(), out status);
        }

        [Fact]
        public void ShouldDeleteUser()
        {
            // arrange
            var user = User.FromName(UserName, false);

            using (new MembershipSwitcher(this.provider))
            {
                // act
                user.Delete();

                // assert
                this.provider.Received().DeleteUser(UserName, true);
            }
        }

        [Fact]
        public void ShouldReturnTrueIfUserExists()
        {
            // arrange
            var user = new FakeMembershipUser();
            this.provider.GetUser(UserName, true).Returns(user);

            // act
            using (new MembershipSwitcher(this.provider))
            {
                // assert
                User.Exists(UserName).Should().BeTrue();
            }
        }

        [Fact]
        public void ShouldReturnFalseIfUserDoesNotExist()
        {
            // arrange
            this.provider.GetUser(UserName, true).ReturnsForAnyArgs(x => null);

            // act
            using (new MembershipSwitcher(this.provider))
            {
                // assert
                User.Exists(UserName).Should().BeFalse();
            }
        }

        [Fact]
        public void ShouldNotBeAdministratorByDefault()
        {
            var user = User.FromName(UserName, true);

            user.IsAdministrator.Should().BeFalse();
        }
    }
}