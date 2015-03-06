namespace Sitecore.FakeDb.Tests.Globalization.Translate
{
  using FluentAssertions;
  using Xunit;

  [Trait("Translate", "Auto-translate is disabled (default)")]
  public class AutoTranslateDisabled : AutoTranslateTestBase
  {
    public AutoTranslateDisabled()
    {
      this.Db.Configuration.Settings.AutoTranslate = false;
    }

    [Fact(DisplayName = @"Setting ""FakeDb.AutoTranslate"" is ""False""")]
    public void SettingAutoTranslateIsTrue()
    {
      this.Db.Configuration.Settings.AutoTranslate.Should().BeFalse();
    }

    [Fact(DisplayName = @"Setting ""FakeDb.AutoTranslatePrefix"" is empty")]
    public void SettingAutoTranslatePrefixIsEmpty()
    {
      this.Db.Configuration.Settings.AutoTranslatePrefix.Should().BeEmpty();
    }

    [Fact(DisplayName = @"Setting ""FakeDb.AutoTranslateSuffix"" is empty")]
    public void SettingAutoTranslateSuffixIsEmpty()
    {
      this.Db.Configuration.Settings.AutoTranslateSuffix.Should().BeEmpty();
    }

    [Fact(DisplayName = @"Translate.Text() returns the same phrase")]
    public void TranslateTextReturnsSamePhrase()
    {
      Sitecore.Globalization.Translate.Text("Hello").Should().Be("Hello");
    }
  }
}