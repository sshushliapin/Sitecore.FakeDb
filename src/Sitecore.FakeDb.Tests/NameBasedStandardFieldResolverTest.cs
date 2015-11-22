namespace Sitecore.FakeDb.Tests
{
  using System;
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Xunit;

  public class NameBasedStandardFieldResolverTest
  {
    [Theory, AutoData]
    public void ShouldBeIDbFieldBuilder(NameBasedStandardFieldResolver sut)
    {
      sut.Should().BeAssignableTo<IDbFieldBuilder>();
    }

    [Theory, AutoData]
    public void ShouldSetFieldInfoReference([Frozen]StandardFieldsReference fields, NameBasedStandardFieldResolver sut)
    {
      sut.FieldReference.Should().BeSameAs(fields);
    }

    [Fact]
    public void ShouldThrowIfFieldInfoReferenceIsNull()
    {
      Action action = () => new NameBasedStandardFieldResolver(null);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*fieldReference");
    }

    [Theory, AutoData]
    public void ShouldReturnEmptyInfoIfRequestIsNull(NameBasedStandardFieldResolver sut)
    {
      sut.Build(null).Should().Be(FieldInfo.Empty);
    }

    [Theory, AutoData]
    public void ShouldReturnEmptyInfoIfRequestIsNotString(NameBasedStandardFieldResolver sut, object request)
    {
      sut.Build(request).Should().Be(FieldInfo.Empty);
    }

    [Theory, AutoData]
    public void ShouldGetFieldFromStandardByName(NameBasedStandardFieldResolver sut)
    {
      sut.Build("__Base template").Should().NotBe(FieldInfo.Empty);
    }
  }
}