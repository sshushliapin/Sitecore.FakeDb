namespace Sitecore.FakeDb.Tests
{
    using FluentAssertions;
    using global::AutoFixture.Xunit2;
    using Sitecore.Data;
    using Xunit;

    public class IdBasedFieldGeneratorTest
    {
        [Theory, AutoData]
        public void ShouldBeIDbFieldBuilder(IdBasedFieldGenerator sut)
        {
            sut.Should().BeAssignableTo<IDbFieldBuilder>();
        }

        [Theory, AutoData]
        public void ShouldReturnEmptyInfoIfRequestIsNull(IdBasedFieldGenerator sut)
        {
            sut.Build(null).Should().Be(FieldInfo.Empty);
        }

        [Theory, AutoData]
        public void ShouldReturnEmptyInfoIfRequestIsIdNull(IdBasedFieldGenerator sut)
        {
            sut.Build(ID.Null).Should().Be(FieldInfo.Empty);
        }

        [Theory, AutoData]
        public void ShouldReturnEmptyInfoIfRequestIsNotString(IdBasedFieldGenerator sut, object request)
        {
            sut.Build(request).Should().Be(FieldInfo.Empty);
        }

        [Theory, AutoData]
        public void ShouldReturnFieldInfoWithSpecifiedId(IdBasedFieldGenerator sut, ID id)
        {
            sut.Build(id).Id.Should().Be(id);
        }

        [Theory, AutoData]
        public void ShouldReturnFieldInfoWithNameFromId(IdBasedFieldGenerator sut, ID id)
        {
            sut.Build(id).Name.Should().Be(id.ToShortID().ToString());
        }

        [Theory, AutoData]
        public void ShouldReturnNotSharedFieldInfo(IdBasedFieldGenerator sut, ID id)
        {
            sut.Build(id).Shared.Should().BeFalse();
        }

        [Theory, AutoData]
        public void ShouldReturnTextFieldInfo(IdBasedFieldGenerator sut, ID id)
        {
            sut.Build(id).Type.Should().Be("text");
        }
    }
}