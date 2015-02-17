namespace Sitecore.FakeDb.Tests.Security.AccessControl
{
  using FluentAssertions;
  using Sitecore.FakeDb.Security.AccessControl;
  using Sitecore.Security.AccessControl;
  using Xunit;

  public class AllowAccessResultTest
  {
    [Fact]
    public void ShouldBeAllowAccessResult()
    {
      // arrange & act
      var result = new AllowAccessResult();

      // assert
      result.Permission.Should().Be(AccessPermission.Allow);
      result.Explanation.Text.Should().Contain("Allow");
    }
  }
}