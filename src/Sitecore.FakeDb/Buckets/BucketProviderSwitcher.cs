namespace Sitecore.FakeDb.Buckets
{
    using Sitecore.Buckets.Managers;
    using Sitecore.Common;

    public class BucketProviderSwitcher : Switcher<PipelineBasedBucketProvider>
    {
        public BucketProviderSwitcher(PipelineBasedBucketProvider bucketProviderToSwitchTo)
            : base(bucketProviderToSwitchTo)
        {
        }
    }
}
