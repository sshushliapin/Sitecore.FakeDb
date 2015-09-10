namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using System;
  using FluentAssertions;
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.Kernel;
  using Sitecore.Data;
  using Xunit;

  public class DatabaseSpecimenBuilderTest
  {
    [Fact]
    public void SutIsISpecimenBuilder()
    {
      var sut = new DatabaseSpecimenBuilder("master");
      sut.Should().BeAssignableTo<ISpecimenBuilder>();
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
    public void ShouldReturnDatabaseInstance(string databaseName)
    {
      // arrange
      var fixture = new Fixture();
      fixture.Customizations.Add(new DatabaseSpecimenBuilder(databaseName));

      // act
      var database = fixture.Create<Database>();

      // assert
      database.Should().Match<Database>(x => x.Name == databaseName);
    }

    [Fact]
    public void ShouldThrowIfDatabaseIdNull()
    {
      // act
      Action action = () => new DatabaseSpecimenBuilder(null);

      // assert
      action.ShouldThrow<ArgumentNullException>();
    }
  }
}
