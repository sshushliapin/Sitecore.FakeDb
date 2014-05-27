namespace Sitecore.FakeDb.Tests.Links
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.FakeDb.Links;
  using Sitecore.Links;
  using Xunit;

  public class SwitchingLinkDatabaseTest
  {
    [Fact]
    public void ShouldSetBehaviour()
    {
      // arrange
      var behavior = Substitute.For<LinkDatabase>();

      // act
      var linkDatabase = new SwitchingLinkDatabase { Behavior = behavior };

      // assert
      linkDatabase.Behavior.Should().BeSameAs(behavior);
    }
  }
}