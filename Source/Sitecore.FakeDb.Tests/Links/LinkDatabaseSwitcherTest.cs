namespace Sitecore.FakeDb.Tests.Links
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.FakeDb.Links;
  using Sitecore.Links;
  using Xunit;

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
        ((IThreadLocalProvider<LinkDatabase>)Globals.LinkDatabase).LocalProvider.Value.Should().Be(behavior);
      }
    }
  }
}