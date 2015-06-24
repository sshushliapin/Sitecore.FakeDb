namespace Sitecore.FakeDb.Tests.ContentSearch
{
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.ContentSearch;
  using Sitecore.FakeDb.ContentSearch;
  using Xunit;

  public class FakeSearchProviderTest
  {
    [Theory, AutoData]
    public void ShouldReturnNullContextIndexName(FakeSearchProvider provider)
    {
      provider.GetContextIndexName(null).Should().Be("fake_index");
    }

    [Theory, AutoData]
    public void ShouldReturnNullContextIndexNameByIIndexableAndIPipeline(FakeSearchProvider provider)
    {
      provider.GetContextIndexName(null, null).Should().Be("fake_index");
    }
  }
}