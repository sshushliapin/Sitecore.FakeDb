namespace Sitecore.FakeDb.Pipelines.Initialize
{
    using Sitecore.Pipelines;
    using Sitecore.SecurityModel.License;

    /// <summary>
    /// This processor disables the <see cref="LicenseWatcher"/> creating it in 
    /// advance before the default one and calling the 'Dispose()' method.
    /// That is required to stop the 'worker' process which may run forever in
    /// some test runners preventing them from completing. 
    /// </summary>
    public class DisableLicenseWatcher
    {
        /// <summary>
        /// Disables the <see cref="LicenseWatcher"/>.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void Process(PipelineArgs args)
        {
            new LicenseWatcher().Dispose();
        }
    }
}