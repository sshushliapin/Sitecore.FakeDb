namespace Sitecore.FakeDb.Configuration
{
  using System.Xml;
  using Sitecore.Diagnostics;
  using Sitecore.StringExtensions;
  using Sitecore.Xml;

  public class Settings
  {
    private readonly XmlDocument section;

    public Settings(XmlDocument section)
    {
      this.section = section;
    }

    protected internal XmlDocument ConfigSection
    {
      get { return this.section; }
    }

    public virtual string this[string name]
    {
      get
      {
        var settingNode = this.SelectSettingNode(name);

        return XmlUtil.GetAttribute("value", settingNode);
      }

      set
      {
        Assert.ArgumentNotNullOrEmpty(name, "name");

        var settingNode = this.SelectSettingNode(name);
        if (settingNode != null)
        {
          XmlUtil.SetAttribute("value", value, settingNode);
        }
        else
        {
          var settingsNode = XmlUtil.EnsurePath("/sitecore/settings", this.section);
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

      this.AddSettingAttribute("name", name, doc, setting);
      this.AddSettingAttribute("value", value, doc, setting);

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