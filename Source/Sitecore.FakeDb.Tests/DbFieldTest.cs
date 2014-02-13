namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Sitecore.Globalization;
  using Xunit;

  public class DbFieldTest
  {
    private readonly DbField field;

    public DbFieldTest()
    {
      this.field = new DbField("Title");
    }

    [Fact]
    public void ShouldSetName()
    {
      // act & assert
      field.Name.Should().Be("Title");
    }

    [Fact]
    public void ShouldAddValuesToLocalizableValuesCollection()
    {
      // act
      this.field.Add("en", "en_value");
      this.field.Add("ua", "ua_value");

      // assert
      this.field.LocalizableValues["en"].Should().Be("en_value");
      this.field.LocalizableValues["ua"].Should().Be("ua_value");
    }

    [Fact]
    public void ShouldGetValueInCurrentLanguage()
    {
      // arrange
      this.field.Add("en", "en_value");
      this.field.Add("da", "da_value");

      var language = Language.Parse("da");

      // act
      using (new LanguageSwitcher(language))
      {
        this.field.Value.Should().Be("da_value");
      }
    }

    [Fact]
    public void ShouldSetAndGetValueInCurrentLanguage()
    {
      // act
      this.field.Value = "Hi there!";

      // assert
      this.field.Value.Should().Be("Hi there!");
    }

    [Fact]
    public void ShouldReturnEmptyStringIfNoValueFoundInCurrentLanguage()
    {
      // arrange
      this.field.Add("en", "en_value");

      // act & assert
      using (new LanguageSwitcher(Language.Parse("da")))
      {
        this.field.Value.Should().BeEmpty();
      }
    }
  }
}