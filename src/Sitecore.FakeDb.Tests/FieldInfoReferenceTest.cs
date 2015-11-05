namespace Sitecore.FakeDb.Tests
{
  using System;
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
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
      sut[Guid.NewGuid()].Should().Be(FieldInfo.Empty);
    }

    [Theory, AutoData]
    internal void ShouldReturnFieldInfoForKnownId(FieldInfoReference sut)
    {
      sut[FieldIDs.DisplayName.Guid].Should().NotBe(FieldInfo.Empty);
    }
  }
}