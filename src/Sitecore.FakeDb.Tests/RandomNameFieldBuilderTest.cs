namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Xunit;

  public class RandomNameFieldBuilderTest
  {
    [Theory, AutoData]
    public void ShouldBeIDbFieldBuilder(RandomNameFieldBuilder sut)
    {
      sut.Should().BeAssignableTo<IDbFieldBuilder>();
    }

    [Theory, AutoData]
    public void ShouldReturnEmptyInfoIfRequestIsNull(RandomNameFieldBuilder sut)
    {
      sut.Build(null).Should().Be(FieldInfo.Empty);
    }

    [Theory, AutoData]
    public void ShouldReturnEmptyInfoIfRequestIsEmptyString(RandomNameFieldBuilder sut)
    {
      sut.Build(string.Empty).Should().Be(FieldInfo.Empty);
    }

    [Theory, AutoData]
    public void ShouldReturnEmptyInfoIfRequestIsNotString(RandomNameFieldBuilder sut, object request)
    {
      sut.Build(request).Should().Be(FieldInfo.Empty);
    }

    [Theory, AutoData]
    public void ShouldReturnFieldInfoWithSpecifiedName(RandomNameFieldBuilder sut, string name)
    {
      sut.Build(name).Name.Should().Be(name);
    }

    [Theory, AutoData]
    public void ShouldReturnFieldInfoWithNewId(RandomNameFieldBuilder sut, string name)
    {
      sut.Build(name).Id.Guid.Should().NotBeEmpty();
    }

    [Theory, AutoData]
    public void ShouldReturnNotSharedFieldInfo(RandomNameFieldBuilder sut, string name)
    {
      sut.Build(name).Shared.Should().BeFalse();
    }

    [Theory, AutoData]
    public void ShouldReturnTextFieldInfo(RandomNameFieldBuilder sut, string name)
    {
      sut.Build(name).Type.Should().Be("text");
    }
  }
}