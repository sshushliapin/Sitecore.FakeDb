namespace Sitecore.FakeDb.Tests.Globalization.Translate
{
  using FluentAssertions;
  using Sitecore.Configuration;
  using Xunit;

  [Trait("Translate", "Auto-translate is disabled (default)")]
  public class AutoTranslateDisabled
  {
    [Fact(DisplayName = @"Setting ""FakeDb.AutoTranslate"" is ""False""")]
    public void SettingAutoTranslateIsTrue()
    {
      Settings.GetSetting("FakeDb.AutoTranslate").Should().Be("false");
    }

    [Fact(DisplayName = @"Translate.Text() returns the same phrase")]
    public void TranslateTextReturnsSamePhrase()
    {
      Sitecore.Globalization.Translate.Text("Hello").Should().Be("Hello");
    }
  }
}