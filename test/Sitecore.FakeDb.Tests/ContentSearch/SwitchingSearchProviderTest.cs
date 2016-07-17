#if !SC72 && !SC80
namespace Sitecore.FakeDb.Tests.ContentSearch
{
  using FluentAssertions;
  using NSubstitute;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Abstractions;
  using Sitecore.Common;
  using Sitecore.ContentSearch;
  using Sitecore.FakeDb.ContentSearch;
  using Xunit;

  public class SwitchingSearchProviderTest
  {
    [Theory, AutoData]
    public void ShouldReturnNullIfCurrentProviderIsNull(SwitchingSearchProvider sut)
    {
      sut.GetContextIndexName(null).Should().BeNull();
    }

    [Theory, AutoData]
    public void ShouldReturnNullByIIndexableAndIPipelineIfCurrentProviderIsNull(SwitchingSearchProvider sut)
    {
      sut.GetContextIndexName(null, null).Should().BeNull();
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldReturnCurrentName(
      [Frozen] SearchProvider current,
      Switcher<SearchProvider> switcher,
      IIndexable indexable,
      string expected,
      SwitchingSearchProvider sut)
    {
      current.GetContextIndexName(indexable).Returns(expected);
      sut.GetContextIndexName(indexable).Should().Be(expected);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldReturnCurrentNameByIIndexableAndIPipeline(
      [Frozen] SearchProvider current,
      Switcher<SearchProvider> switcher,
      IIndexable indexable,
      ICorePipeline pipeline,
      string expected,
      SwitchingSearchProvider sut)
    {
      current.GetContextIndexName(indexable, pipeline).Returns(expected);
      sut.GetContextIndexName(indexable, pipeline).Should().Be(expected);
    }
  }
}
#endif