namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using FluentAssertions;
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.Kernel;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Xunit;

  public class ItemSpecimenBuilderTest
  {
    [Fact]
    public void SutIsSpecimenBuilder()
    {
      var sut = new ItemSpecimenBuilder();
      sut.Should().BeAssignableTo<ISpecimenBuilder>();
    }

    [Fact]
    public void CreateReturnsNoSpecimentIfNoItemRequested()
    {
      var sut = new ItemSpecimenBuilder();
      sut.Create(new object(), null).Should().BeOfType<NoSpecimen>();
    }

    [Fact]
    public void CreateReturnsItemInstance()
    {
      var fixture = new Fixture();
      fixture.Customizations.Add(new DatabaseSpecimenBuilder("master"));
      fixture.Customizations.Add(new ItemSpecimenBuilder());

      fixture.Create<Item>().Should().NotBeNull();
    }

    [Fact]
    public void CreateResolvesDatabaseFromContext()
    {
      var fixture = new Fixture();
      fixture.Customizations.Add(new DatabaseSpecimenBuilder("core"));
      fixture.Customizations.Add(new ItemSpecimenBuilder());

      fixture.Create<Item>().Database.Name.Should().Be("core");
    }

    [Fact]
    public void CreateResolvesItemNameFromContext()
    {
      var fixture = new Fixture();
      fixture.Customizations.Add(new DatabaseSpecimenBuilder("master"));
      fixture.Customizations.Add(new ItemSpecimenBuilder());

      var frozenString = fixture.Freeze<string>();
      fixture.Create<Item>().Name.Should().Be(frozenString);
    }

    [Fact]
    public void CreateResolvesItemIdFromContext()
    {
      var fixture = new Fixture();
      fixture.Customizations.Add(new DatabaseSpecimenBuilder("master"));
      fixture.Customizations.Add(new ItemSpecimenBuilder());

      var frozenId = fixture.Freeze<ID>();
      fixture.Create<Item>().ID.Should().BeSameAs(frozenId);
    }
  }
}