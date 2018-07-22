namespace Sitecore.FakeDb.AutoFixture
{
  using global::AutoFixture;
  using Sitecore.Diagnostics;

  public class ContextDatabaseCustomization : ICustomization
  {
    public void Customize(IFixture fixture)
    {
      Assert.ArgumentNotNull(fixture, "fixture");

      fixture.Customizations.Add(new ContextDatabaseSpecimenBuilder());
    }
  }
}