namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data;
  using Xunit;

  public class FieldInfoReferenceTest
  {
    [Theory, AutoData]
    internal void ShouldReturnEmptyFieldInfoForUnknownName(FieldInfoReference sut)
    {
      sut["unknown field"].Should().Be(FieldInfo.Empty);
    }

    [Theory, AutoData]
    internal void ShouldReturnFieldInfoForKnownName(FieldInfoReference sut)
    {
      sut["__Display name"].Should().NotBe(FieldInfo.Empty);
    }

    [Theory, AutoData]
    internal void ShouldReturnEmptyFieldInfoForUnknownId(FieldInfoReference sut)
    {
      sut[ID.NewID].Should().Be(FieldInfo.Empty);
    }

    [Theory, AutoData]
    internal void ShouldReturnFieldInfoForKnownId(FieldInfoReference sut)
    {
      sut[FieldIDs.DisplayName].Should().NotBe(FieldInfo.Empty);
    }
  }
}