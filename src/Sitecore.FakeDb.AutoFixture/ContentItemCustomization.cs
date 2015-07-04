namespace Sitecore.FakeDb.AutoFixture
{
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.Kernel;
  using Sitecore.Diagnostics;

  public class ContentItemCustomization : ICustomization
  {
    public void Customize(IFixture fixture)
    {
      Assert.ArgumentNotNull(fixture, "fixture");

      var db = fixture.Create<Db>();

      fixture.Customizations.Insert(
        0,
        new FilteringSpecimenBuilder(
          new Postprocessor(
            new ItemSpecimenBuilder(), new AddContentItemCommand(db)),
            new ItemSpecification()));

      fixture.Customizations.Add(
        new FilteringSpecimenBuilder(
          new Postprocessor(
            new MethodInvoker(new ListFavoringConstructorQuery()),
            new AddContentDbItemCommand(db)),
          new DbItemSpecification()));
    }
  }
}