namespace Sitecore.FakeDb.Tests.Security
{
  using System.Web.Security;
  using FluentAssertions;
  using NSubstitute;
  using Xunit;
  using Sitecore.FakeDb.Security.Web;

  public class MembershipSwitcherTest
  {
    [Fact]
    public void ShouldSwitchMembershipProvider()
    {
      // arrange
      var localProvider = Substitute.For<MembershipProvider, IThreadLocalProvider<MembershipProvider>>();

      // act
      using (new MembershipSwitcher(localProvider))
      {
        ((IThreadLocalProvider<MembershipProvider>)Membership.Provider).LocalProvider.Value.Should().Be(localProvider);
      }
    }
  }
}