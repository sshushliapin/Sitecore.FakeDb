namespace Sitecore.FakeDb.Tests
{
  using System;
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Xunit;

  public class StandardNameFieldBuilderTest
  {
    [Theory, AutoData]
    public void ShouldBeIDbFieldBuilder(StandardNameFieldBuilder sut)
    {
      sut.Should().BeAssignableTo<IDbFieldBuilder>();
    }

    [Theory, AutoData]
    public void ShouldSetFieldInfoReference([Frozen]FieldInfoReference fields, StandardNameFieldBuilder sut)
    {
      sut.FieldReference.Should().BeSameAs(fields);
    }

    [Fact]
    public void ShouldThrowIfFieldInfoReferenceIsNull()
    {
      Action action = () => new StandardNameFieldBuilder(null);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*fieldReference");
    }

    [Theory, AutoData]
    public void ShouldReturnEmptyInfoIfRequestIsNull(StandardNameFieldBuilder sut)
    {
      sut.Build(null).Should().Be(FieldInfo.Empty);
    }

    [Theory, AutoData]
    public void ShouldReturnEmptyInfoIfRequestIsNotString(StandardNameFieldBuilder sut, object request)
    {
      sut.Build(request).Should().Be(FieldInfo.Empty);
    }

    [Theory, AutoData]
    public void ShouldGetFieldFromStandardByName(StandardNameFieldBuilder sut)
    {
      sut.Build("__Base template").Should().NotBe(FieldInfo.Empty);
    }
  }
}