namespace Sitecore.FakeDb.Tests
{
  using System;
  using FluentAssertions;
  using global::AutoFixture.Xunit2;
  using Xunit;

  public class IdBasedStandardFieldResolverTest
  {
    [Theory, AutoData]
    public void ShouldBeIDbFieldBuilder(IdBasedStandardFieldResolver sut)
    {
      sut.Should().BeAssignableTo<IDbFieldBuilder>();
    }

    [Theory, AutoData]
    public void ShouldSetFieldInfoReference([Frozen]StandardFieldsReference fields, IdBasedStandardFieldResolver sut)
    {
      sut.FieldReference.Should().BeSameAs(fields);
    }

    [Fact]
    public void ShouldThrowIfFieldInfoReferenceIsNull()
    {
      Action action = () => new IdBasedStandardFieldResolver(null);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*fieldReference");
    }

    [Theory, AutoData]
    public void ShouldReturnEmptyInfoIfRequestIsNull(IdBasedStandardFieldResolver sut)
    {
      sut.Build(null).Should().Be(FieldInfo.Empty);
    }

    [Theory, AutoData]
    public void ShouldReturnEmptyInfoIfRequestIsNotId(IdBasedStandardFieldResolver sut, object request)
    {
      sut.Build(request).Should().Be(FieldInfo.Empty);
    }

    [Theory, AutoData]
    public void ShouldGetFieldFromStandardById(IdBasedStandardFieldResolver sut)
    {
      sut.Build(FieldIDs.BaseTemplate).Should().NotBe(FieldInfo.Empty);
    }
  }
}