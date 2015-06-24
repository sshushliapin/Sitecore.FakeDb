namespace Sitecore.FakeDb.Tests.Samples
{
  using System.Collections.Generic;
  using System.Linq;
  using NSubstitute;
  using Sitecore.ContentSearch;
  using Sitecore.ContentSearch.SearchTypes;
  using Xunit;

  /// <summary>
  /// Inspired by http://ctor.io/create-simple-unit-tests-for-the-sitecore-content-search/?utm_campaign=twitter&utm_medium=twitter&utm_source=twitter
  /// </summary>
  public class ContentSearchSamples
  {
    [Fact]
    public void ShouldGetProducts()
    {
      // arrange
      var searchIndex = Substitute.For<ISearchIndex>();
      searchIndex
        .CreateSearchContext()
        .GetQueryable<ProductSearchResultItem>()
        .Returns(new[]
                {
                  new ProductSearchResultItem { Free = true }, 
                  new ProductSearchResultItem { Free = false },
                  new ProductSearchResultItem { Free = true }
                }.AsQueryable());

      ContentSearchManager.SearchConfiguration.Indexes["fake_index"] = searchIndex;

      var repository = new SearchRepository();

      // act
      var products = repository.GetProducts(null);

      // assert
      Assert.Equal(2, products.Count());
    }

    public class SearchRepository
    {
      public virtual IEnumerable<ProductSearchResultItem> GetProducts(SitecoreIndexableItem item)
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
  }
}