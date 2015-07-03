namespace Sitecore.FakeDb.AutoFixture
{
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.Kernel;
  using Sitecore.Data.Items;

  public class AutoDbCustomization : ICustomization
  {
    public void Customize(IFixture fixture)
    {
      var db = fixture.Create<Db>();
      // TODO: Avoind the Inject call.
      fixture.Inject(db);

      fixture.Customizations.Add(new DatabaseSpecimenBuilder());
      fixture.Customizations.Add(new ItemSpecimenBuilder());
    }
  }
}