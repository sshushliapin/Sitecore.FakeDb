namespace Sitecore.FakeDb.Tests.Globalization.Translate
{
  using FluentAssertions;
  using Sitecore.Configuration;
  using Xunit;

  [Trait("Translate", "Auto-translate is enabled with language suffix")]
  public class AutoTranslateEnabledWithLanguageSuffix : AutoTranslateEnabledTestBase
  {
    public AutoTranslateEnabledWithLanguageSuffix()
    {
      this.Db.Configuration.Settings["FakeDb.AutoTranslatePrefix"] = string.Empty;
      this.Db.Configuration.Settings["FakeDb.AutoTranslateSuffix"] = "_{lang}";
    }

    [Fact(DisplayName = @"Setting ""FakeDb.AutoTranslate"" is ""True""")]
    public void SettingAutoTranslateIsTrue()
    {
      Settings.GetSetting("FakeDb.AutoTranslate").Should().Be("true");
    }

    [Fact(DisplayName = @"Setting ""FakeDb.AutoTranslatePrefix"" is empty")]
    public void SettingAutoTranslatePrefixIsEmpty()
    {
      Settings.GetSetting("FakeDb.AutoTranslatePrefix").Should().BeEmpty();
    }

    [Fact(DisplayName = @"Setting ""FakeDb.AutoTranslateSuffix"" is ""_{lang}""")]
    public void SettingAutoTranslateSuffixIsLang()
    {
      Settings.GetSetting("FakeDb.AutoTranslateSuffix").Should().Be("_{lang}");
    }

    [Fact(DisplayName = @"Translate.Text() adds context language to the end of the phrase")]
    public void TranslateTextAddContextLanguageToEnd()
    {
      Sitecore.Globalization.Translate.Text("Hello!").Should().Be("Hello!_en");
    }
  }
}