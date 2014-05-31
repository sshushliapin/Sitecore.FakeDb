namespace Sitecore.FakeDb.Tests.Links
{
  using FluentAssertions;
  using Xunit;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Links;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Links;
  using Sitecore.FakeDb.Data.Items;

  public class FakeLinkDatabaseTest
  {
    private readonly LinkDatabase behavior;

    private readonly FakeLinkDatabase _fakeLinkDatabase;

    private readonly Database database = Database.GetDatabase("master");

    private readonly Item item;

    private readonly ItemLink[] links = new ItemLink[0];

    public FakeLinkDatabaseTest()
    {
      this.item = ItemHelper.CreateInstance(this.database);

      this.behavior = Substitute.For<LinkDatabase>();
      this._fakeLinkDatabase = new FakeLinkDatabase {Behavior = this.behavior};
    }

    [Fact]
    public void ShouldReturnEmptyValuesWithoutBehaviorSet()
    {
      // arrange
      var linkDatabase = new FakeLinkDatabase();

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

    [Fact]
    public void ShouldSetBehaviour()
    {
      // assert
      this._fakeLinkDatabase.Behavior.Should().BeSameAs(this.behavior);
    }

    [Fact]
    public void ShouldCallCompact()
    {
      // act
      this._fakeLinkDatabase.Compact(this.database);

      // assert
      this.behavior.Received().Compact(this.database);
    }

    [Fact]
    public void ShouldGetBrokenLinks()
    {
      // arrange
      this.behavior.GetBrokenLinks(this.database).Returns(this.links);

      // act & assert
      this._fakeLinkDatabase.GetBrokenLinks(this.database).ShouldBeEquivalentTo(this.links);
    }

    [Fact]
    public void ShouldGetReferenceCount()
    {
      this.behavior.GetReferenceCount(this.item).Returns(1);

      // act
      this._fakeLinkDatabase.GetReferenceCount(this.item).Should().Be(1);
    }

    [Fact]
    public void ShouldGetReferences()
    {
      // arrange
      this.behavior.GetReferences(this.item).Returns(this.links);

      // act & assert
      this._fakeLinkDatabase.GetReferences(this.item).ShouldBeEquivalentTo(this.links);
    }

    [Fact]
    public void ShouldGetItemReferences()
    {
      // arrange
      this.behavior.GetItemReferences(this.item, false).Returns(this.links);

      // act & assert
      this._fakeLinkDatabase.GetItemReferences(this.item, false).ShouldBeEquivalentTo(this.links);
    }

    [Fact]
    public void ShouldGetReferrerCount()
    {
      this.behavior.GetReferrerCount(this.item).Returns(1);

      // act
      this._fakeLinkDatabase.GetReferrerCount(this.item).Should().Be(1);
    }

    [Fact]
    public void ShouldGetReferrers()
    {
      // arrange
      this.behavior.GetReferrers(this.item).Returns(this.links);

      // act & assert
      this._fakeLinkDatabase.GetReferrers(this.item).ShouldBeEquivalentTo(this.links);
    }

    [Fact]
    public void ShouldGetReferrersByFieldId()
    {
      // arrange
      this.behavior.GetReferrers(this.item, ID.Null).Returns(this.links);

      // act & assert
      this._fakeLinkDatabase.GetReferrers(this.item, ID.Null).ShouldBeEquivalentTo(this.links);
    }

    [Fact]
    public void ShouldGetItemReferrers()
    {
      // arrange
      this.behavior.GetItemReferrers(this.item, false).Returns(this.links);

      // act & assert
      this._fakeLinkDatabase.GetItemReferrers(this.item, false).ShouldBeEquivalentTo(this.links);
    }

    [Fact]
    public void ShouldGetItemVersionReferrers()
    {
      // arrange
      this.behavior.GetItemVersionReferrers(this.item).Returns(this.links);

      // act & assert
      this._fakeLinkDatabase.GetItemVersionReferrers(this.item).ShouldBeEquivalentTo(this.links);
    }

    [Fact]
    public void ShouldGetReferrersDeep()
    {
      // arrange
      this.behavior.GetReferrers(this.item, false).Returns(this.links);

      // act & assert
      this._fakeLinkDatabase.GetReferrers(this.item, false).ShouldBeEquivalentTo(this.links);
    }

    [Fact]
    public void ShouldCallHasExternalReferrers()
    {
      // arrange
      this.behavior.HasExternalReferrers(this.item, false).Returns(true);

      // act & assert
      this._fakeLinkDatabase.HasExternalReferrers(this.item, false).Should().BeTrue();
    }

    [Fact]
    public void ShouldCallRebuild()
    {
      // act
      this._fakeLinkDatabase.Rebuild(this.database);

      // assert
      this.behavior.Received().Rebuild(this.database);
    }

    [Fact]
    public void ShouldCallRemoveReferences()
    {
      // act
      this._fakeLinkDatabase.RemoveReferences(this.item);

      // assert
      this.behavior.Received().RemoveReferences(this.item);
    }

    [Fact]
    public void ShouldCallUpdateItemVersionReferences()
    {
      // act
      this._fakeLinkDatabase.UpdateItemVersionReferences(this.item);

      // assert
      this.behavior.Received().UpdateItemVersionReferences(this.item);
    }

    [Fact]
    public void ShouldCallUpdateReferences()
    {
      // act
      this._fakeLinkDatabase.UpdateReferences(this.item);

      // assert
      this.behavior.Received().UpdateReferences(this.item);
    }

  }
}