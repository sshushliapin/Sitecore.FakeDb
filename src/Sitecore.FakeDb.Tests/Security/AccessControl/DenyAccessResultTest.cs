namespace Sitecore.FakeDb.Tests.Security.AccessControl
{
  using FluentAssertions;
  using Sitecore.FakeDb.Security.AccessControl;
  using Sitecore.Security.AccessControl;
  using Xunit;

  public class DenyAccessResultTest
  {
    [Fact]
    public void ShouldBeDenyAccessResult()
    {
      // arrange & act
      var result = new DenyAccessResult();

      // assert
      result.Permission.Should().Be(AccessPermission.Deny);
      result.Explanation.Text.Should().Contain("Deny");
    }
  }
}