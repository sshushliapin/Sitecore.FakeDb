namespace Sitecore.FakeDb.AutoFixture
{
  using Ploeh.AutoFixture;

  public class AutoDbCustomization : ICustomization
  {
    public void Customize(IFixture fixture)
    {
      var db = fixture.Create<Db>();
      fixture.Inject(db);
      fixture.Customizations.Add(new DatabaseSpecimenBuilder());
      fixture.Customizations.Add(new ItemSpecimenBuilder());
    }
  }
}