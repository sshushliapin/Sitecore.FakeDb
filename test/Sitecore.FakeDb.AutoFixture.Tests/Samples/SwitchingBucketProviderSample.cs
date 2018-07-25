namespace Sitecore.FakeDb.AutoFixture.Tests.Samples
{
  using NSubstitute;
  using global::AutoFixture;
  using global::AutoFixture.AutoNSubstitute;
  using global::AutoFixture.Xunit2;
  using Sitecore.Buckets.Managers;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.AutoFixture;
  using Sitecore.FakeDb.Buckets;
  using Xunit;

  // NuGet packages required:
  // PM> Install-Package xunit
  // PM> Install-Package NSubstitute
  // PM> Install-Package AutoFixture.Xunit2
  // PM> Install-Package AutoFixture.AutoNSubstitute
  // PM> Install-Package Sitecore.FakeDb.AutoFixture
  public class SwitchingBucketProviderSample
  {
    [Theory, DefaultAutoData]
    public void SwitchBucketProvider([Frozen]BucketProvider provider, BucketProviderSwitcher switcher, Item source, Item target)
    {
      BucketManager.MoveItemIntoBucket(source, target);
      provider.Received().MoveItemIntoBucket(source, target);
    }

    private class DefaultAutoDataAttribute : AutoDataAttribute
    {
      public DefaultAutoDataAttribute()
        : base(() => new Fixture().Customize(new AutoNSubstituteCustomization())
                            .Customize(new AutoDbCustomization()))
      {
      }
    }
  }
}
