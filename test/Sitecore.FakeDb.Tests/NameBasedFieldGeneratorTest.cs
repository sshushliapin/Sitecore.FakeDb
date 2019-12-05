namespace Sitecore.FakeDb.Tests
{
    using FluentAssertions;
    using global::AutoFixture.Xunit2;
    using Xunit;

    public class NameBasedFieldGeneratorTest
    {
        [Theory, AutoData]
        public void ShouldBeIDbFieldBuilder(NameBasedFieldGenerator sut)
        {
            sut.Should().BeAssignableTo<IDbFieldBuilder>();
        }

        [Theory, AutoData]
        public void ShouldReturnEmptyInfoIfRequestIsNull(NameBasedFieldGenerator sut)
        {
            sut.Build(null).Should().Be(FieldInfo.Empty);
        }

        [Theory, AutoData]
        public void ShouldReturnEmptyInfoIfRequestIsEmptyString(NameBasedFieldGenerator sut)
        {
            sut.Build(string.Empty).Should().Be(FieldInfo.Empty);
        }

        [Theory, AutoData]
        public void ShouldReturnEmptyInfoIfRequestIsNotString(NameBasedFieldGenerator sut, object request)
        {
            sut.Build(request).Should().Be(FieldInfo.Empty);
        }

        [Theory, AutoData]
        public void ShouldReturnFieldInfoWithSpecifiedName(NameBasedFieldGenerator sut, string name)
        {
            sut.Build(name).Name.Should().Be(name);
        }

        [Theory, AutoData]
        public void ShouldReturnFieldInfoWithNewId(NameBasedFieldGenerator sut, string name)
        {
            sut.Build(name).Id.Guid.Should().NotBeEmpty();
        }

        [Theory, AutoData]
        public void ShouldReturnNotSharedFieldInfo(NameBasedFieldGenerator sut, string name)
        {
            sut.Build(name).Shared.Should().BeFalse();
        }

        [Theory, AutoData]
        public void ShouldReturnTextFieldInfo(NameBasedFieldGenerator sut, string name)
        {
            sut.Build(name).Type.Should().Be("text");
        }
    }
}