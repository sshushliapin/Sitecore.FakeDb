namespace Sitecore.FakeDb.Tests.Globalization
{
  using FluentAssertions;
  using Sitecore.Globalization;
  using Xunit;
  using Xunit.Extensions;

  public class TranslateTest
  {
    [Fact]
    public void ShouldTranslateText()
    {
      // arrange
      Translate.Text("My text").Should().Be("en:My text");
    }

    [Theory]
    [InlineData("en", "en:My text")]
    [InlineData("da", "da:My text")]
    public void ShouldTranslateTextByLanguage(string language, string expected)
    {
      // arrange
      Translate.TextByLanguage("My text", Language.Parse(language)).Should().Be(expected);
    }
  }
}