namespace Sitecore.FakeDb.Configuration
{
  using System.Xml;

  public class DbConfiguration
  {
    private readonly Settings settings;

    public DbConfiguration(XmlDocument config)
    {
      this.settings = new Settings(config);
    }

    public Settings Settings
    {
      get { return this.settings; }
    }
  }
}