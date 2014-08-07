namespace Sitecore.FakeDb.Tests.Security.Authentication
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Security.Accounts;
  using Sitecore.Security.Authentication;
  using System.Threading;
  using System.Threading.Tasks;
  using Xunit;

  public class AuthenticationSwitcherTest
  {
    [Fact]
    public void ShouldSwitchAuthenticationProviderInCurrentThreadOnly()
    {
      //arrange
      var provider = Substitute.For<AuthenticationProvider>();

      var user = User.FromName(@"extranet\Rambo", true);
      provider.GetActiveUser().Returns(user);

      // act
      var t1 = Task.Factory.StartNew(() =>
      {
        using (new AuthenticationSwitcher(provider))
        {
          AuthenticationManager.Provider.GetActiveUser().Should().BeSameAs(user);

          Thread.Sleep(100);
        }
      });

      // assert
      var t2 = Task.Factory.StartNew(() =>
      {
        AuthenticationManager.Provider.GetActiveUser().Should().NotBeSameAs(user);
      });

      t1.Wait();
      t2.Wait();
    }
  }
}