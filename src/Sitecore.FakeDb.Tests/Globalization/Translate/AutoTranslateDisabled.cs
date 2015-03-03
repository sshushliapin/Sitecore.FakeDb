namespace Sitecore.FakeDb.Tests.Globalization.Translate
{
  using FluentAssertions;
  using Sitecore.Configuration;
  using Xunit;

  [Trait("Auto-translate is disabled (default)", "")]
  public class AutoTranslateDisabled
  {
    [Fact(DisplayName = @"Setting ""Sitecore.FakeDb.AutoTranslate"" is ""False""")]
    public void SettingAutoTranslateIsTrue()
    {
      Settings.GetSetting("Sitecore.FakeDb.AutoTranslate").Should().Be("false");
    }

    [Fact(DisplayName = @"Translate.Text() returns the same phrase")]
    public void TranslateTextReturnSamePhrase()
    {
      Sitecore.Globalization.Translate.Text("Hello").Should().Be("Hello");
    }
  }
}