using FluentAssertions;
using NSubstitute;
using Sitecore.FakeDb.Links;
using Sitecore.Links;

namespace Sitecore.FakeDb.Tests.Links
{

  using Xunit;

  public class LinkDatabaseSwitcherTest
  {
    [Fact]
    public void ShouldTemporarilyOverrideLinkDatabaseBehavior()
    {
      // arrange
      Globals.Load();
      var linkDb = Globals.LinkDatabase as FakeLinkDatabase;
      var behavior = Substitute.For<LinkDatabase>();

      // act & assert
      linkDb.Behavior.Should().BeNull();

      using (new LinkDatabaseSwitcher(behavior))
      {
        linkDb.Behavior.Should().Be(behavior);
      }

      linkDb.Behavior.Should().BeNull();
    }


  }
}