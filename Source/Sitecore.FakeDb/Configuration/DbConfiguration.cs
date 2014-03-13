namespace Sitecore.FakeDb.Configuration
{
  using System.Configuration;
  using Sitecore.Diagnostics;
  using Sitecore.Configuration;
  using System;

  public class DbConfiguration
  {
    private readonly Settings settings;

    public DbConfiguration()
    {
      var section = Factory.GetConfiguration();
      this.settings = new Settings(section);
    }

    public Settings Settings
    {
      get { return this.settings; }
    }
  }
}