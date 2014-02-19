namespace Sitecore.FakeDb.Tests.Security.AccessControl
{
  using FluentAssertions;
  using Sitecore.FakeDb.Security.AccessControl;
  using Xunit;

  public class DbItemAccessTest
  {
    [Fact]
    public void ShouldSetAllAccessRightsToTrueByDefault()
    {
      // arrange
      var itemAccess = new DbItemAccess();

      // act & assert
      itemAccess.CanRead.Should().BeTrue();
      itemAccess.CanWrite.Should().BeTrue();
      itemAccess.CanRename.Should().BeTrue();
      itemAccess.CanCreate.Should().BeTrue();
      itemAccess.CanDelete.Should().BeTrue();
      itemAccess.CanAdmin.Should().BeTrue();
    }
  }
}