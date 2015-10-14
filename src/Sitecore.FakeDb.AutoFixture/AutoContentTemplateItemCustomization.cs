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

      fixture.Customizations.Insert(
        0,
        new FilteringSpecimenBuilder(
          new Postprocessor(
            new TemplateItemSpecimenBuilder(), new AddContentTemplateItemCommand()),
            new TemplateItemSpecification()));
    }
  }
}