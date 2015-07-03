namespace Sitecore.FakeDb.AutoFixture
{
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.Kernel;

  public class ContentItemCustomization : ICustomization
  {
    public void Customize(IFixture fixture)
    {
      var db = fixture.Create<Db>();
      fixture.Customizations.Add(new Postprocessor(new ContentItemGenerator(db), new ContentItemGeneratorCommand(db)));
    }
  }
}