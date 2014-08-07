namespace Sitecore.FakeDb.Tests.Security.AccessControl
{
  using FluentAssertions;
  using Sitecore.FakeDb.Security.AccessControl;
  using Xunit;

  public class DbItemAccessTest
  {
    [Fact]
    public void ShouldNotSetAccessRightsByDefault()
    {
      // arrange
      var itemAccess = new DbItemAccess();

      // act & assert
      itemAccess.CanRead.Should().Be(null);
      itemAccess.CanWrite.Should().Be(null);
      itemAccess.CanRename.Should().Be(null);
      itemAccess.CanCreate.Should().Be(null);
      itemAccess.CanDelete.Should().Be(null);
      itemAccess.CanAdmin.Should().Be(null);
    }
  }
}