namespace Sitecore.FakeDb.ContentSearch
{
    using System;
    using Sitecore.Abstractions;
    using Sitecore.Common;
    using Sitecore.ContentSearch;

    public class SwitchingSearchProvider : SearchProvider
    {
        public override string GetContextIndexName(IIndexable indexable)
        {
            var currentProvider = Switcher<SearchProvider>.CurrentValue;
            return currentProvider != null ? currentProvider.GetContextIndexName(indexable) : null;
        }

        [Obsolete("ICorePipeline is obsolete in Sitecore")]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        public override string GetContextIndexName(IIndexable indexable, ICorePipeline pipeline)
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
        {
            var currentProvider = Switcher<SearchProvider>.CurrentValue;
            return currentProvider?.GetContextIndexName(indexable, pipeline);
        }
    }
}