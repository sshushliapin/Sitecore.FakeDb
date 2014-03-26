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
      template.Fields.Should().BeEmpty();
    }

    [Fact]
    public void ShouldCreateEmptyFieldsCollectionWhenSetNameAndId()
    {
      // arrange
      var template = new DbTemplate("t", ID.NewID);

      // act & assert
      template.Fields.Should().BeEmpty();
    }

    // TODO:[High] The test below states that we cannot get fake item fields by id.
    [Fact]
    public void ShouldCreateTemplateFieldsUsingNamesAsLowercaseKeys()
    {
      // arrange
      var template = new DbTemplate { "Title", "Description" };

      // assert
      template.Fields.Select(f => f.Name).ShouldBeEquivalentTo(new[] { "Title", "Description" });
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
      template.StandardFields[FieldIDs.StandardValues].Should().NotBeNull();
      template.StandardFields[FieldIDs.StandardValues].Value.Should().BeEmpty();

      template.StandardFields[FieldIDs.BaseTemplate].Should().NotBeNull();
      template.StandardFields[FieldIDs.BaseTemplate].Value.Should().BeEmpty();
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
      template.StandardFields[FieldIDs.BaseTemplate].Value.Should().BeEquivalentTo(baseTemplatesRawValue);
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
      template.StandardFields[FieldIDs.StandardValues].Value.Should().NotBeNullOrEmpty();
      template.StandardFields[FieldIDs.StandardValues].Value.Should().BeEquivalentTo(standardValuesItemId.ToString());
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