namespace Sitecore.FakeDb.Buckets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore.Buckets.Managers;
    using Sitecore.Common;
    using Sitecore.ContentSearch.SearchTypes;
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Data;
    using Sitecore.Data.Items;

    /// <summary>
    /// Delegates calls to a mocked <see cref="BucketProvider"/> if it is set using 
    /// the <see cref="BucketProviderSwitcher"/>. Otherwise behaves like a stub.
    /// </summary>
    public class SwitchingBucketProvider : BucketProvider
    {
        public BucketProvider CurrentProvider
        {
            get { return Switcher<BucketProvider>.CurrentValue; }
        }

        public override void AddSearchTabToItem(Item item)
        {
            var current = this.CurrentProvider;
            if (current != null)
            {
                current.AddSearchTabToItem(item);
            }
        }

        public override void CloneItemIntoBucket(Item source, Item target, bool deep)
        {
            var current = this.CurrentProvider;
            if (current != null)
            {
                current.CloneItemIntoBucket(source, target, deep);
            }
        }

        public override Item CloneItem([NotNull] Item source, [NotNull] Item target, bool deep)
        {
            var current = this.CurrentProvider;
            return current != null ? current.CloneItem(source, target, deep) : base.CloneItem(source, target, deep);
        }

        public override void CopyItemIntoBucket(Item source, Item target, bool deep)
        {
            var current = this.CurrentProvider;
            if (current != null)
            {
                current.CopyItemIntoBucket(source, target, deep);
            }
        }

        public override Item CopyItem(Item target, Item source, bool deep)
        {
            var current = this.CurrentProvider;
            return current != null ? current.CopyItem(target, source, deep) : base.CopyItem(target, source, deep);
        }

        public override void CreateBucket(Item item, Action<Item> callBack)
        {
            var current = this.CurrentProvider;
            if (current != null)
            {
                current.CreateBucket(item, callBack);
            }
        }

        public override IEnumerable<IEnumerable<SitecoreUIFacet>> GetFacets(List<SearchStringModel> searchQuery, string locationFilter)
        {
            var current = this.CurrentProvider;
            return current != null ? current.GetFacets(searchQuery, locationFilter) : Enumerable.Empty<IEnumerable<SitecoreUIFacet>>();
        }

        public override bool IsBucket(Item item)
        {
            var current = this.CurrentProvider;
            return current != null && current.IsBucket(item);
        }

        public override bool IsItemContainedWithinBucket(Item item)
        {
            var current = this.CurrentProvider;
            return current != null && current.IsItemContainedWithinBucket(item);
        }

        public override bool IsTemplateBucketable(ID templateId, Database database)
        {
            var current = this.CurrentProvider;
            return current != null && current.IsTemplateBucketable(templateId, database);
        }

        public override void MoveItemIntoBucket(Item source, Item target)
        {
            var current = this.CurrentProvider;
            if (current != null)
            {
                current.MoveItemIntoBucket(source, target);
            }
        }

        public override void RemoveBucket(Item item)
        {
            var current = this.CurrentProvider;
            if (current != null)
            {
                current.RemoveBucket(item);
            }
        }

        public override void SyncBucket(Item item)
        {
            var current = this.CurrentProvider;
            if (current != null)
            {
                current.SyncBucket(item);
            }
        }
    }
}