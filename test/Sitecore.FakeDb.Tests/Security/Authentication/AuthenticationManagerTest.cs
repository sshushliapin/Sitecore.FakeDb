namespace Sitecore.FakeDb.Tests.Security.Authentication
{
    using System;
    using FluentAssertions;
    using NSubstitute;
    using Sitecore.Security.Accounts;
    using Sitecore.Security.Authentication;
    using Xunit;

#if !SC82161115 && !SC82161221
    /// <summary>
    /// Disable the entire test for Sitecore 8.2.1 and later because the 
    /// existing functionality relies on provider switching which is no longer 
    /// supported. Starting from Sitecore 8.2.1, the providers used by static 
    /// managers have been obsoleted. One have to mock new abstractions 
    /// created for each of the old static managers. For instance, in order to
    /// mock the <see cref="AuthenticationManager"/> one have to inject
    /// <see cref="Sitecore.Abstractions.BaseAuthenticationManager"/> into SUT.
    /// In production, the default implementation will be injected using new
    /// Sitecore DI.
    /// 
    /// Please go to the following thread for details:
    /// https://github.com/sergeyshushlyapin/Sitecore.FakeDb/issues/154
    /// </summary>
    [Obsolete("Not supported in Sitecore 8.2.1 and later.")]
    public class AuthenticationManagerTest
    {
        private readonly AuthenticationProvider provider;

        private readonly User user;

        public AuthenticationManagerTest()
        {
            this.provider = Substitute.For<AuthenticationProvider>();

            this.user = Substitute.For<User>("John", false);
            this.user.Name.Returns("John");
        }

        [Fact]
        public void ShouldGetActiveUser()
        {
            // arrange
            this.provider.GetActiveUser().Returns(this.user);

            using (new AuthenticationSwitcher(this.provider))
            {
                // act & assert
                AuthenticationManager.GetActiveUser().Should().Be(this.user);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShouldLoginUser(bool login)
        {
            // arrange
            this.provider.Login(this.user).Returns(login);

            using (new AuthenticationSwitcher(this.provider))
            {
                // act & assert
                AuthenticationManager.Login(this.user).Should().Be(login);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShouldLoginUserByName(bool login)
        {
            // arrange
            this.provider.Login("John", false).Returns(login);

            using (new AuthenticationSwitcher(this.provider))
            {
                // act & assert
                AuthenticationManager.Login("John", false).Should().Be(login);
            }
        }

        [Fact]
        public void ShouldLogout()
        {
            // arrange
            this.provider.GetActiveUser().Returns(this.user);

            using (new AuthenticationSwitcher(this.provider))
            {
                // act
                AuthenticationManager.Logout();

                // assert
                this.provider.Received().Logout();
            }
        }

        [Fact]
        public void ShouldSetActiveUser()
        {
            // arrange
            using (new AuthenticationSwitcher(this.provider))
            {
                // act
                AuthenticationManager.SetActiveUser(this.user);

                // assert
                this.provider.Received().SetActiveUser(this.user);
            }
        }

        [Fact]
        public void ShouldSetActiveUserByName()
        {
            // arrange
            using (new AuthenticationSwitcher(this.provider))
            {
                // act
                AuthenticationManager.SetActiveUser("John");

                // assert
                this.provider.Received().SetActiveUser("John");
            }
        }
    }
#endif
}