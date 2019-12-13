namespace Sitecore.FakeDb.Configuration
{
    using System;
    using System.Xml;

    public class DbConfiguration : IDisposable
    {
        public DbConfiguration(XmlDocument config)
        {
            Settings = new Settings(config);
        }

        public Settings Settings { get; }

        public void Dispose()
        {
            Settings?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
