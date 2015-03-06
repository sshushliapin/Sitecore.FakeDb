namespace Sitecore.FakeDb.Tests.Globalization.Translate
{
  using FluentAssertions;
  using Xunit;

  [Trait("Translate", "Auto-translate is enabled with language prefix")]
  public class AutoTranslateEnabledWithLanguagePrefix : AutoTranslateTestBase
  {
    public AutoTranslateEnabledWithLanguagePrefix()
    {
      this.Db.Configuration.Settings.AutoTranslate = true;
      this.Db.Configuration.Settings.AutoTranslatePrefix = "{lang}:";
      this.Db.Configuration.Settings.AutoTranslateSuffix = string.Empty;
    }

    [Fact(DisplayName = @"Setting ""FakeDb.AutoTranslate"" is ""True""")]
    public void SettingAutoTranslateIsTrue()
    {
      this.Db.Configuration.Settings.AutoTranslate.Should().BeTrue();
    }

    [Fact(DisplayName = @"Setting ""FakeDb.AutoTranslatePrefix"" is ""{lang}:""")]
    public void SettingAutoTranslatePrefixIsLang()
    {
      this.Db.Configuration.Settings.AutoTranslatePrefix.Should().Be("{lang}:");
    }

    [Fact(DisplayName = @"Setting ""FakeDb.AutoTranslateSuffix"" is empty")]
    public void SettingAutoTranslateSuffixIsEmpty()
    {
      this.Db.Configuration.Settings.AutoTranslateSuffix.Should().BeEmpty();
    }

    [Fact(DisplayName = @"Translate.Text() adds context language at the beginning of the phrase")]
    public void TranslateTextAddContextLanguageToBeginningOfPhrase()
    {
      Sitecore.Globalization.Translate.Text("Hello!").Should().StartWith("en:Hello!");
    }

    [Fact(DisplayName = "Translate.Text() does not use the default \"*\" suffix")]
    public void TranslateTextDontUseDefaultSuffix()
    {
      Sitecore.Globalization.Translate.Text("Hello!").Should().NotEndWith("*");
    }
  }
}