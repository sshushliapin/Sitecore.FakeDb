namespace Sitecore.FakeDb.Tests.Buckets
{
  using System;
  using System.Collections.Generic;
  using FluentAssertions;
  using NSubstitute;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Buckets.Managers;
  using Sitecore.ContentSearch.SearchTypes;
  using Sitecore.ContentSearch.Utilities;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Buckets;
  using Xunit;

  public class SwitchingBucketProviderTest
  {
    [Theory, DefaultSubstituteAutoData]
    public void ShouldBeBucketProvider(SwitchingBucketProvider sut)
    {
      sut.Should().BeAssignableTo<BucketProvider>();
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldCallAddSearchTabToItem([Frozen]BucketProvider current, BucketProviderSwitcher switcher, SwitchingBucketProvider sut, Item item)
    {
      sut.AddSearchTabToItem(item);
      current.Received().AddSearchTabToItem(item);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldCallCloneItemIntoBucket([Frozen]BucketProvider current, BucketProviderSwitcher switcher, SwitchingBucketProvider sut, Item source, Item target, bool deep)
    {
      sut.CloneItemIntoBucket(source, target, deep);
      current.Received().CloneItemIntoBucket(source, target, deep);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldCallCloneItem([Frozen]BucketProvider current, BucketProviderSwitcher switcher, SwitchingBucketProvider sut, Item source, Item target, bool deep, Item expected)
    {
      current.CloneItem(source, target, deep).Returns(expected);
      sut.CloneItem(source, target, deep).Should().BeSameAs(expected);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldCallCopyItemIntoBucket([Frozen]BucketProvider current, BucketProviderSwitcher switcher, SwitchingBucketProvider sut, Item source, Item target, bool deep)
    {
      sut.CopyItemIntoBucket(source, target, deep);
      current.Received().CopyItemIntoBucket(source, target, deep);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldCallCopyItem([Frozen]BucketProvider current, BucketProviderSwitcher switcher, SwitchingBucketProvider sut, Item source, Item target, bool deep)
    {
      sut.CopyItem(source, target, deep);
      current.Received().CopyItem(source, target, deep);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldCallCreateBucket([Frozen] BucketProvider current, BucketProviderSwitcher switcher, SwitchingBucketProvider sut, Item item, Action<Item> callBack)
    {
      sut.CreateBucket(item, callBack);
      current.Received().CreateBucket(item, callBack);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldCallGetFacets([Frozen] BucketProvider current, BucketProviderSwitcher switcher, SwitchingBucketProvider sut, List<SearchStringModel> searchQuery, string locationFilter, IList<IEnumerable<SitecoreUIFacet>> expected)
    {
      current.GetFacets(searchQuery, locationFilter).Returns(expected);
      sut.GetFacets(searchQuery, locationFilter).Should().BeSameAs(expected);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldGetEmptyFacetsIfNoCurrentProvider(SwitchingBucketProvider sut, List<SearchStringModel> searchQuery, string locationFilter, IList<IEnumerable<SitecoreUIFacet>> expected)
    {
      sut.GetFacets(searchQuery, locationFilter).Should().BeEmpty();
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldCallIsBucket([Frozen] BucketProvider current, BucketProviderSwitcher switcher, SwitchingBucketProvider sut, Item item)
    {
      current.IsBucket(item).Returns(true);
      sut.IsBucket(item).Should().BeTrue();
    }

    [Theory, DefaultSubstituteAutoData]
    public void IsBucketReturnsFalseIfNoCurrentProvider(SwitchingBucketProvider sut, Item item)
    {
      sut.IsBucket(item).Should().BeFalse();
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldCallIsItemContainedWithinBucket([Frozen] BucketProvider current, BucketProviderSwitcher switcher, SwitchingBucketProvider sut, Item item)
    {
      current.IsItemContainedWithinBucket(item).Returns(true);
      sut.IsItemContainedWithinBucket(item).Should().BeTrue();
    }

    [Theory, DefaultSubstituteAutoData]
    public void IsItemContainedWithinBucketReturnsFalseIfNoCurrentProvider(SwitchingBucketProvider sut, Item item)
    {
      sut.IsItemContainedWithinBucket(item).Should().BeFalse();
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldCallIsTemplateBucketable([Frozen] BucketProvider current, BucketProviderSwitcher switcher, SwitchingBucketProvider sut, ID templateId, Database database)
    {
      current.IsTemplateBucketable(templateId, database).Returns(true);
      sut.IsTemplateBucketable(templateId, database).Should().BeTrue();
    }

    [Theory, DefaultSubstituteAutoData]
    public void IsTemplateBucketableReturnsFalseIfNoCurrentProvider(SwitchingBucketProvider sut, ID tempalteId, Database database)
    {
      sut.IsTemplateBucketable(tempalteId, database).Should().BeFalse();
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldCallMoveItemIntoBucket([Frozen] BucketProvider current, BucketProviderSwitcher switcher, SwitchingBucketProvider sut, Item source, Item target)
    {
      sut.MoveItemIntoBucket(source, target);
      current.Received().MoveItemIntoBucket(source, target);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldCallRemoveBucket([Frozen] BucketProvider current, BucketProviderSwitcher switcher, SwitchingBucketProvider sut, Item item)
    {
      sut.RemoveBucket(item);
      current.Received().RemoveBucket(item);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldCallSyncBucket([Frozen] BucketProvider current, BucketProviderSwitcher switcher, SwitchingBucketProvider sut, Item item)
    {
      sut.SyncBucket(item);
      current.Received().SyncBucket(item);
    }
  }
}