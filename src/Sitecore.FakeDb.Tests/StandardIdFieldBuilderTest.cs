namespace Sitecore.FakeDb.Tests
{
  using System;
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Xunit;

  public class StandardIdFieldBuilderTest
  {
    [Theory, AutoData]
    public void ShouldBeIDbFieldBuilder(StandardIdFieldBuilder sut)
    {
      sut.Should().BeAssignableTo<IDbFieldBuilder>();
    }

    [Theory, AutoData]
    public void ShouldSetFieldInfoReference([Frozen]FieldInfoReference fields, StandardIdFieldBuilder sut)
    {
      sut.FieldReference.Should().BeSameAs(fields);
    }

    [Fact]
    public void ShouldThrowIfFieldInfoReferenceIsNull()
    {
      Action action = () => new StandardIdFieldBuilder(null);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*fieldReference");
    }

    [Theory, AutoData]
    public void ShouldReturnEmptyInfoIfRequestIsNull(StandardIdFieldBuilder sut)
    {
      sut.Build(null).Should().Be(FieldInfo.Empty);
    }

    [Theory, AutoData]
    public void ShouldReturnEmptyInfoIfRequestIsNotId(StandardIdFieldBuilder sut, object request)
    {
      sut.Build(request).Should().Be(FieldInfo.Empty);
    }

    [Theory, AutoData]
    public void ShouldGetFieldFromStandardById(StandardIdFieldBuilder sut)
    {
      sut.Build(FieldIDs.BaseTemplate).Should().NotBe(FieldInfo.Empty);
    }
  }
}