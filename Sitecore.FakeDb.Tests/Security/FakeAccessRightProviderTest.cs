namespace Sitecore.FakeDb.Tests.Security
{
  using FluentAssertions;
  using Sitecore.FakeDb.Security.AccessControl;
  using Sitecore.Security.AccessControl;
  using Xunit;

  public class FakeAccessRightProviderTest
  {
    [Fact]
    public void ShouldGetAccessRight()
    {
      // arrange
      var provider = new FakeAccessRightProvider();

      // act
      provider.GetAccessRight("item:dosmth").ShouldBeEquivalentTo(new AccessRight("item:dosmth"));
    }
  }
}