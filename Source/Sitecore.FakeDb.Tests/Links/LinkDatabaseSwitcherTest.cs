namespace Sitecore.FakeDb.Tests.Links
{
  using Xunit;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.FakeDb.Links;
  using Sitecore.Links;

  public class LinkDatabaseSwitcherTest
  {
    [Fact]
    public void ShouldSwitchLinkDatabaseBehavior()
    {
      // arrange
      Globals.Load();

      var behavior = Substitute.For<LinkDatabase>();

      // act & assert
      using (new LinkDatabaseSwitcher(behavior))
      {
        ((FakeLinkDatabase)Globals.LinkDatabase).Behavior.Should().Be(behavior);
      }
    }
  }
}