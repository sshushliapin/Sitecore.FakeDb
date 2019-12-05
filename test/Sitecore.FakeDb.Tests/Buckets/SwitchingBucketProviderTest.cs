namespace Sitecore.FakeDb.Tests.Buckets
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using NSubstitute;
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
        public void ShouldCallAddSearchTabToItem(
            BucketProvider current,
            SwitchingBucketProvider sut,
            Item item)
        {
            using (new BucketProviderSwitcher(current))
            {
                sut.AddSearchTabToItem(item);
                current.Received().AddSearchTabToItem(item);
            }
        }

        [Theory, DefaultSubstituteAutoData]
        public void ShouldCallCloneItemIntoBucket(
            BucketProvider current,
            SwitchingBucketProvider sut,
            Item source,
            Item target,
            bool deep)
        {
            using (new BucketProviderSwitcher(current))
            {
                sut.CloneItemIntoBucket(source, target, deep);
                current.Received().CloneItemIntoBucket(source, target, deep);
            }
        }

        [Theory, DefaultSubstituteAutoData]
        public void ShouldCallCloneItem(
            BucketProvider current,
            SwitchingBucketProvider sut,
            Item source,
            Item target,
            bool deep,
            Item expected)
        {
            using (new BucketProviderSwitcher(current))
            {
                current.CloneItem(source, target, deep).Returns(expected);
                sut.CloneItem(source, target, deep).Should().BeSameAs(expected);
            }
        }

        [Theory, DefaultSubstituteAutoData]
        public void ShouldCallCopyItemIntoBucket(
            BucketProvider current,
            SwitchingBucketProvider sut,
            Item source,
            Item target,
            bool deep)
        {
            using (new BucketProviderSwitcher(current))
            {
                sut.CopyItemIntoBucket(source, target, deep);
                current.Received().CopyItemIntoBucket(source, target, deep);
            }
        }

        [Theory, DefaultSubstituteAutoData]
        public void ShouldCallCopyItem(
            BucketProvider current,
            SwitchingBucketProvider sut,
            Item source,
            Item target,
            bool deep)
        {
            using (new BucketProviderSwitcher(current))
            {
                sut.CopyItem(source, target, deep);
                current.Received().CopyItem(source, target, deep);
            }
        }

        [Theory, DefaultSubstituteAutoData]
        public void ShouldCallCreateBucket(
            BucketProvider current,
            SwitchingBucketProvider sut,
            Item item,
            Action<Item> callBack)
        {
            using (new BucketProviderSwitcher(current))
            {
                sut.CreateBucket(item, callBack);
                current.Received().CreateBucket(item, callBack);
            }
        }

        [Theory, DefaultSubstituteAutoData]
        public void ShouldCallGetFacets(
            BucketProvider current,
            SwitchingBucketProvider sut,
            List<SearchStringModel> searchQuery,
            string locationFilter,
            IList<IEnumerable<SitecoreUIFacet>> expected)
        {
            using (new BucketProviderSwitcher(current))
            {
                current.GetFacets(searchQuery, locationFilter).Returns(expected);
                sut.GetFacets(searchQuery, locationFilter).Should().BeSameAs(expected);
            }
        }

        [Theory, DefaultSubstituteAutoData]
        public void ShouldGetEmptyFacetsIfNoCurrentProvider(
            SwitchingBucketProvider sut,
            List<SearchStringModel> searchQuery,
            string locationFilter)
        {
            sut.GetFacets(searchQuery, locationFilter).Should().BeEmpty();
        }

        [Theory, DefaultSubstituteAutoData]
        public void ShouldCallIsBucket(
            BucketProvider current,
            SwitchingBucketProvider sut,
            Item item)
        {
            using (new BucketProviderSwitcher(current))
            {
                current.IsBucket(item).Returns(true);
                sut.IsBucket(item).Should().BeTrue();
            }
        }

        [Theory, DefaultSubstituteAutoData]
        public void IsBucketReturnsFalseIfNoCurrentProvider(SwitchingBucketProvider sut, Item item)
        {
            sut.IsBucket(item).Should().BeFalse();
        }

        [Theory, DefaultSubstituteAutoData]
        public void ShouldCallIsItemContainedWithinBucket(
            BucketProvider current,
            SwitchingBucketProvider sut,
            Item item)
        {
            using (new BucketProviderSwitcher(current))
            {
                current.IsItemContainedWithinBucket(item).Returns(true);
                sut.IsItemContainedWithinBucket(item).Should().BeTrue();
            }
        }

        [Theory, DefaultSubstituteAutoData]
        public void IsItemContainedWithinBucketReturnsFalseIfNoCurrentProvider(
            SwitchingBucketProvider sut,
            Item item)
        {
            sut.IsItemContainedWithinBucket(item).Should().BeFalse();
        }

        [Theory, DefaultSubstituteAutoData]
        public void ShouldCallIsTemplateBucketable(
            BucketProvider current,
            SwitchingBucketProvider sut,
            ID templateId,
            Database database)
        {
            using (new BucketProviderSwitcher(current))
            {
                current.IsTemplateBucketable(templateId, database).Returns(true);
                sut.IsTemplateBucketable(templateId, database).Should().BeTrue();
            }
        }

        [Theory, DefaultSubstituteAutoData]
        public void IsTemplateBucketableReturnsFalseIfNoCurrentProvider(
            SwitchingBucketProvider sut,
            ID tempalteId,
            Database database)
        {
            sut.IsTemplateBucketable(tempalteId, database).Should().BeFalse();
        }

        [Theory, DefaultSubstituteAutoData]
        public void ShouldCallMoveItemIntoBucket(
            BucketProvider current,
            SwitchingBucketProvider sut,
            Item source,
            Item target)
        {
            using (new BucketProviderSwitcher(current))
            {
                sut.MoveItemIntoBucket(source, target);
                current.Received().MoveItemIntoBucket(source, target);
            }
        }

        [Theory, DefaultSubstituteAutoData]
        public void ShouldCallRemoveBucket(
            BucketProvider current,
            SwitchingBucketProvider sut,
            Item item)
        {
            using (new BucketProviderSwitcher(current))
            {
                sut.RemoveBucket(item);
                current.Received().RemoveBucket(item);
            }
        }

        [Theory, DefaultSubstituteAutoData]
        public void ShouldCallSyncBucket(
            BucketProvider current,
            SwitchingBucketProvider sut,
            Item item)
        {
            using (new BucketProviderSwitcher(current))
            {
                sut.SyncBucket(item);
                current.Received().SyncBucket(item);
            }
        }
    }
}