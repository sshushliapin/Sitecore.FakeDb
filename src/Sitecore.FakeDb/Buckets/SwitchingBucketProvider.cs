namespace Sitecore.FakeDb.Buckets
{
    using System;
    using System.Collections.Generic;

    using Sitecore.Buckets.Managers;
    using Sitecore.Common;
    using Sitecore.ContentSearch.SearchTypes;
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Data;
    using Sitecore.Data.Items;

    public class SwitchingBucketProvider : PipelineBasedBucketProvider
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
            else
            {
                base.AddSearchTabToItem(item);
            }

        }

        public override Item CloneItem([NotNull] Item source, [NotNull] Item target, bool deep)
        {
            var current = this.CurrentProvider;
            return current != null ? current.CloneItem(source, target, deep) : base.CloneItem(source, target, deep);
        }

        public override void CloneItemIntoBucket(Item source, Item target, bool deep)
        {
            var current = this.CurrentProvider;
            if (current != null)
            {
                current.CloneItemIntoBucket(source, target, deep);
            }
            else
            {
                base.CloneItemIntoBucket(source, target, deep);
            }
        }

        public override void CopyItemIntoBucket(Item source, Item target, bool deep)
        {
            var current = this.CurrentProvider;
            if (current != null)
            {
                current.CopyItemIntoBucket(source, target, deep);
            }
            else
            {
                base.CopyItemIntoBucket(source, target, deep);
            }
        }

        public override void CreateBucket(Item item, Action<Item> callBack)
        {
            var current = this.CurrentProvider;
            if (current != null)
            {
                current.CreateBucket(item, callBack);
            }
            else
            {
                base.CreateBucket(item, callBack);
            }
        }

        public override IEnumerable<IEnumerable<SitecoreUIFacet>> GetFacets(List<SearchStringModel> searchQuery, string locationFilter)
        {
            var current = this.CurrentProvider;
            return current != null ? current.GetFacets(searchQuery, locationFilter) : base.GetFacets(searchQuery, locationFilter);
        }

        public override bool IsBucket(Item item)
        {
            var current = this.CurrentProvider;
            return current != null ? current.IsBucket(item) : base.IsBucket(item);
        }

        public override bool IsItemContainedWithinBucket(Item item)
        {
            var current = this.CurrentProvider;
            return current != null ? current.IsItemContainedWithinBucket(item) : base.IsItemContainedWithinBucket(item);
        }

        public override bool IsTemplateBucketable(ID templateId, Database database)
        {
            var current = this.CurrentProvider;
            return current != null ? current.IsTemplateBucketable(templateId, database) : base.IsTemplateBucketable(templateId, database);
        }

        public override void MoveItemIntoBucket(Item source, Item target)
        {
            var current = this.CurrentProvider;
            if (current != null)
            {
                current.MoveItemIntoBucket(source, target);
            }
            else
            {
                base.MoveItemIntoBucket(source, target);
            }
        }

        public override void RemoveBucket(Item item)
        {
            var current = this.CurrentProvider;
            if (current != null)
            {
                current.RemoveBucket(item);
            }
            else
            {
                base.RemoveBucket(item);
            }
        }

        public override void SyncBucket(Item item)
        {
            var current = this.CurrentProvider;
            if (current != null)
            {
                current.RemoveBucket(item);
            }
            else
            {
                base.RemoveBucket(item);
            }
        }

        public override Item CopyItem(Item target, Item source, bool deep)
        {
            var current = this.CurrentProvider;
            return current != null ? current.CopyItem(target, source, deep) : base.CopyItem(target, source, deep);

        }
    }
}
