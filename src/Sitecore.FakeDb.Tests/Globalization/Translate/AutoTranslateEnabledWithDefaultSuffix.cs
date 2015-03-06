namespace Sitecore.FakeDb.Tests.Globalization.Translate
{
  using FluentAssertions;
  using Sitecore.Configuration;
  using Xunit;

  [Trait("Translate", "Auto-translate is enabled with default suffix")]
  public class AutoTranslateEnabledWithDefaultSuffix : AutoTranslateEnabledTestBase
  {
    [Fact(DisplayName = @"Setting ""FakeDb.AutoTranslate"" is ""True""")]
    public void SettingAutoTranslateIsTrue()
    {
      Settings.GetSetting("FakeDb.AutoTranslate").Should().Be("true");
    }

    [Fact(DisplayName = @"Setting ""FakeDb.AutoTranslateSuffix"" is ""*""")]
    public void SettingAutoTranslateSuffixIsAsterisk()
    {
      Settings.GetSetting("FakeDb.AutoTranslateSuffix").Should().Be("*");
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