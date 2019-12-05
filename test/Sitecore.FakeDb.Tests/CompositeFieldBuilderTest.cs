namespace Sitecore.FakeDb.Tests
{
    using System.Linq;
    using FluentAssertions;
    using NSubstitute;
    using global::AutoFixture.Xunit2;
    using Xunit;

    public class CompositeFieldBuilderTest
    {
        [Theory, DefaultSubstituteAutoData]
        public void ShouldBeIDbFieldBuilder(CompositeFieldBuilder sut)
        {
            sut.Should().BeAssignableTo<IDbFieldBuilder>();
        }

        [Fact]
        public void ShouldContainListOfBuilders()
        {
            new CompositeFieldBuilder().Builders.Should().BeEquivalentTo(Enumerable.Empty<IDbFieldBuilder>());
        }

        [Theory, DefaultSubstituteAutoData]
        public void ShouldReceiveListOfBuilders(CompositeFieldBuilder sut)
        {
            sut.Builders.Should().HaveCount(3);
        }

        [Theory, AutoData]
        public void ShouldReturnEmptyInfoIfNoBuildersPassed(object request)
        {
            new CompositeFieldBuilder().Build(request).Should().Be(FieldInfo.Empty);
        }

        [Theory, DefaultSubstituteAutoData]
        public void ShouldCallEachBuilder([Frozen] IDbFieldBuilder[] builders, CompositeFieldBuilder sut, object request)
        {
            sut.Build(request);

            builders.ElementAt(0).Received().Build(request);
            builders.ElementAt(1).Received().Build(request);
            builders.ElementAt(2).Received().Build(request);
        }

        [Theory, DefaultSubstituteAutoData]
        public void ShouldReturnFirstFieldInfoThatIsNotEmpty([Frozen] IDbFieldBuilder[] builders, CompositeFieldBuilder sut, object request, FieldInfo fieldInfo)
        {
            builders.ElementAt(1).Build(request).Returns(fieldInfo);
            sut.Build(request).Should().Be(fieldInfo);
        }

        [Theory, DefaultSubstituteAutoData]
        public void ShouldStopIteratingBuildersIfFieldInfoResolved([Frozen] IDbFieldBuilder[] builders, CompositeFieldBuilder sut, object request, FieldInfo fieldInfo)
        {
            builders.ElementAt(1).Build(request).Returns(fieldInfo);
            sut.Build(request);
            builders.ElementAt(2).DidNotReceiveWithAnyArgs().Build(null);
        }

        [Theory, DefaultSubstituteAutoData]
        public void ShouldAddBuilders(CompositeFieldBuilder sut, IDbFieldBuilder builder)
        {
            sut.Builders.Add(builder);
            sut.Builders.Should().HaveCount(4);
        }
    }
}