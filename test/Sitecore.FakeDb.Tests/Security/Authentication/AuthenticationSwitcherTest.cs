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
#pragma warning disable 618
          AuthenticationManager.Provider.GetActiveUser().Should().BeSameAs(user);
#pragma warning restore 618

          Thread.Sleep(100);
        }
      });

      // assert
      var t2 = Task.Factory.StartNew(() =>
      {
#pragma warning disable 618
        AuthenticationManager.Provider.GetActiveUser().Should().NotBeSameAs(user);
#pragma warning restore 618
      });

      t1.Wait();
      t2.Wait();
    }
  }
}