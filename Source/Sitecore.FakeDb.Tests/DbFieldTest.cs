namespace Sitecore.FakeDb.Tests
{
  using System;
  using System.Collections.Generic;
  using FluentAssertions;
  using Sitecore.Data;
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

    [Fact(Skip = "To be implemented")]
    public void ShouldUseLatesAddedValueIfShared()
    {
      // arrange
      this.field.Shared = true;

      // act
      this.field.Add("en", "shared value");
      this.field.Add("da", "new shared value");

      // assert
      this.field.Value.Should().Be("new shared value");
    }

    [Fact]
    public void ShouldBeReadonlyDictionary()
    {
      // act & assert
      Assert.Throws<NotSupportedException>(() => DbField.FieldIdToNameMapping.Clear());
    }

    [Theory]
    [InlineData("{12C33F3F-86C5-43A5-AEB4-5598CEC45116}", "__Base template")]
    [InlineData("{001DD393-96C5-490B-924A-B0F25CD9EFD8}", "__Lock")]
    [InlineData("{F1A1FE9E-A60C-4DDB-A3A0-BB5B29FE732E}", "__Renderings")]
    [InlineData("{DEC8D2D5-E3CF-48B6-A653-8E69E2716641}", "__Security")]
    [InlineData("{F7D48A55-2158-4F02-9356-756654404F73}", "__Standard values")]
    public void ShouldMapDefaultFieldNameById(string fieldId, string expectedName)
    {
      // arrange
      var id = new ID(fieldId);

      // act
      var dbfield = new DbField(id);

      // assert
      dbfield.Name.Should().Be(expectedName);
    }

    [Theory]
    [InlineData("__Base template", "{12C33F3F-86C5-43A5-AEB4-5598CEC45116}")]
    [InlineData("__Lock", "{001DD393-96C5-490B-924A-B0F25CD9EFD8}")]
    [InlineData("__Renderings", "{F1A1FE9E-A60C-4DDB-A3A0-BB5B29FE732E}")]
    [InlineData("__Security", "{DEC8D2D5-E3CF-48B6-A653-8E69E2716641}")]
    [InlineData("__Standard values", "{F7D48A55-2158-4F02-9356-756654404F73}")]
    public void ShouldMapDefaultFieldIdByName(string fieldName, string expectedId)
    {
      // act
      var dbfield = new DbField(fieldName);

      // assert
      dbfield.ID.ToString().Should().Be(expectedId);
    }

    [Theory(Skip = "To be implemented")]
    [InlineData("__Security", true)]
    [InlineData("Title", false)]
    public void ShouldMarkStandardFieldsSharedByDefault(string fieldName, bool shared)
    {
      // arrange & act
      var field = new DbField(fieldName);

      // assert
      field.Shared.Should().Be(shared);
    }
  }
}