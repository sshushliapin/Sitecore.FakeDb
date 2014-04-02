using Sitecore.FakeDb.Data.Engines;

namespace Sitecore.FakeDb.Tests
{
  using System;
  using System.Linq;
  using FluentAssertions;
  using Sitecore.Data;
  using Xunit;

  public class DbTemplateTest
  {
    [Fact]
    public void ShouldCreateEmptyFieldsCollection()
    {
      // arrange
      var template = new DbTemplate();

      // act & assert
      template.Fields.Should().HaveCount(3); // three standard fields
    }

    [Fact]
    public void ShouldCreateEmptyFieldsCollectionWhenSetNameAndId()
    {
      // arrange
      var template = new DbTemplate("t", ID.NewID);

      // act & assert
      template.Fields.Should().HaveCount(3); // three standard fields
    }

    // TODO:[High] The test below states that we cannot get fake item fields by id.
    [Fact]
    public void ShouldCreateTemplateFieldsUsingNamesAsLowercaseKeys()
    {
      // arrange
      var template = new DbTemplate { "Title", "Description" };

      // assert
      template.Fields.Select(f => f.Name).ShouldBeEquivalentTo(new[]
      {
        "Title", "Description", 
        DataStorage.BaseTemplateFieldName, 
        DataStorage.LayoutDetailsFieldName, 
        DataStorage.StandardValuesFieldName
      });
    }

    [Fact]
    public void ShouldBeAssignableToItem()
    {
      var template = new DbTemplate();

      template.Should().BeAssignableTo<DbItem>();
    }

    [Fact]
    public void ShouldHaveStandardFieldsInitialized()
    {
      //arrange
      var template = new DbTemplate();

      //assert
      template.Fields[FieldIDs.StandardValues].Should().NotBeNull();
      template.Fields[FieldIDs.StandardValues].Value.Should().BeEmpty();

      template.Fields[FieldIDs.BaseTemplate].Should().NotBeNull();
      template.Fields[FieldIDs.BaseTemplate].Value.Should().BeEmpty();
    }

    [Fact]
    public void ShouldHaveBaseTemplatePopulated()
    {
      //arrange
      var baseTemplates = new ID[] { ID.NewID, ID.NewID };
      var baseTemplatesRawValue = string.Join("|", baseTemplates.AsEnumerable());

      // act
      var template = new DbTemplate("Template", ID.NewID, baseTemplates);

      //assert
      template.Fields[FieldIDs.BaseTemplate].Value.Should().BeEquivalentTo(baseTemplatesRawValue);
    }

    [Fact]
    public void ShouldCreateStandardValuesIfChildNodeAdded()
    {
      //arrange
      var templateId = ID.NewID;
      var standardValuesItemId = ID.NewID;
      var template = new DbTemplate("Name", templateId)
      {
        new DbItem(Constants.StandardValuesItemName, standardValuesItemId, templateId)
      };

      //assert
      template.Fields[FieldIDs.StandardValues].Value.Should().NotBeNullOrEmpty();
      template.Fields[FieldIDs.StandardValues].Value.Should().BeEquivalentTo(standardValuesItemId.ToString());
    }

    [Fact]
    public void ShouldProperlyDetectStandardValuesItem()
    {
      //arrange
      var templateId = ID.NewID;
      var template = new DbTemplate("Name", templateId);
      var notStandardValues = new DbItem("Item");
      var notThisTemplateStandardValues = new DbItem(Constants.StandardValuesItemName);
      var thisTemplateStandardValues = new DbItem(Constants.StandardValuesItemName, ID.NewID, templateId);

      // assert
      template.IsStandardValuesItem(notStandardValues).Should().BeFalse();
      template.IsStandardValuesItem(notThisTemplateStandardValues).Should().BeFalse();
      template.IsStandardValuesItem(thisTemplateStandardValues).Should().BeTrue();
    }

    [Fact]
    public void ShouldNotAllowToAddFieldsWithValues()
    {
      //arrange
      var template = new DbTemplate();

      //act
      Action action = () => template.Add("Key", " Value");

      //assert
      action.ShouldThrow<InvalidOperationException>();
    }

  }
}