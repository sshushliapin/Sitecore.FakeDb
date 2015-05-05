namespace Sitecore.FakeDb.Tests
{
  using System.Linq;
  using FluentAssertions;
  using Sitecore.Data;
  using Xunit;

  public class DbTemplateTest
  {
    [Fact]
    public void ShouldBeAnItem()
    {
      // arrange
      var template = new DbTemplate();

      // assert
      template.Should().BeAssignableTo<DbItem>();
    }

    [Fact]
    public void ShouldInstantiateStandardValuesCollection()
    {
      // arrange & act
      var template = new DbTemplate();

      // assert
      template.StandardValues.Should().NotBeNull();
    }

    // TODO:[High] The test below states that we cannot get fake item fields by id.

    [Fact]
    public void ShouldCreateTemplateFieldsUsingNamesAsLowercaseKeys()
    {
      // arrange
      var template = new DbTemplate { "Title", "Description" };

      // assert
      template.Fields.Where(f => !f.Name.StartsWith("__")).Select(f => f.Name).ShouldBeEquivalentTo(new[] { "Title", "Description" });
    }

    [Fact]
    public void ShouldSetStandardValues()
    {
      // arrange & act
      var template = new DbTemplate { { "Title", "$name" } };

      // assert
      var id = template.Fields.Single(f => f.Name == "Title").ID;

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

    [Fact]
    public void ShouldBeEmptyBaseIds()
    {
      // arrange
      var template = new DbTemplate();

      // act & assert
      template.BaseIDs.Should().BeEmpty();
    }

    [Fact]
    public void ShouldGetBaseIdsFromFieldsIfExist()
    {
      // arrange
      var id1 = ID.NewID;
      var id2 = ID.NewID;

      var template = new DbTemplate();
      template.Fields.Add(new DbField(FieldIDs.BaseTemplate) { Value = id1 + "|" + id2 });

      // act & assert
      template.BaseIDs[0].Should().Be(id1);
      template.BaseIDs[1].Should().Be(id2);
    }
  }
}