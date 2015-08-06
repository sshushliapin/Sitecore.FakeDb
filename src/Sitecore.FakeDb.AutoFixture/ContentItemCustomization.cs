namespace Sitecore.FakeDb.AutoFixture
{
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.Kernel;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  /// <summary>
  /// A customization that adds items to the current <see cref="Database"/>.
  /// The default <see cref="Database"/> is "master".
  /// </summary>
  public class ContentItemCustomization : ICustomization
  {
    /// <summary>
    /// Customizes the specified fixture by adding items to the current <see cref="Database"/>.
    /// </summary>
    /// <param name="fixture">The fixture to customize.</param>
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

      fixture.Customizations.Insert(
        0,
        new FilteringSpecimenBuilder(
          new Postprocessor(
            new MethodInvoker(new ListFavoringConstructorQuery()),
            new AddContentDbItemCommand(db)),
          new DbItemSpecification()));
    }
  }
}