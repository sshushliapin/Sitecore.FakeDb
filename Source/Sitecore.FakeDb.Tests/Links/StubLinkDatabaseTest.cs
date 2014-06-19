namespace Sitecore.FakeDb.Tests.Links
{
  using FluentAssertions;
  using Sitecore.FakeDb.Links;
  using Xunit;

  public class StubLinkDatabaseTest
  {
    [Fact]
    public void ShouldReturnEmptyValuesWithoutBehaviorSet()
    {
      // arrange
      var linkDatabase = new StubLinkDatabase();

      // act & assert
      linkDatabase.Compact(null);
      linkDatabase.GetBrokenLinks(null).Should().BeEmpty();
      linkDatabase.GetReferenceCount(null).Should().Be(0);
      linkDatabase.GetReferences(null).Should().BeEmpty();
      linkDatabase.GetItemReferences(null, false).Should().BeEmpty();
      linkDatabase.GetReferrerCount(null).Should().Be(0);
      linkDatabase.GetReferrers(null).Should().BeEmpty();
      linkDatabase.GetReferrers(null, null).Should().BeEmpty();
      linkDatabase.GetItemReferrers(null, false).Should().BeEmpty();
      linkDatabase.GetItemVersionReferrers(null).Should().BeEmpty();
      linkDatabase.GetReferrers(null, false).Should().BeEmpty();
      linkDatabase.HasExternalReferrers(null, false).Should().BeFalse();
      linkDatabase.Rebuild(null);
      linkDatabase.RemoveReferences(null);
      linkDatabase.UpdateItemVersionReferences(null);
      linkDatabase.UpdateReferences(null);
    }
  }
}