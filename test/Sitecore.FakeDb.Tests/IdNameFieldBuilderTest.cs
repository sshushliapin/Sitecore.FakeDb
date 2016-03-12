namespace Sitecore.FakeDb.Tests
{
  using System;
  using System.Linq;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Xunit;

  public class IdNameFieldBuilderTest
  {
    [Theory, DefaultSubstituteAutoData]
    public void ShouldBeIDbFieldBuilder(IdNameFieldBuilder sut)
    {
      sut.Should().BeAssignableTo<IDbFieldBuilder>();
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldContainNameBilder(IDbFieldBuilder nameBuilder, IDbFieldBuilder idBuilder)
    {
      var sut = new IdNameFieldBuilder(nameBuilder, idBuilder);
      sut.NameBuilder.Should().BeSameAs(nameBuilder);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldContainIdBilder(IDbFieldBuilder nameBuilder, IDbFieldBuilder idBuilder)
    {
      var sut = new IdNameFieldBuilder(nameBuilder, idBuilder);
      sut.IdBuilder.Should().BeSameAs(idBuilder);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldReturnEmptyInfoIfRequestIsNull(IdNameFieldBuilder sut)
    {
      sut.Build(null).Should().Be(FieldInfo.Empty);
    }

    [Fact]
    public void ShouldThrowIfNameBuilderIsNull()
    {
      Action action = () => new IdNameFieldBuilder(null, null);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*nameBuilder");
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldThrowIfIdBuilderIsNull(IDbFieldBuilder nameBuilder)
    {
      Action action = () => new IdNameFieldBuilder(nameBuilder, null);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*idBuilder");
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldReturnEmptyInfoIfEmptyRequestPassed(IdNameFieldBuilder sut)
    {
      sut.Build(new { }).Should().Be(FieldInfo.Empty);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldCallNameBuilderWithFirstStringRequestFound(IdNameFieldBuilder sut, string[] request, FieldInfo expected)
    {
      sut.NameBuilder.Build(request.ElementAt(0)).Returns(expected);
      sut.Build(request).Name.Should().Be(expected.Name);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldCallIdBuilderWithFirstIdRequestFound(IdNameFieldBuilder sut, ID[] request, FieldInfo expected)
    {
      sut.IdBuilder.Build(request.ElementAt(0)).Returns(expected);
      sut.Build(request).Id.Should().Be(expected.Id);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldReturnFieldInfoWithNameFromNameFieldBuilder(IdNameFieldBuilder sut, string name, ID id, FieldInfo nameInfo, FieldInfo idInfo)
    {
      sut.NameBuilder.Build(name).Returns(nameInfo);
      sut.IdBuilder.Build(id).Returns(idInfo);
      sut.Build(new object[] { name, id }).Name.Should().Be(nameInfo.Name);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldReturnFieldInfoWithIdFromIdFieldBuilder(IdNameFieldBuilder sut, string name, ID id, FieldInfo nameInfo, FieldInfo idInfo)
    {
      sut.NameBuilder.Build(name).Returns(nameInfo);
      sut.IdBuilder.Build(id).Returns(idInfo);
      sut.Build(new object[] { name, id }).Id.Should().Be(idInfo.Id);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldReturnFieldInfoWithSharedFromNameFieldBuilder(IdNameFieldBuilder sut, string name, ID id, FieldInfo nameInfo, FieldInfo idInfo)
    {
      sut.NameBuilder.Build(name).Returns(nameInfo);
      sut.IdBuilder.Build(id).Returns(idInfo);
      sut.Build(new object[] { name, id }).Shared.Should().Be(nameInfo.Shared);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldReturnFieldInfoWithTypeFromNameFieldBuilder(IdNameFieldBuilder sut, string name, ID id, FieldInfo nameInfo, FieldInfo idInfo)
    {
      sut.NameBuilder.Build(name).Returns(nameInfo);
      sut.IdBuilder.Build(id).Returns(idInfo);
      sut.Build(new object[] { name, id }).Type.Should().Be(nameInfo.Type);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldReturnEmptyInfoIfNameAndIdBuildersReturnEmptyInfo(IdNameFieldBuilder sut, object[] request)
    {
      sut.Build(request).Should().Be(FieldInfo.Empty);
    }
  }
}