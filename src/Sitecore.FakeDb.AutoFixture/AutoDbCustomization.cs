namespace Sitecore.FakeDb.AutoFixture
{
  using Ploeh.AutoFixture;

  public class AutoDbCustomization : ICustomization
  {
    public void Customize(IFixture fixture)
    {
      fixture.Customizations.Add(new DatabaseSpecimenBuilder());
      fixture.Customizations.Add(new ItemSpecimenBuilder());
    }
  }
}