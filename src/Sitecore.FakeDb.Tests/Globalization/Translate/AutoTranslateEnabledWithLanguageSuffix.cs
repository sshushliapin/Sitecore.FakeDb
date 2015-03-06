namespace Sitecore.FakeDb.Tests.Globalization.Translate
{
  using FluentAssertions;
  using Xunit;

  [Trait("Translate", "Auto-translate is enabled with language suffix")]
  public class AutoTranslateEnabledWithLanguageSuffix : AutoTranslateTestBase
  {
    public AutoTranslateEnabledWithLanguageSuffix()
    {
      this.Db.Configuration.Settings.AutoTranslate = true;
      this.Db.Configuration.Settings.AutoTranslatePrefix = string.Empty;
      this.Db.Configuration.Settings.AutoTranslateSuffix = "_{lang}";
    }

    [Fact(DisplayName = @"Setting ""FakeDb.AutoTranslate"" is ""True""")]
    public void SettingAutoTranslateIsTrue()
    {
      this.Db.Configuration.Settings.AutoTranslate.Should().BeTrue();
    }

    [Fact(DisplayName = @"Setting ""FakeDb.AutoTranslatePrefix"" is empty")]
    public void SettingAutoTranslatePrefixIsEmpty()
    {
      this.Db.Configuration.Settings.AutoTranslatePrefix.Should().BeEmpty();
    }

    [Fact(DisplayName = @"Setting ""FakeDb.AutoTranslateSuffix"" is ""_{lang}""")]
    public void SettingAutoTranslateSuffixIsLang()
    {
      this.Db.Configuration.Settings.AutoTranslateSuffix.Should().Be("_{lang}");
    }

    [Fact(DisplayName = @"Translate.Text() adds context language to the end of the phrase")]
    public void TranslateTextAddContextLanguageToEnd()
    {
      Sitecore.Globalization.Translate.Text("Hello!").Should().Be("Hello!_en");
    }
  }
}