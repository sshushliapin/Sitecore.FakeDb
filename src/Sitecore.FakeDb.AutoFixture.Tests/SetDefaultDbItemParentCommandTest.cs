namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using FluentAssertions;
  using Ploeh.AutoFixture;
  using Xunit;

  public class SetDefaultDbItemParentCommandTest
  {
    [Fact]
    public void ShouldSetParentIdToSitecoreContentRoot()
    {
      // arrange
      var fixture = new Fixture();
      var item = fixture.Create<DbItem>();

      var sut = new SetDefaultDbItemParentCommand();

      // act
      sut.Execute(item, null);

      // assert
      item.ParentID.Should().Be(ItemIDs.ContentRoot);
    }

    [Fact]
    public void ShouldNotThrowIfNoDbItemPassed()
    {
      // arrange
      var sut = new SetDefaultDbItemParentCommand();

      // act
      sut.Execute(new object(), null);
    }
  }
}