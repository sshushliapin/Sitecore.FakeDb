namespace Sitecore.FakeDb.Configuration
{
  using System.Xml;
  using Sitecore.StringExtensions;

  public class Settings
  {
    private readonly XmlDocument section;

    public Settings(XmlDocument section)
    {
      this.section = section;
    }

    public virtual string this[string name]
    {
      get
      {
        var settingNode = this.SelectSettingNode(name);

        return settingNode.Attributes["value"].InnerText;
      }

      set
      {
        var settingNode = this.SelectSettingNode(name);
        if (settingNode != null)
        {
          settingNode.Attributes["value"].Value = value;
        }
        else
        {
          var settingsNode = this.section.SelectSingleNode("/sitecore/settings");
          var setting = this.CreateSettingNode(name, value);

          settingsNode.AppendChild(setting);
        }

        Sitecore.Configuration.Settings.Reset();
      }
    }

    protected virtual XmlElement CreateSettingNode(string name, string value)
    {
      var doc = this.section;
      var setting = doc.CreateElement("setting");

      AddSettingAttribute("name", name, doc, setting);
      AddSettingAttribute("value", value, doc, setting);

      return setting;
    }

    protected virtual void AddSettingAttribute(string name, string value, XmlDocument doc, XmlElement setting)
    {
      var attribute = doc.CreateAttribute(name);
      attribute.Value = value;

      setting.Attributes.Append(attribute);
    }

    protected virtual XmlNode SelectSettingNode(string name)
    {
      return this.section.SelectSingleNode("/sitecore/settings/setting[@name='{0}']".FormatWith(name));
    }
  }
}