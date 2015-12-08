namespace Sitecore.FakeDb.ContentSearch
{
  using Sitecore.Abstractions;
  using Sitecore.Common;
  using Sitecore.ContentSearch;

  public class SwitchingSearchProvider : SearchProvider
  {
    static SwitchingSearchProvider()
    {
      // TODO: Workaround. Have to request the Locator property to get the internal field initialized.
      ContentSearchManager.Locator.GetInstance<ICorePipeline>();
    }

    public override string GetContextIndexName(IIndexable indexable)
    {
      var currentProvider = Switcher<SearchProvider>.CurrentValue;
      return currentProvider != null ? currentProvider.GetContextIndexName(indexable) : null;
    }

    public override string GetContextIndexName(IIndexable indexable, ICorePipeline pipeline)
    {
      var currentProvider = Switcher<SearchProvider>.CurrentValue;
      return currentProvider != null ? currentProvider.GetContextIndexName(indexable, pipeline) : null;
    }
  }
}