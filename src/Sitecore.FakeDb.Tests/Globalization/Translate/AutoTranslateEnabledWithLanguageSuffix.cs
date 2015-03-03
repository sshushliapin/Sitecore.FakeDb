namespace Sitecore.FakeDb.Tests.Globalization.Translate
{
  using FluentAssertions;
  using Sitecore.Configuration;
  using Xunit;

  [Trait("Auto-translate is enabled with language suffix", "")]
  public class AutoTranslateEnabledWithLanguageSuffix : AutoTranslateEnabledTestBase
  {
    public AutoTranslateEnabledWithLanguageSuffix()
    {
      this.Db.Configuration.Settings["Sitecore.FakeDb.AutoTranslateSuffix"] = "_{lang}";
    }

    [Fact(DisplayName = @"Setting ""Sitecore.FakeDb.AutoTranslate"" is ""True""")]
    public void SettingAutoTranslateIsTrue()
    {
      Settings.GetSetting("Sitecore.FakeDb.AutoTranslate").Should().Be("true");
    }

    [Fact(DisplayName = @"Setting ""Sitecore.FakeDb.AutoTranslateSuffix"" is ""_{lang}""")]
    public void SettingAutoTranslateSuffixIsLang()
    {
      Settings.GetSetting("Sitecore.FakeDb.AutoTranslateSuffix").Should().Be("_{lang}");
    }

    [Fact(DisplayName = @"Translate.Text() adds context language to the end of the phrase")]
    public void TranslateTextAddContextLanguageToEnd()
    {
      Sitecore.Globalization.Translate.Text("Hello!").Should().Be("Hello!_en");
    }
  }
}