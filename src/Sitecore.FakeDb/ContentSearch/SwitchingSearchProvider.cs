namespace Sitecore.FakeDb.ContentSearch
{
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

        public override string GetContextIndexName(IIndexable indexable, BaseCorePipelineManager pipeline)
        {
            var currentProvider = Switcher<SearchProvider>.CurrentValue;
            return currentProvider?.GetContextIndexName(indexable, pipeline);
        }
    }
}
