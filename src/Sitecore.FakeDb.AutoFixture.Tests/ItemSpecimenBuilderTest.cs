namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using FluentAssertions;
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.Kernel;
  using Sitecore.Data.Items;
  using Xunit;

  public class ItemSpecimenBuilderTest
  {
    [Fact]
    public void ShouldBeISpecimenBuilder()
    {
      // arrange
      var customization = new ItemSpecimenBuilder();

      // assert
      customization.Should().BeAssignableTo<ISpecimenBuilder>();
    }

    [Fact]
    public void ShouldReturnNoSpecimentIfNotDatabaseRequested()
    {
      // arrange
      var customization = new ItemSpecimenBuilder();

      // act
      var result = customization.Create(new object(), null);

      result.Should().BeOfType<NoSpecimen>();
    }

    [Fact]
    public void ShouldReturnItemInstance()
    {
      // arrange
      var fixture = new Fixture();
      fixture.Customizations.Add(new ItemSpecimenBuilder());

      // act
      var database = fixture.Create<Item>();

      // assert
      database.Should().NotBeNull();
    }
  }
}