namespace Sitecore.FakeDb.AutoFixture.Tests
{
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
  }
}
