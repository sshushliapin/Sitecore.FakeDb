namespace Sitecore.FakeDb.Tests.Links
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.FakeDb.Links;
  using Sitecore.Links;
  using Xunit;

  public class SwitchingLinkDatabaseTest
  {
    private readonly LinkDatabase behavior;

    private readonly SwitchingLinkDatabase linkDatabase;

    private readonly Database database = Database.GetDatabase("master");

    private readonly Item item;

    private readonly ItemLink[] links = new ItemLink[0];

    public SwitchingLinkDatabaseTest()
    {
      this.item = ItemHelper.CreateInstance(this.database);

      this.behavior = Substitute.For<LinkDatabase>();
      this.linkDatabase = new SwitchingLinkDatabase { Behavior = this.behavior };
    }

    [Fact]
    public void ShouldSetBehaviour()
    {
      // assert
      this.linkDatabase.Behavior.Should().BeSameAs(this.behavior);
    }

    [Fact]
    public void ShouldResetBehaviourOnDispose()
    {
      // act
      this.linkDatabase.Dispose();

      // assert
      this.linkDatabase.Behavior.Should().BeNull();
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
  }
}