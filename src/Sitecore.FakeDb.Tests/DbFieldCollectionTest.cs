namespace Sitecore.FakeDb.Tests
{
  using System;
  using System.Collections;
  using System.Linq;
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data;
  using Xunit;

  public class DbFieldCollectionTest
  {
    [Theory, AutoData]
    public void AddField(DbFieldCollection sut, DbField field)
    {
      sut.Add(field);
      sut.Count().Should().Be(1);
    }

    [Theory, AutoData]
    public void AddFieldByName(DbFieldCollection sut, string fieldName)
    {
      sut.Add(fieldName);
      sut.Single().Name.Should().Be(fieldName);
    }

    [Theory, AutoData]
    public void AddFieldByNameGeneratesNewId(DbFieldCollection sut, string fieldName)
    {
      sut.Add(fieldName);
      sut.Single().ID.Should().NotBe(ID.Null);
    }

    [Theory, AutoData]
    public void AddFieldByNameSetsEmptyValue(DbFieldCollection sut, string fieldName)
    {
      sut.Add(fieldName);
      sut.Single().Value.Should().BeEmpty();
    }

    [Theory, AutoData]
    public void AddFieldByNameAndValue(DbFieldCollection sut, string fieldName, string value)
    {
      sut.Add(fieldName, value);
      sut.Single().Value.Should().Be(value);
    }

    [Theory, AutoData]
    public void GetFieldById(DbFieldCollection sut, DbField field)
    {
      sut.Add(field);
      sut[field.ID].ShouldBeEquivalentTo(field);
    }

    [Theory, AutoData]
    public void SetFieldById(DbFieldCollection sut, [Frozen] ID id, DbField field)
    {
      sut[id] = field;
      sut[id].Should().BeSameAs(field);
    }

    [Theory, AutoData]
    public void ResetFieldById(DbFieldCollection sut, [Frozen] ID id, DbField originalField, DbField newField)
    {
      sut[id] = originalField;
      sut[id] = newField;

      sut[id].Should().BeSameAs(newField);
    }

    [Theory, AutoData]
    public void GetFieldThrowsIfNoFieldIdFound(DbFieldCollection sut, ID missingFieldId)
    {
      var expectedMessage = string.Format("The given field \"{0}\" is not present in the item.", missingFieldId);

      Assert.Throws<InvalidOperationException>(() => sut[missingFieldId])
        .Message.Should().Be(expectedMessage);
    }

    [Theory, AutoData]
    public void ContainsFieldIsTrueIfExists(DbFieldCollection sut, DbField field)
    {
      sut.Add(field);
      sut.ContainsKey(field.ID).Should().BeTrue();
    }

    [Theory, AutoData]
    public void ContainsKeyIsFalseIfNotFound(DbFieldCollection sut, ID missingFieldId)
    {
      sut.ContainsKey(missingFieldId).Should().BeFalse();
    }

    [Theory, AutoData]
    public void SutReturnsFields(DbFieldCollection sut, DbField field1, DbField field2)
    {
      sut.Add(field1);
      sut.Add(field2);

      sut.ShouldAllBeEquivalentTo(new[] { field1, field2 });
    }

    [Theory, AutoData]
    public void SutReturnsEnumerator(DbFieldCollection sut)
    {
      ((IEnumerable)sut).GetEnumerator().Should().NotBeNull();
    }

    [Theory, AutoData]
    public void TryGetValueReturnsFalseAndEmptyStringIfNoValueFound(DbFieldCollection sut, ID fieldId)
    {
      string value;
      sut.TryGetValue(fieldId, out value).Should().BeFalse();
      value.Should().BeEmpty();
    }

    [Theory, AutoData]
    public void TryGetValueReturnsTrueAndValue(DbFieldCollection sut, ID fieldId, string expected)
    {
      sut.Add(fieldId, expected);

      string value;
      sut.TryGetValue(fieldId, out value).Should().BeTrue();
      value.Should().Be(expected);
    }
  }
}