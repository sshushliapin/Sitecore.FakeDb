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
    public void ShouldBeISpecimenBuilder()
    {
      // arrange
      var customization = new DatabaseSpecimenBuilder("master");

      // assert
      customization.Should().BeAssignableTo<ISpecimenBuilder>();
    }

    [Fact]
    public void ShouldReturnNoSpecimentIfNotDatabaseRequested()
    {
      // arrange
      var customization = new DatabaseSpecimenBuilder("master");

      // act
      var result = customization.Create(new object(), null);

      // assert
      result.Should().BeOfType<NoSpecimen>();
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
