namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data;
  using Xunit;

  public class SharedDbFieldTest
  {
    [Theory, AutoData]
    public void ShouldBeDbField(SharedDbField sut)
    {
      sut.Should().BeAssignableTo<DbField>();
    }

    [Theory, AutoData]
    public void ShouldCreateFieldById(ID id)
    {
      var sut = new SharedDbField(id);
      sut.ID.Should().BeSameAs(id);
    }

    [Theory, AutoData]
    public void ShouldCreateFieldByName(string name)
    {
      var sut = new SharedDbField(name);
      sut.Name.Should().Be(name);
    }

    [Theory, AutoData]
    public void ShouldCreateFieldByNameAndId(string name, ID id)
    {
      var sut = new SharedDbField(name, id);
      sut.Name.Should().Be(name);
      sut.ID.Should().BeSameAs(id);
    }

    [Theory, AutoData]
    public void ShouldSetObsoleteSharedField(string name, ID id)
    {
      new SharedDbField(id).Shared.Should().BeTrue();
      new SharedDbField(name).Shared.Should().BeTrue();
      new SharedDbField(name, id).Shared.Should().BeTrue();
    }

    [Theory, AutoData]
    public void ShouldRetunLastValueAddedIgnoringVersion([NoAutoProperties] SharedDbField sut, string value1, string expected)
    {
      sut.Add("en", value1);
      sut.Add("en", expected);

      sut.GetValue("en", 1).Should().Be(expected);
      sut.GetValue("en", 2).Should().Be(expected);
    }

    [Theory, AutoData]
    public void ShouldRetunLastValueAddedIgnoringLanguage([NoAutoProperties] SharedDbField sut, string value1, string expected)
    {
      sut.Add("en", value1);
      sut.Add("da", expected);

      sut.GetValue("en", 1).Should().Be(expected);
      sut.GetValue("da", 1).Should().Be(expected);
    }
  }
}