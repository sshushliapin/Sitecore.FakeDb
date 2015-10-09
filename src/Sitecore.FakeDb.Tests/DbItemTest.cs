namespace Sitecore.FakeDb.Tests
{
  using System;
  using System.Linq;
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data;
  using Sitecore.FakeDb.Security.AccessControl;
  using Xunit;

  public class DbItemTest
  {
    [Theory, AutoData]
    public void SutGeneratesNewIdsIfNotSet(string name)
    {
      var sut = new DbItem(name);
      sut.ID.IsNull.Should().BeFalse();
    }

    [Theory, AutoData]
    public void SutGeneratesNameBasedOnIdIfNotSet(ID id)
    {
      var sut = new DbItem(null, id);
      sut.Name.Should().Be(id.ToShortID().ToString());
    }

    [Theory, AutoData]
    public void SutSetsNullTemplate([NoAutoProperties]DbItem sut)
    {
      sut.TemplateID.IsNull.Should().BeTrue();
    }

    [Theory, AutoData]
    public void SutSetsEmptyChildrenCollection([NoAutoProperties]DbItem sut)
    {
      sut.Children.Should().BeEmpty();
    }

    [Theory, AutoData]
    public void SutSetsEmptyFieldsCollection([NoAutoProperties]DbItem sut)
    {
      sut.Fields.Should().BeEmpty();
    }

    [Theory, AutoData]
    public void SutSetsNullFullPath([NoAutoProperties]DbItem sut)
    {
      sut.FullPath.Should().BeNull();
    }

    [Theory, AutoData]
    public void SutSetsNullParentId([NoAutoProperties]DbItem sut)
    {
      sut.ParentID.Should().BeNull();
    }

    [Theory, AutoData]
    public void SutAddsFieldByNameAndValue()
    {
      var sut = new DbItem("home") { { "Title", "Welcome!" } };
      sut.Fields.Should().ContainSingle(f => f.Name == "Title" && f.Value == "Welcome!");
    }

    [Fact]
    public void SutAddsFieldByIdAndValue()
    {
      var sut = new DbItem("home") { { FieldIDs.Hidden, "1" } };
      sut.Fields.Should().ContainSingle(f => f.ID == FieldIDs.Hidden && f.Value == "1");
    }

    [Theory, AutoData]
    public void SutAddsChildItem(DbItem sut, DbItem child)
    {
      sut.Add(child);
      sut.Children.Single().Should().BeEquivalentTo(child);
    }

    [Theory, AutoData]
    public void SutCreateNewItemAccess([NoAutoProperties]DbItem sut)
    {
      sut.Access.Should().BeOfType<DbItemAccess>();
    }

    [Fact]
    public void SutSetsItemAccess()
    {
      var sut = new DbItem("home") { Access = new DbItemAccess { CanRead = false } };
      sut.Access.CanRead.Should().BeFalse();
    }

    [Theory, AutoData]
    public void AddThrowsIfFieldNameIsNull(DbItem sut, string value)
    {
      Action action = () => sut.Add((string)null, value);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*fieldName");
    }

    [Theory, AutoData]
    public void AddThrowsIfFieldIdIsNull(DbItem sut, string value)
    {
      Action action = () => sut.Add((ID)null, value);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*fieldId");
    }

    [Theory, AutoData]
    public void AddThrowsIfFieldIsNull(DbItem sut)
    {
      Action action = () => sut.Add((DbField)null);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*field");
    }

    [Theory, AutoData]
    public void AddThrowsIfChildItemIsNull(DbItem sut)
    {
      Action action = () => sut.Add((DbItem)null);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*child");
    }

    [Theory, AutoData]
    public void AddVersionThrowsIfLanguageIsNull(DbItem sut)
    {
      Action action = () => sut.AddVersion(null, 0);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*language");
    }

    [Theory, AutoData]
    public void AddVersionThrowsIfVersionIsNegative(DbItem sut)
    {
      Action action = () => sut.AddVersion("en", -1);
      action.ShouldThrow<ArgumentOutOfRangeException>().WithMessage("*version");
    }

    [Theory, AutoData]
    public void GetVersionCountThrowsIfLanguageIsNull(DbItem sut)
    {
      Action action = () => sut.GetVersionCount(null);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*language");
    }

    [Theory, AutoData]
    public void RemoveVersionThrowsIfLanguageIsNull(DbItem sut)
    {
      Action action = () => sut.RemoveVersion(null);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*language");
    }
  }
}