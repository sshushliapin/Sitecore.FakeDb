namespace Sitecore.FakeDb.Tests
{
    using FluentAssertions;
    using global::AutoFixture.Xunit2;
    using Sitecore.Data;
    using Xunit;

    public class FieldInfoReferenceTest
    {
        [Theory, AutoData]
        internal void ShouldReturnEmptyFieldInfoForUnknownName(StandardFieldsReference sut)
        {
            sut["unknown field"].Should().Be(FieldInfo.Empty);
        }

        [Theory, AutoData]
        internal void ShouldReturnFieldInfoForKnownName(StandardFieldsReference sut)
        {
            sut["__Display name"].Should().NotBe(FieldInfo.Empty);
        }

        [Theory, AutoData]
        internal void ShouldReturnEmptyFieldInfoForUnknownId(StandardFieldsReference sut)
        {
            sut[ID.NewID].Should().Be(FieldInfo.Empty);
        }

        [Theory, AutoData]
        internal void ShouldReturnFieldInfoForKnownId(StandardFieldsReference sut)
        {
            sut[FieldIDs.DisplayName].Should().NotBe(FieldInfo.Empty);
        }
    }
}