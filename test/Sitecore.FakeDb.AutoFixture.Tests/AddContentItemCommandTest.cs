namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using System;
  using FluentAssertions;
  using global::AutoFixture;
  using global::AutoFixture.Kernel;
  using global::AutoFixture.Xunit2;
  using Sitecore.Data.Items;
  using Xunit;

  [Trait("Category", "RequireLicense")]
  public class AddContentItemCommandTest
  {
    [Theory, AutoData]
    public void ExecuteThrowsIfSpecimenIsNull(AddContentItemCommand sut)
    {
      Action action = () => sut.Execute(null, null);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*specimen");
    }

    [Theory, AutoData]
    public void ExecuteThrowsIfContextIsNull(AddContentItemCommand sut, object specimen)
    {
      Action action = () => sut.Execute(specimen, null);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*context");
    }

    [Theory, AutoData]
    public void ExecuteIgnoresNotDbItemSpecimens(AddContentItemCommand sut, object specimen, SpecimenContext context)
    {
      Action action = () => sut.Execute(specimen, context);
      action.ShouldNotThrow();
    }

    [Fact]
    public void CreateAddsItemToDb()
    {
      var fixture = new Fixture();
      var db = fixture.Freeze<Db>();
      fixture.Inject(db.Database);
      var item = fixture.Freeze<Item>(x => x.OmitAutoProperties());
      var sut = new AddContentItemCommand();

      sut.Execute(item, new SpecimenContext(fixture));

      db.GetItem(item.ID).Should().NotBeNull();
    }
  }
}