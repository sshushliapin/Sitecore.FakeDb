namespace Sitecore.FakeDb.Tests.Buckets
{
    using FluentAssertions;
    using NSubstitute;

    using Ploeh.AutoFixture.Xunit2;

    using Sitecore.Buckets.Managers;
    using Sitecore.Common;
    using Sitecore.FakeDb.Buckets;
    using Sitecore.FakeDb.Links;
    using Sitecore.Links;
    using Xunit;

    using BucketProviderSwitcher = Sitecore.FakeDb.Buckets.BucketProviderSwitcher;

    public class BucketProviderSwitcherTest
    {
        [Theory, AutoData]
        public void SutIsSwitcher(BucketProviderSwitcher sut)
        {
            sut.Should().BeAssignableTo<Switcher<PipelineBasedBucketProvider>>();
        }

        [Theory, AutoData]
        public void SutSwitchesSwitcherCurrentValue([Frozen]PipelineBasedBucketProvider provider, BucketProviderSwitcher sut)
        {
            BucketProviderSwitcher.CurrentValue.Should().BeSameAs(provider);
        }

#if SC80 || SC81
        [Theory, AutoData]
        public void SutSwitchesSwitcherSitecoreBucketProvider([Frozen]PipelineBasedBucketProvider provider, BucketProviderSwitcher sut)
        {
            BucketManager.Provider.Name.Should().Be("switcher");
        }
#endif

        [Theory, AutoData]
        public void DisposeRestoresPreviousSitecoreBucketProvider([Frozen]PipelineBasedBucketProvider provider, BucketProviderSwitcher sut)
        {
            sut.Dispose();
            BucketManager.Provider.Name.Should().Be("sitecore");
        }
    }
}
