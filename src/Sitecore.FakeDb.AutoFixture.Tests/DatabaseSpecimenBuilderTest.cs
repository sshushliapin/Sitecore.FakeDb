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
      var customization = new DatabaseSpecimenBuilder();

      // assert
      customization.Should().BeAssignableTo<ISpecimenBuilder>();
    }

    [Fact]
    public void ShouldReturnNoSpecimentIfNotDatabaseRequested()
    {
      // arrange
      var customization = new DatabaseSpecimenBuilder();

      // act
      var result = customization.Create(new object(), null);

      // assert
      result.Should().BeOfType<NoSpecimen>();
    }

    [Fact]
    public void ShouldReturnMasterDatabaseInstance()
    {
      // arrange
      var fixture = new Fixture();
      fixture.Customizations.Add(new DatabaseSpecimenBuilder());

      // act
      var database = fixture.Create<Database>();

      // assert
      database.Should().Match<Database>(x => x.Name == "master");
    }

    [Fact]
    public void ShouldSpecifyDatabaseName()
    {
      // arrange
      var fixture = new Fixture();
      fixture.Customizations.Add(new DatabaseSpecimenBuilder("web"));

      // act
      var database = fixture.Create<Database>();

      // assert
      database.Should().Match<Database>(x => x.Name == "web");
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
