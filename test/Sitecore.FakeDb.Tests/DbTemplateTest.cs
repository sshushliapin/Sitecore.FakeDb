namespace Sitecore.FakeDb.Tests
{
  using System;
  using System.Linq;
  using FluentAssertions;
  using global::AutoFixture.Xunit2;
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
      var template = new DbTemplate { "Title", "Description" };
      template.Fields.Where(f => !f.Name.StartsWith("__")).Select(f => f.Name).ShouldBeEquivalentTo(new[] { "Title", "Description" });
    }

    [Theory, AutoData]
    public void ShouldSetStandardValues([NoAutoProperties]DbTemplate template)
    {
      template.Add("Title", "$name");

      var id = template.Fields.Single(f => f.Name == "Title").ID;

      template.Fields[id].Value.Should().Be(string.Empty);
      template.StandardValues[id].Value.Should().Be("$name");
    }

    [Theory, AutoData]
    public void ShouldAddFieldById([NoAutoProperties]DbTemplate template, ID fieldId)
    {
      template.Add(fieldId);

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
      template.Fields.Add(new DbField(FieldIDs.BaseTemplate) { Value = id1 + "|" + id2 });

      template.BaseIDs[0].Should().Be(id1);
      template.BaseIDs[1].Should().Be(id2);
    }

    [Theory, AutoData]
    public void ShouldSetDefaultParentId([NoAutoProperties] DbTemplate template)
    {
      template.ParentID.Should().Be(ItemIDs.TemplateRoot);
    }

    [Theory, AutoData]
    public void ShouldAddFieldCopyToStandardValues(DbTemplate template, DbField field, string standardValue)
    {
      template.Add(field, standardValue);

      template.StandardValues.Should().Contain(
        f => f.Name == field.Name &&
             f.ID == field.ID &&
             f.Shared == field.Shared &&
             f.Source == field.Source &&
             f.Type == field.Type &&
             f.Value == standardValue);
    }

    [Theory, AutoData]
    public void ShouldAddFieldToFields(DbTemplate template, DbField field, string standardValue)
    {
      template.Add(field, standardValue);
      template.Fields.Single().Should().BeSameAs(field);
    }

    [Theory, AutoData]
    public void AddThrowsIfFieldIsNull(DbTemplate sut, string standardValue)
    {
      Action action = () => sut.Add((DbField)null, standardValue);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*field");
    }
  }
}