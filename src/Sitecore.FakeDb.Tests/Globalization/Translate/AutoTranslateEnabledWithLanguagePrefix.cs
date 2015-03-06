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
      this.Db.Configuration.Settings["FakeDb.AutoTranslatePrefix"] = "{lang}:";
      this.Db.Configuration.Settings["FakeDb.AutoTranslateSuffix"] = string.Empty;
    }

    [Fact(DisplayName = @"Setting ""FakeDb.AutoTranslate"" is ""True""")]
    public void SettingAutoTranslateIsTrue()
    {
      Settings.GetSetting("FakeDb.AutoTranslate").Should().Be("true");
    }

    [Fact(DisplayName = @"Setting ""FakeDb.AutoTranslatePrefix"" is ""{lang}:""")]
    public void SettingAutoTranslatePrefixIsLang()
    {
      Settings.GetSetting("FakeDb.AutoTranslatePrefix").Should().Be("{lang}:");
    }

    [Fact(DisplayName = @"Setting ""FakeDb.AutoTranslateSuffix"" is empty")]
    public void SettingAutoTranslateSuffixIsEmpty()
    {
      Settings.GetSetting("FakeDb.AutoTranslateSuffix").Should().BeEmpty();
    }

    [Fact(DisplayName = @"Translate.Text() adds context language at the beginning of the phrase")]
    public void TranslateTextAddContextLanguageToBeginningOfPhrase()
    {
      Sitecore.Globalization.Translate.Text("Hello!").Should().StartWith("en:Hello!");
    }
  }
}