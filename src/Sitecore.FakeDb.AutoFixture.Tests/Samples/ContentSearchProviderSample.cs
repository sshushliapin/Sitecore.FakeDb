namespace Sitecore.FakeDb.AutoFixture.Tests.Samples
{
  using System.Collections.Generic;
  using System.Linq;
  using NSubstitute;
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.AutoNSubstitute;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Abstractions;
  using Sitecore.Common;
  using Sitecore.ContentSearch;
  using Sitecore.ContentSearch.SearchTypes;
  using Xunit;

  /// <summary>
  /// Inspired by http://ctor.io/create-simple-unit-tests-for-the-sitecore-content-search/?utm_campaign=twitter&utm_medium=twitter&utm_source=twitter
  /// </summary>
  public class ContentSearchProviderSample
  {
    [Theory, DefaultAutoData]
    public void ShouldGetFreeProducts(
      SearchRepository sut,
      ISearchIndex searchIndex,
      IIndexable indexable,
      [Frozen]SearchProvider provider,
      Switcher<SearchProvider> switcher)
    {
      // arrange
      searchIndex
        .CreateSearchContext()
        .GetQueryable<ProductSearchResultItem>()
        .Returns(new[]
          {
            new ProductSearchResultItem { Free = true },
            new ProductSearchResultItem { Free = false },
            new ProductSearchResultItem { Free = true }
          }.AsQueryable());

      ContentSearchManager.SearchConfiguration.Indexes["indexName"] = searchIndex;
      provider.GetContextIndexName(indexable, Arg.Any<ICorePipeline>()).Returns("indexName");

      // act
      var products = sut.GetProducts(indexable);

      // assert
      Assert.Equal(2, products.Count());
    }

    public class SearchRepository
    {
      public virtual IEnumerable<ProductSearchResultItem> GetProducts(IIndexable item)
      {
        using (var context = ContentSearchManager.CreateSearchContext(item))
        {
          var query = context.GetQueryable<ProductSearchResultItem>();

          var products = query.Where(searchResultItem => searchResultItem.Free);

          return products.ToList();
        }
      }
    }

    public class ProductSearchResultItem : SearchResultItem
    {
      [IndexField("free")]
      public virtual bool Free { get; set; }
    }

    private class DefaultAutoDataAttribute : AutoDataAttribute
    {
      public DefaultAutoDataAttribute()
        : base(new Fixture().Customize(new AutoNSubstituteCustomization()))
      {
      }
    }
  }
}