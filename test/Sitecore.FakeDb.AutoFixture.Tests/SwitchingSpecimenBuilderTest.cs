namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using System;
  using System.Reflection;
  using FluentAssertions;
  using NSubstitute;
  using Ploeh.AutoFixture.Kernel;
  using Ploeh.AutoFixture.Xunit2;
  using Xunit;

  public class SwitchingSpecimenBuilderTest
  {
    [Theory, AutoData]
    public void ShouldBeICustomization(SwitchingSpecimenBuilder sut)
    {
      sut.Should().BeAssignableTo<ISpecimenBuilder>();
    }

    [Theory, AutoData]
    public void ShouldReturnNoSpecimenIfRequestIsNull(SwitchingSpecimenBuilder sut)
    {
      sut.Create(null, null).Should().BeOfType<NoSpecimen>();
    }

    [Theory, AutoData]
    public void CreateReturnsNoSpecimentIfRequestIsNotICustomAttributeProvider(SwitchingSpecimenBuilder sut)
    {
      sut.Create(new object(), null).Should().BeOfType<NoSpecimen>();
    }

    [Theory, AutoData]
    public void CreateReturnsNoSpecimenIfICustomAttributeProviderDoesNotReturnExpectedAttributeType(SwitchingSpecimenBuilder sut)
    {
      var request = Substitute.For<ICustomAttributeProvider>();
      request.GetCustomAttributes(Arg.Any<Type>(), Arg.Any<bool>()).Returns(new object[0]);

      sut.Create(request, null).Should().BeOfType<NoSpecimen>();
    }

    [Theory, AutoData]
    public void CreateReturnsNoSpecimenIfRequestIsNotParameterInfo(SwitchingSpecimenBuilder sut)
    {
      var request = Substitute.For<ICustomAttributeProvider>();
      request.GetCustomAttributes(Arg.Any<Type>(), Arg.Any<bool>()).Returns(new object[] { new SwitchedAttribute() });

      sut.Create(request, null).Should().BeOfType<NoSpecimen>();
    }
  }
}