namespace Sitecore.FakeDb.ContentSearch
{
  using Sitecore.Abstractions;
  using Sitecore.ContentSearch;

  public class FakeSearchProvider : SearchProvider
  {
    static FakeSearchProvider()
    {
      // TODO: Workaround. Have to request the Locator property to get the internal field initialized.
      ContentSearchManager.Locator.GetInstance<ICorePipeline>();
    }

    public override string GetContextIndexName(IIndexable indexable)
    {
      // TODO: Avoid the index name hardcoding.
      return "fake_index";
    }

    public override string GetContextIndexName(IIndexable indexable, ICorePipeline pipeline)
    {
      // TODO: Avoid the index name hardcoding.
      return "fake_index";
    }
  }
}