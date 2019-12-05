namespace Sitecore.FakeDb.Tests.Buckets
{
    using FluentAssertions;
    using Sitecore.Buckets.Managers;
    using Sitecore.Common;
    using Sitecore.FakeDb.Buckets;
    using Xunit;

    public class BucketProviderSwitcherTest
    {
        [Theory, DefaultSubstituteAutoData]
        public void SutIsSwitcher(BucketProviderSwitcher sut)
        {
            sut.Should().BeAssignableTo<Switcher<BucketProvider>>();
        }

        [Theory, DefaultSubstituteAutoData]
        public void SutSwitchesSwitcherCurrentValue(
            BucketProvider expected)
        {
            using (new BucketProviderSwitcher(expected))
            {
                BucketProviderSwitcher.CurrentValue.Should().BeSameAs(expected);
            }
        }
    }
}