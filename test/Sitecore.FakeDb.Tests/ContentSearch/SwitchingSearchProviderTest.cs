namespace Sitecore.FakeDb.Tests.ContentSearch
{
    using FluentAssertions;
    using NSubstitute;
    using global::AutoFixture.Xunit2;
    using Sitecore.Abstractions;
    using Sitecore.Common;
    using Sitecore.ContentSearch;
    using Sitecore.FakeDb.ContentSearch;
    using Xunit;

    public class SwitchingSearchProviderTest
    {
        [Theory, AutoData]
        public void ShouldReturnNullIfCurrentProviderIsNull(
            SwitchingSearchProvider sut)
        {
            sut.GetContextIndexName(null).Should().BeNull();
        }

        [Theory, AutoData]
        public void ShouldReturnNullByIIndexableAndIPipelineIfCurrentProviderIsNull(
            SwitchingSearchProvider sut)
        {
            sut.GetContextIndexName(null, null).Should().BeNull();
        }

        [Theory, DefaultSubstituteAutoData]
        public void ShouldReturnCurrentName(
            [Frozen] SearchProvider current,
            IIndexable indexable,
            string expected,
            SwitchingSearchProvider sut)
        {
            using (new Switcher<SearchProvider>(current))
            {
                current.GetContextIndexName(indexable).Returns(expected);
                sut.GetContextIndexName(indexable).Should().Be(expected);
            }
        }

        [Theory, DefaultSubstituteAutoData]
        public void ShouldReturnCurrentNameByIIndexableAndIPipeline(
            [Frozen] SearchProvider current,
            IIndexable indexable,
            BaseCorePipelineManager pipeline,
            string expected,
            SwitchingSearchProvider sut)
        {
            using (new Switcher<SearchProvider>(current))
            {
                current.GetContextIndexName(indexable, pipeline).Returns(expected);
                sut.GetContextIndexName(indexable, pipeline).Should().Be(expected);
            }
        }
    }
}
