namespace Sitecore.FakeDb.Tests.Links
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.FakeDb.Links;
  using Sitecore.Links;
  using System;
  using Xunit;

  public class FakeLinkDatabaseTest : IDisposable
  {
    private readonly LinkDatabase behavior;

    private readonly FakeLinkDatabase linkDatabase;

    private readonly Database database = Database.GetDatabase("master");

    private readonly Item item;

    private readonly ItemLink[] links = new ItemLink[0];

    public FakeLinkDatabaseTest()
    {
      this.item = ItemHelper.CreateInstance(this.database);

      this.behavior = Substitute.For<LinkDatabase>();
      this.linkDatabase = new FakeLinkDatabase();
      this.linkDatabase.LocalProvider.Value = this.behavior;
    }

    [Fact]
    public void ShouldReturnEmptyValuesWithoutBehaviorSet()
    {
      // arrange
      var stubLinkDatabase = new FakeLinkDatabase();

      // act & assert
      stubLinkDatabase.Compact(null);
      stubLinkDatabase.GetBrokenLinks(null).Should().BeEmpty();
      stubLinkDatabase.GetReferenceCount(null).Should().Be(0);
      stubLinkDatabase.GetReferences(null).Should().BeEmpty();
      stubLinkDatabase.GetItemReferences(null, false).Should().BeEmpty();
      stubLinkDatabase.GetReferrerCount(null).Should().Be(0);
      stubLinkDatabase.GetReferrers(null).Should().BeEmpty();
      stubLinkDatabase.GetReferrers(null, null).Should().BeEmpty();
      stubLinkDatabase.GetItemReferrers(null, false).Should().BeEmpty();
      stubLinkDatabase.GetItemVersionReferrers(null).Should().BeEmpty();
      stubLinkDatabase.GetReferrers(null).Should().BeEmpty();
      stubLinkDatabase.HasExternalReferrers(null, false).Should().BeFalse();
      stubLinkDatabase.Rebuild(null);
      stubLinkDatabase.RemoveReferences(null);
      stubLinkDatabase.UpdateItemVersionReferences(null);
      stubLinkDatabase.UpdateReferences(null);
    }

    [Fact]
    public void ShouldSetBehaviour()
    {
      // assert
      this.linkDatabase.LocalProvider.Value.Should().BeSameAs(this.behavior);
    }

    [Fact]
    public void ShouldCallCompact()
    {
      // act
      this.linkDatabase.Compact(this.database);

      // assert
      this.behavior.Received().Compact(this.database);
    }

    [Fact]
    public void ShouldGetBrokenLinks()
    {
      // arrange
      this.behavior.GetBrokenLinks(this.database).Returns(this.links);

      // act & assert
      this.linkDatabase.GetBrokenLinks(this.database).ShouldBeEquivalentTo(this.links);
    }

    [Fact]
    public void ShouldGetReferenceCount()
    {
      this.behavior.GetReferenceCount(this.item).Returns(1);

      // act
      this.linkDatabase.GetReferenceCount(this.item).Should().Be(1);
    }

    [Fact]
    public void ShouldGetReferences()
    {
      // arrange
      this.behavior.GetReferences(this.item).Returns(this.links);

      // act & assert
      this.linkDatabase.GetReferences(this.item).ShouldBeEquivalentTo(this.links);
    }

    [Fact]
    public void ShouldGetItemReferences()
    {
      // arrange
      this.behavior.GetItemReferences(this.item, false).Returns(this.links);

      // act & assert
      this.linkDatabase.GetItemReferences(this.item, false).ShouldBeEquivalentTo(this.links);
    }

    [Fact]
    public void ShouldGetReferrerCount()
    {
      this.behavior.GetReferrerCount(this.item).Returns(1);

      // act
      this.linkDatabase.GetReferrerCount(this.item).Should().Be(1);
    }

    [Fact]
    public void ShouldGetReferrers()
    {
      // arrange
      this.behavior.GetReferrers(this.item).Returns(this.links);

      // act & assert
      this.linkDatabase.GetReferrers(this.item).ShouldBeEquivalentTo(this.links);
    }

    [Fact]
    public void ShouldGetReferrersByFieldId()
    {
      // arrange
      this.behavior.GetReferrers(this.item, ID.Null).Returns(this.links);

      // act & assert
      this.linkDatabase.GetReferrers(this.item, ID.Null).ShouldBeEquivalentTo(this.links);
    }

    [Fact]
    public void ShouldGetItemReferrers()
    {
      // arrange
      this.behavior.GetItemReferrers(this.item, false).Returns(this.links);

      // act & assert
      this.linkDatabase.GetItemReferrers(this.item, false).ShouldBeEquivalentTo(this.links);
    }

    [Fact]
    public void ShouldGetItemVersionReferrers()
    {
      // arrange
      this.behavior.GetItemVersionReferrers(this.item).Returns(this.links);

      // act & assert
      this.linkDatabase.GetItemVersionReferrers(this.item).ShouldBeEquivalentTo(this.links);
    }

    [Fact]
    [Obsolete]
    public void ShouldGetReferrersDeep()
    {
      // arrange
      this.behavior.GetReferrers(this.item, false).Returns(this.links);

      // act & assert
      this.linkDatabase.GetReferrers(this.item, false).ShouldBeEquivalentTo(this.links);
    }

    [Fact]
    public void ShouldCallHasExternalReferrers()
    {
      // arrange
      this.behavior.HasExternalReferrers(this.item, false).Returns(true);

      // act & assert
      this.linkDatabase.HasExternalReferrers(this.item, false).Should().BeTrue();
    }

    [Fact]
    public void ShouldCallRebuild()
    {
      // act
      this.linkDatabase.Rebuild(this.database);

      // assert
      this.behavior.Received().Rebuild(this.database);
    }

    [Fact]
    public void ShouldCallRemoveReferences()
    {
      // act
      this.linkDatabase.RemoveReferences(this.item);

      // assert
      this.behavior.Received().RemoveReferences(this.item);
    }

    [Fact]
    public void ShouldCallUpdateItemVersionReferences()
    {
      // act
      this.linkDatabase.UpdateItemVersionReferences(this.item);

      // assert
      this.behavior.Received().UpdateItemVersionReferences(this.item);
    }

    [Fact]
    public void ShouldCallUpdateReferences()
    {
      // act
      this.linkDatabase.UpdateReferences(this.item);

      // assert
      this.behavior.Received().UpdateReferences(this.item);
    }

    public void Dispose()
    {
      this.linkDatabase.Dispose();
    }
  }
}