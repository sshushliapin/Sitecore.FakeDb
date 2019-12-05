namespace Sitecore.FakeDb.AutoFixture.Tests
{
    using System;
    using FluentAssertions;
    using global::AutoFixture.Kernel;
    using Sitecore.Data;
    using Xunit;

    public class DatabaseSpecimenBuilderTest
    {
        [Fact]
        public void SutIsISpecimenBuilder()
        {
            Assert.IsAssignableFrom<ISpecimenBuilder>(new DatabaseSpecimenBuilder("master"));
        }

        [Fact]
        public void SutThrowsIfDatabaseIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new DatabaseSpecimenBuilder(null));
        }

        [Fact]
        public void CreateReturnsNoSpecimentIfNoDatabaseRequested()
        {
            var sut = new DatabaseSpecimenBuilder("master");
            sut.Create(new object(), null).Should().BeOfType<NoSpecimen>();
        }

        [Theory]
        [InlineData("master")]
        [InlineData("web")]
        [InlineData("core")]
        public void CreateReturnsDatabaseInstance(string databaseName)
        {
            var sut = new DatabaseSpecimenBuilder(databaseName);

            var database = sut.Create(typeof(Database), null);

            database.Should().Match<Database>(x => x.Name == databaseName);
        }
    }
}