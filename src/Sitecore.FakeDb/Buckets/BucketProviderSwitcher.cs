namespace Sitecore.FakeDb.Buckets
{
  using Sitecore.Buckets.Managers;
  using Sitecore.Common;

  public class BucketProviderSwitcher : Switcher<BucketProvider>
  {
    public BucketProviderSwitcher(BucketProvider bucketProviderToSwitchTo)
      : base(bucketProviderToSwitchTo)
    {
    }
  }
}
