namespace Sitecore.FakeDb.AutoFixture
{
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.Kernel;

  public class AutoDbCustomization : ICustomization
  {
    public void Customize(IFixture fixture)
    {
      fixture.Freeze<Db>();
      fixture.Customizations.Add(new DatabaseSpecimenBuilder());
      fixture.Customizations.Add(new ItemSpecimenBuilder());
      fixture.Customizations.Add(
        new FilteringSpecimenBuilder(
          new Postprocessor(
            new MethodInvoker(new ListFavoringConstructorQuery()),
            new SetDefaultDbItemParentCommand()),
          new DbItemSpecification()));
    }
  }
}