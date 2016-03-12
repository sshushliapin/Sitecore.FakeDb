namespace Sitecore.FakeDb.Tests.Buckets
{
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
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
    public void SutSwitchesSwitcherCurrentValue([Frozen]BucketProvider provider, BucketProviderSwitcher sut)
    {
      BucketProviderSwitcher.CurrentValue.Should().BeSameAs(provider);
    }
  }
}
