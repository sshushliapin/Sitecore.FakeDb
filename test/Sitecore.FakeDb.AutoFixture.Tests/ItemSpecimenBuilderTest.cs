namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using FluentAssertions;
  using global::AutoFixture;
  using global::AutoFixture.Kernel;
  using global::AutoFixture;
  using global::AutoFixture.Xunit2;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Globalization;
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

    [Theory, ItemSpecimenBuilderAutoData]
    public void CreateResolvesItemNameFromContext([Frozen] string name, Item item)
    {
      item.Name.Should().Be(name);
    }

    [Theory, ItemSpecimenBuilderAutoData]
    public void CreateResolvesItemIdFromContext([Frozen] ID id, Item item)
    {
      item.ID.Should().BeSameAs(id);
    }

    [Theory, ItemSpecimenBuilderAutoData]
    public void CreateResolveTemplateIdFromContext([Frozen] ID id, Item item)
    {
      item.TemplateID.Should().BeSameAs(id);
    }

    [Theory, ItemSpecimenBuilderAutoData]
    public void CreateResolveBranchIdFromContext([Frozen] ID id, Item item)
    {
      item.BranchId.Should().BeSameAs(id);
    }

    [Theory, ItemSpecimenBuilderAutoData]
    public void CreateResolveFieldListFromContext([Frozen] FieldList fields, Item item)
    {
      item.InnerData.Fields.Should().BeSameAs(fields);
    }

    [Theory, ItemSpecimenBuilderAutoData]
    public void CreateResolveLanguageFromContext([Frozen] Language language, Item item)
    {
      item.Language.Should().BeSameAs(language);
    }

    [Theory, ItemSpecimenBuilderAutoData]
    public void CreateResolveVersionFromContext([Frozen] Version version, Item item)
    {
      item.Version.Should().BeSameAs(version);
    }

    private class ItemSpecimenBuilderAutoDataAttribute : AutoDataAttribute
    {
      public ItemSpecimenBuilderAutoDataAttribute()
      {
        this.Fixture.Customizations.Add(new DatabaseSpecimenBuilder("master"));
        this.Fixture.Customizations.Add(new ItemSpecimenBuilder());
      }
    }
  }
}