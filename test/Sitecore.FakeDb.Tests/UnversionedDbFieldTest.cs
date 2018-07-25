namespace Sitecore.FakeDb.Tests
{
  using System;
  using FluentAssertions;
  using global::AutoFixture.Xunit2;
  using Sitecore.Data;
  using Xunit;

  public class UnversionedDbFieldTest
  {
    [Theory, AutoData]
    public void ShouldBeDbField(UnversionedDbField sut)
    {
      sut.Should().BeAssignableTo<DbField>();
    }

    [Theory, AutoData]
    public void ShouldCreateFieldById(ID id)
    {
      var sut = new UnversionedDbField(id);
      sut.ID.Should().BeSameAs(id);
    }

    [Theory, AutoData]
    public void ShouldCreateFieldByName(string name)
    {
      var sut = new UnversionedDbField(name);
      sut.Name.Should().Be(name);
    }

    [Theory, AutoData]
    public void ShouldCreateFieldByNameAndId(string name, ID id)
    {
      var sut = new UnversionedDbField(name, id);
      sut.Name.Should().Be(name);
      sut.ID.Should().BeSameAs(id);
    }

    [Theory, AutoData]
    public void AddThrowsIfLanguageIsNull([NoAutoProperties] UnversionedDbField sut)
    {
      Action action = () => sut.Add(null, null);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*language");
    }

    [Theory, AutoData]
    public void ShouldRetunEmptyValueForLanguage([NoAutoProperties] UnversionedDbField sut)
    {
      sut.GetValue("en", 1).Should().BeEmpty();
    }

    [Theory, AutoData]
    public void ShouldRetunLastValueAddedPerLanguage([NoAutoProperties] UnversionedDbField sut, string value1, string expected)
    {
      sut.Add("en", value1);
      sut.Add("en", expected);

      sut.GetValue("en", 1).Should().Be(expected);
      sut.GetValue("en", 2).Should().Be(expected);
    }

    [Theory, AutoData]
    public void ShouldSetValuePerLanguage([NoAutoProperties] UnversionedDbField sut, string value1, string value2)
    {
      sut.SetValue("en", value1);
      sut.SetValue("dk", value2);

      sut.GetValue("en", 0).Should().Be(value1);
      sut.GetValue("dk", 0).Should().Be(value2);
    }
  }
}