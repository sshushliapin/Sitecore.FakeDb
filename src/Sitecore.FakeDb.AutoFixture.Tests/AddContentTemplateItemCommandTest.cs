namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using System;
  using FluentAssertions;
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.Kernel;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data.Items;
  using Xunit;

  public class AddContentTemplateItemCommandTest
  {
    [Theory, AutoData]
    public void ExecuteThrowsIfSpecimenIsNull(AddContentTemplateItemCommand sut)
    {
      Action action = () => sut.Execute(null, null);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*specimen");
    }

    [Theory, AutoData]
    public void ExecuteThrowsIfContextIsNull(AddContentTemplateItemCommand sut, object specimen)
    {
      Action action = () => sut.Execute(specimen, null);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*context");
    }

    [Theory, AutoData]
    public void ExecuteIgnoresNotTemplateItemSpecimens(AddContentTemplateItemCommand sut, object specimen, SpecimenContext context)
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
      fixture.Customize<Item>(x => x.OmitAutoProperties());
      var templateItem = fixture.Freeze<TemplateItem>(x => x.OmitAutoProperties());
      var sut = new AddContentTemplateItemCommand();

      sut.Execute(templateItem, new SpecimenContext(fixture));

      db.GetItem(templateItem.ID).Should().NotBeNull();
    }
  }
}