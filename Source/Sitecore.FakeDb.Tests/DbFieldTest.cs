namespace Sitecore.FakeDb.Tests
{
  using System;
  using System.Collections.Generic;
  using FluentAssertions;
  using Sitecore.Globalization;
  using Xunit;
  using Xunit.Extensions;

  public class DbFieldTest
  {
    private readonly DbField field;

    private readonly DbField layoutField;

    public DbFieldTest()
    {
      this.field = new DbField("Title") { Type = "Single-Line Text" };
      this.layoutField = new DbField(FieldIDs.LayoutField);
    }

    [Fact]
    public void ShouldSetName()
    {
      // act & assert
      this.field.Name.Should().Be("Title");
      this.layoutField.Name.Should().Be("__Renderings");
    }

    [Fact]
    public void ShouldSetType()
    {
      // act & assert
      this.field.Type.Should().Be("Single-Line Text");
    }

    [Fact]
    public void ShouldInstantiateVersionsAsSortedDictionary()
    {
      // act
      this.field.Add("en", "value");

      // assert
      this.field.Values["en"].Should().BeOfType<SortedDictionary<int, string>>();
    }

    [Fact]
    public void ShouldAddAndGetLocalizedValues()
    {
      // act
      this.field.Add("en", "en_value");
      this.field.Add("ua", "ua_value");

      // assert
      this.field.GetValue("en", 1).Should().Be("en_value");
      this.field.GetValue("ua", 1).Should().Be("ua_value");
    }

    [Fact]
    public void ShouldAddAndGetVersionedValues()
    {
      // act
      this.field.Add("en", 1, "en_value1");
      this.field.Add("en", 2, "en_value2");
      this.field.Add("ua", 1, "ua_value1");
      this.field.Add("ua", 2, "ua_value2");

      // assert
      this.field.GetValue("en", 1).Should().Be("en_value1");
      this.field.GetValue("en", 2).Should().Be("en_value2");
      this.field.GetValue("ua", 1).Should().Be("ua_value1");
      this.field.GetValue("ua", 2).Should().Be("ua_value2");
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
    public void ShouldGetLatestAvailableVersionIfNothingFound()
    {
      // arrange
      this.field.Add("en", 1, "value");

      // assert
      this.field.GetValue("en", 100).Should().Be("value");
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

    [Fact]
    public void ShouldAddVersionsImplicitly()
    {
      // act
      this.field.Add("en", 3, "Hello!");

      // assert
      this.field.Values["en"][1].Should().BeEmpty();
      this.field.Values["en"][2].Should().BeEmpty();
      this.field.Values["en"][3].Should().Be("Hello!");
    }

    [Fact]
    public void ShouldNotOverrideExistingVersionWhenAddingVersionsImplicitly()
    {
      // act
      this.field.Add("en", 1, "Hello v1!");
      this.field.Add("en", 3, "Hello v3!");

      // assert
      this.field.Values["en"][1].Should().Be("Hello v1!");
      this.field.Values["en"][2].Should().BeEmpty();
      this.field.Values["en"][3].Should().Be("Hello v3!");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ShouldThrowExceptionIfVersionIsNotPositive(int version)
    {
      // act
      Action action = () => this.field.Add("en", version, "value");

      // assert
      action.ShouldThrow<ArgumentOutOfRangeException>().WithMessage("Version cannot be zero or negative.*");
    }

    [Fact]
    public void ShouldThrowExceptionIfVersionExists()
    {
      // arrange
      this.field.Add("en", 1, "value");

      // act
      Action action = () => this.field.Add("en", 1, "value");

      // assert
      action.ShouldThrow<ArgumentException>().WithMessage("An item with the same version has already been added.");
    }
  }
}