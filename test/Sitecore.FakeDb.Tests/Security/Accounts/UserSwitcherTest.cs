namespace Sitecore.FakeDb.Tests.Security.Accounts
{
  using FluentAssertions;
  using Sitecore.Security.Accounts;
  using Xunit;

  public class UserSwitcherTest
  {
    [Fact]
    public void ShouldSwitchContextUser()
    {
      // act
      using (new UserSwitcher(@"extranet\Visitor", true))
      {
        // assert
        Context.User.Name.Should().Be(@"extranet\Visitor");
      }

      Context.User.Name.Should().Be(@"default\Anonymous");
    }
  }
}