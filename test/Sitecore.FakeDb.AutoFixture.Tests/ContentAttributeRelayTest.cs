namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using System;
  using System.Reflection;
  using FluentAssertions;
  using NSubstitute;
  using Ploeh.AutoFixture.Kernel;
  using Xunit;

  public class ContentAttributeRelayTest
  {
    [Fact]
    public void SutIsISpecimenBuilder()
    {
      Assert.IsAssignableFrom<ISpecimenBuilder>(new ContentAttributeRelay());
    }

    [Fact]
    public void CreateReturnsNoSpecimentIfRequestIsNull()
    {
      var sut = new ContentAttributeRelay();
      sut.Create(null, null).Should().BeOfType<NoSpecimen>();
    }

    [Fact]
    public void CreateReturnsNoSpecimentIfRequestIsNotICustomAttributeProvider()
    {
      var sut = new ContentAttributeRelay();
      sut.Create(new object(), null).Should().BeOfType<NoSpecimen>();
    }

    [Fact]
    public void CreateReturnsNoSpecimenIfICustomAttributeProviderDoesNotReturnExpectedAttributeType()
    {
      var sut = new ContentAttributeRelay();
      var request = Substitute.For<ICustomAttributeProvider>();
      request.GetCustomAttributes(Arg.Any<Type>(), Arg.Any<bool>()).Returns(new object[0]);

      sut.Create(request, null).Should().BeOfType<NoSpecimen>();
    }

    [Fact]
    public void CreateReturnsNoSpecimenIfRequestIsNotParameterInfo()
    {
      var sut = new ContentAttributeRelay();
      var request = Substitute.For<ICustomAttributeProvider>();
      request.GetCustomAttributes(Arg.Any<Type>(), Arg.Any<bool>()).Returns(new object[] { new ContentAttribute() });

      sut.Create(request, null).Should().BeOfType<NoSpecimen>();
    }
  }
}