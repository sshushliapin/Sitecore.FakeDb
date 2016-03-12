namespace Sitecore.FakeDb.Tests
{
  using System.Linq;
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data;
  using Xunit;

  public class DbTemplateTest
  {
    [Theory, AutoData]
    public void ShouldBeAnItem([NoAutoProperties]DbTemplate template)
    {
      template.Should().BeAssignableTo<DbItem>();
    }

    [Theory, AutoData]
    public void ShouldInstantiateStandardValuesCollection([NoAutoProperties]DbTemplate template)
    {
      template.StandardValues.Should().NotBeNull();
    }

    [Fact]
    public void ShouldCreateTemplateFieldsUsingNamesAsLowercaseKeys()
    {
      // arrange
      var template = new DbTemplate { "Title", "Description" };

      // assert
      template.Fields.Where(f => !f.Name.StartsWith("__")).Select(f => f.Name).ShouldBeEquivalentTo(new[] { "Title", "Description" });
    }

    [Theory, AutoData]
    public void ShouldSetStandardValues([NoAutoProperties]DbTemplate template)
    {
      // act
      template.Add("Title", "$name");

      // assert
      var id = template.Fields.Single(f => f.Name == "Title").ID;

      template.Fields[id].Value.Should().Be(string.Empty);
      template.StandardValues[id].Value.Should().Be("$name");
    }

    [Theory, AutoData]
    public void ShouldAddFieldById([NoAutoProperties]DbTemplate template, ID fieldId)
    {
      // act
      template.Add(fieldId);

      // assert
      template.Fields[fieldId].Should().NotBeNull();
      template.Fields[fieldId].Name.Should().Be(fieldId.ToShortID().ToString());
    }

    [Theory, AutoData]
    public void ShouldBeEmptyBaseIds([NoAutoProperties]DbTemplate template)
    {
      template.BaseIDs.Should().BeEmpty();
    }

    [Theory, AutoData]
    public void ShouldGetBaseIdsFromFieldsIfExist([NoAutoProperties]DbTemplate template, ID id1, ID id2)
    {
      // arrange
      template.Fields.Add(new DbField(FieldIDs.BaseTemplate) { Value = id1 + "|" + id2 });

      // act & assert
      template.BaseIDs[0].Should().Be(id1);
      template.BaseIDs[1].Should().Be(id2);
    }

    [Theory, AutoData]
    public void ShouldSetDefaultParentId([NoAutoProperties] DbTemplate template)
    {
      template.ParentID.Should().Be(ItemIDs.TemplateRoot);
    }
  }
}