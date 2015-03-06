namespace Sitecore.FakeDb.Tests.Globalization.Translate
{
  using FluentAssertions;
  using Sitecore.Configuration;
  using Xunit;

  [Trait("Translate", "Auto-translate is enabled with language prefix")]
  public class AutoTranslateEnabledWithLanguagePrefix : AutoTranslateEnabledTestBase
  {
    public AutoTranslateEnabledWithLanguagePrefix()
    {
      this.Db.Configuration.Settings["Sitecore.FakeDb.AutoTranslatePrefix"] = "{lang}:";
      this.Db.Configuration.Settings["Sitecore.FakeDb.AutoTranslateSuffix"] = string.Empty;
    }

    [Fact(DisplayName = @"Setting ""Sitecore.FakeDb.AutoTranslate"" is ""True""")]
    public void SettingAutoTranslateIsTrue()
    {
      Settings.GetSetting("Sitecore.FakeDb.AutoTranslate").Should().Be("true");
    }

    [Fact(DisplayName = @"Setting ""Sitecore.FakeDb.AutoTranslatePrefix"" is ""{lang}:""")]
    public void SettingAutoTranslatePrefixIsLang()
    {
      Settings.GetSetting("Sitecore.FakeDb.AutoTranslatePrefix").Should().Be("{lang}:");
    }

    [Fact(DisplayName = @"Setting ""Sitecore.FakeDb.AutoTranslateSuffix"" is empty")]
    public void SettingAutoTranslateSuffixIsEmpty()
    {
      Settings.GetSetting("Sitecore.FakeDb.AutoTranslateSuffix").Should().BeEmpty();
    }

    [Fact(DisplayName = @"Translate.Text() adds context language at the beginning of the phrase")]
    public void TranslateTextAddContextLanguageToBeginningOfPhrase()
    {
      Sitecore.Globalization.Translate.Text("Hello!").Should().StartWith("en:Hello!");
    }
  }
}