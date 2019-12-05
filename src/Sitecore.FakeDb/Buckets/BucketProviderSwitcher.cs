namespace Sitecore.FakeDb.Buckets
{
    using Sitecore.Buckets.Managers;
    using Sitecore.Common;

    /// <summary>
    /// Switches the <see cref="BucketProvider"/>, typically with a mocked instance.
    /// </summary>
    public class BucketProviderSwitcher : Switcher<BucketProvider>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BucketProviderSwitcher"/> class.
        /// </summary>
        /// <param name="bucketProviderToSwitchTo">The bucket provider to switch to.</param>
        public BucketProviderSwitcher(BucketProvider bucketProviderToSwitchTo)
            : base(bucketProviderToSwitchTo)
        {
        }
    }
}