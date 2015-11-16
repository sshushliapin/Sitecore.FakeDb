namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data;
  using Xunit;

  public class RandomIdFieldBuilderTest
  {
    [Theory, AutoData]
    public void ShouldBeIDbFieldBuilder(RandomIdFieldBuilder sut)
    {
      sut.Should().BeAssignableTo<IDbFieldBuilder>();
    }

    [Theory, AutoData]
    public void ShouldReturnEmptyInfoIfRequestIsNull(RandomIdFieldBuilder sut)
    {
      sut.Build(null).Should().Be(FieldInfo.Empty);
    }

    [Theory, AutoData]
    public void ShouldReturnEmptyInfoIfRequestIsIdNull(RandomIdFieldBuilder sut)
    {
      sut.Build(ID.Null).Should().Be(FieldInfo.Empty);
    }

    [Theory, AutoData]
    public void ShouldReturnEmptyInfoIfRequestIsNotString(RandomIdFieldBuilder sut, object request)
    {
      sut.Build(request).Should().Be(FieldInfo.Empty);
    }

    [Theory, AutoData]
    public void ShouldReturnFieldInfoWithSpecifiedId(RandomIdFieldBuilder sut, ID id)
    {
      sut.Build(id).Id.Should().Be(id);
    }

    [Theory, AutoData]
    public void ShouldReturnFieldInfoWithNameFromId(RandomIdFieldBuilder sut, ID id)
    {
      sut.Build(id).Name.Should().Be(id.ToShortID().ToString());
    }

    [Theory, AutoData]
    public void ShouldReturnNotSharedFieldInfo(RandomIdFieldBuilder sut, ID id)
    {
      sut.Build(id).Shared.Should().BeFalse();
    }

    [Theory, AutoData]
    public void ShouldReturnTextFieldInfo(RandomIdFieldBuilder sut, ID id)
    {
      sut.Build(id).Type.Should().Be("text");
    }
  }
}