namespace Sitecore.FakeDb.AutoFixture
{
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.Kernel;
  using Sitecore.Diagnostics;

  public class AutoContentTemplateItemCustomization : ICustomization
  {
    public void Customize(IFixture fixture)
    {
      Assert.ArgumentNotNull(fixture, "fixture");

      var db = fixture.Create<Db>();

      fixture.Customizations.Insert(
        0,
        new FilteringSpecimenBuilder(
          new Postprocessor(
            new TemplateItemSpecimenBuilder(), new AddContentTemplateItemCommand(db)),
            new TemplateItemSpecification()));
    }
  }
}