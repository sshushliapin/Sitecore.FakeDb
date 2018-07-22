namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using FluentAssertions;
  using global::AutoFixture;
  using global::AutoFixture.Kernel;
  using Sitecore.Data.Items;
  using Xunit;

  public class TemplateItemSpecimenBuilderTest
  {
    [Fact]
    public void SutIsSpecimenBuilder()
    {
      var sut = new TemplateItemSpecimenBuilder();
      sut.Should().BeAssignableTo<ISpecimenBuilder>();
    }

    [Fact]
    public void CreateReturnsNoSpecimentIfNoItemRequested()
    {
      var sut = new TemplateItemSpecimenBuilder();
      sut.Create(new object(), null).Should().BeOfType<NoSpecimen>();
    }

    [Fact]
    public void CreateReturnsTemplateItemInstance()
    {
      var fixture = new Fixture();
      fixture.Customizations.Add(new TemplateItemSpecimenBuilder());

      fixture.Create<TemplateItem>().Should().NotBeNull();
    } 
  }
}