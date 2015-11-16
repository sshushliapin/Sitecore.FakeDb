namespace Sitecore.FakeDb.Tests
{
  using System;
  using System.Linq;
  using FluentAssertions;
  using NSubstitute;
  using Ploeh.AutoFixture.Xunit2;
  using Xunit;

  public class MixedFieldBuilderTest
  {
    [Theory, DefaultSubstituteAutoData]
    public void ShouldBeIDbFieldBuilder(MixedFieldBuilder sut)
    {
      sut.Should().BeAssignableTo<IDbFieldBuilder>();
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldContainInnerBilder([Frozen]IDbFieldBuilder builder, MixedFieldBuilder sut)
    {
      sut.Builder.Should().BeSameAs(builder);
    }

    [Fact]
    public void ShouldThrowIfBuilderIsNull()
    {
      Action action = () => new MixedFieldBuilder(null);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*builder");
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldCallInnerBuilderIfNotEnumerablePassed(MixedFieldBuilder sut, object request, FieldInfo expected)
    {
      sut.Builder.Build(request).Returns(expected);
      sut.Build(request).Should().Be(expected);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldReturnEmptyInfoIfEmptyRequestPassed(MixedFieldBuilder sut)
    {
      sut.Build(new { }).Should().Be(FieldInfo.Empty);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldCallInnerBuilderForEachMixedRequestPartIfRequestIsEnumerable(MixedFieldBuilder sut, object[] request)
    {
      sut.Build(request);

      sut.Builder.Received().Build(request.ElementAt(0));
      sut.Builder.Received().Build(request.ElementAt(1));
      sut.Builder.Received().Build(request.ElementAt(2));
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldReturnFirstFieldInfoThatIsNotEmpty(MixedFieldBuilder sut, object[] request, FieldInfo fieldInfo)
    {
      sut.Builder.Build(request.ElementAt(1)).Returns(fieldInfo);
      sut.Build(request).Should().Be(fieldInfo);
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldStopIteratingMixedRequestsIfFieldInfoResolved(MixedFieldBuilder sut, object[] request, FieldInfo fieldInfo)
    {
      sut.Builder.Build(request.ElementAt(1)).Returns(fieldInfo);
      sut.Build(request);
      sut.Builder.DidNotReceive().Build(request.ElementAt(2));
    }
  }
}