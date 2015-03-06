namespace Sitecore.FakeDb.Tests.Globalization.Translate
{
  using FluentAssertions;
  using Xunit;

  [Trait("Translate", "Auto-translate is enabled with default suffix")]
  public class AutoTranslateEnabledWithDefaultSuffix : AutoTranslateTestBase
  {
    public AutoTranslateEnabledWithDefaultSuffix()
    {
      this.Db.Configuration.Settings.AutoTranslate = true;
    }

    [Fact(DisplayName = @"Setting ""FakeDb.AutoTranslate"" is ""True""")]
    public void SettingAutoTranslateIsTrue()
    {
      this.Db.Configuration.Settings.AutoTranslate.Should().BeTrue();
    }

    [Fact(DisplayName = @"Setting ""FakeDb.AutoTranslateSuffix"" is empty")]
    public void SettingAutoTranslateSuffixIsAsterisk()
    {
      this.Db.Configuration.Settings.AutoTranslateSuffix.Should().BeEmpty();
    }

    [Fact(DisplayName = @"Translate.Text() adds ""*"" to the end of the phrase")]
    public void TranslateTextAddAsteriskToEnd()
    {
      Sitecore.Globalization.Translate.Text("Hello!").Should().Be("Hello!*");
    }

    [Fact(DisplayName = @"Translate.Text() does not translate phrases twice")]
    public void TranslateTextNotTranslatePhraseTwice()
    {
      Sitecore.Globalization.Translate.Text(
        Sitecore.Globalization.Translate.Text("Hello!"))
        .Should().Be("Hello!*");
    }
  }
}