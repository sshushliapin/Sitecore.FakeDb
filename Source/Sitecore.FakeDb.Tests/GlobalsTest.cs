namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Sitecore.FakeDb.Links;
  using Sitecore.FakeDb.Tasks;
  using Xunit;

  public class GlobalsTest
  {
    [Fact]
    public void ShouldLoadLinkDatabase()
    {
      // act
      Globals.Load();

      // assert
      Globals.LinkDatabase.Should().BeOfType<FakeLinkDatabase>();
      Globals.TaskDatabase.Should().BeOfType<FakeTaskDatabase>();
    }
  }
}