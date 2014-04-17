namespace Sitecore.FakeDb.Tests
{
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
    public void ShouldCreateEmptyStandardValuesCollection()
    {
      // arrange & act
      var template = new DbTemplate();

      // assert
      template.StandardValues.Should().BeEmpty();
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
    public void ShouldSetStandardValues()
    {
      // arrange & act
      var template = new DbTemplate { { "Title", "$name" } };

      // assert
      var id = template.Fields.Single().ID;
      template.Fields[id].Value.Should().Be(string.Empty);
      template.StandardValues[id].Value.Should().Be("$name");
    }

    [Fact]
    public void ShouldAddFieldById()
    {
      // arrange
      var fieldId = ID.NewID;

      // act
      var template = new DbTemplate { fieldId };

      // assert
      template.Fields[fieldId].Should().NotBeNull();
      template.Fields[fieldId].Name.Should().Be(fieldId.ToShortID().ToString());
    }
  }
}