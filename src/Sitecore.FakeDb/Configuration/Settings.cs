namespace Sitecore.FakeDb.Configuration
{
    using System.Xml;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Sitecore.Configuration;
    using Sitecore.Diagnostics;
    using Sitecore.StringExtensions;
    using Sitecore.Xml;

    public class Settings : IDisposable
    {
        private const string AutoTranslateSetting = "FakeDb.AutoTranslate";

        private const string AutoTranslatePrefixSetting = "FakeDb.AutoTranslatePrefix";

        private const string AutoTranslateSuffixSetting = "FakeDb.AutoTranslateSuffix";

        private readonly ICollection<SettingsSwitcher> switchers = new Collection<SettingsSwitcher>();

        public Settings(XmlDocument section)
        {
            ConfigSection = section;
        }

        protected internal XmlDocument ConfigSection { get; }

        public bool AutoTranslate
        {
            get => MainUtil.GetBool(this[AutoTranslateSetting], false);
            set => this[AutoTranslateSetting] = value.ToString().ToLower();
        }


        public string AutoTranslatePrefix
        {
            get => this[AutoTranslatePrefixSetting];
            set => this[AutoTranslatePrefixSetting] = value;
        }

        public string AutoTranslateSuffix
        {
            get => this[AutoTranslateSuffixSetting];
            set => this[AutoTranslateSuffixSetting] = value;
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
                    var settingsNode = XmlUtil.EnsurePath("/sitecore/settings", this.ConfigSection);
                    var setting = this.CreateSettingNode(name, value);
                    settingsNode.AppendChild(setting);
                }

                switchers.Add(new SettingsSwitcher(name, value));
            }
        }

        protected virtual XmlElement CreateSettingNode(string name, string value)
        {
            var doc = this.ConfigSection;
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
            return this.ConfigSection.SelectSingleNode("/sitecore/settings/setting[@name='{0}']".FormatWith(name));
        }

        public void Dispose()
        {
            foreach (var switcher in switchers)
            {
                switcher.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
