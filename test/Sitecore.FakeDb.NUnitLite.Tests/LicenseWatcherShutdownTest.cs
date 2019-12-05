namespace Sitecore.FakeDb.NUnitLite.Tests
{
    using NUnit.Framework;
    using Sitecore.FakeDb;

    /// <summary>
    /// Test for issue https://github.com/sergeyshushlyapin/Sitecore.FakeDb/issues/133.
    /// </summary>
    public class LicenseWatcherShutdownTest
    {
        [Test]
        public void ShouldShutdownLicenseWatcher()
        {
            using (var db = new Db())
            {
                db.GetItem("/sitecore/content");
            }
        }
    }
}