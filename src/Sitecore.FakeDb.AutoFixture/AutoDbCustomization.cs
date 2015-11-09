namespace Sitecore.FakeDb.AutoFixture
{
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.Kernel;
  using Sitecore.Data;
  using Sitecore.Data.Items;

  /// <summary>
  /// A customization that enables creation of Sitecore types such as <see cref="Database"/> 
  /// or <see cref="Item"/>.
  /// </summary>
  public class AutoDbCustomization : ICustomization
  {
    /// <summary>
    /// Customizes the specified fixture by adding the Sitecore specific specimen builders.
    /// </summary>
    /// <param name="fixture">The fixture to customize.</param>
    public void Customize(IFixture fixture)
    {
      fixture.Freeze<Db>();
      fixture.Customize(new ContextDatabaseCustomization());
      fixture.Customizations.Add(
        new CompositeSpecimenBuilder(
          new ItemSpecimenBuilder(),
          new FilteringSpecimenBuilder(
            new Postprocessor(
              new ContentAttributeRelay(),
              new AddContentDbItemCommand()),
            new DbItemParameterSpecification()),
          new FilteringSpecimenBuilder(
            new Postprocessor(
              new ContentAttributeRelay(),
              new AddContentItemCommand()),
            new ItemParameterSpecification()),
          new FilteringSpecimenBuilder(
            new Omitter(),
            new PropertySpecification(typeof(ID), "ParentID")),
          new FilteringSpecimenBuilder(
            new Omitter(),
            new PropertySpecification(typeof(ID), "TemplateID")),
          new FilteringSpecimenBuilder(
            new Omitter(),
            new PropertySpecification(typeof(ID[]), "BaseIDs")),
          new Postprocessor(
            new SwitchingSpecimenBuilder(),
            new SwitchCommand())));
    }
  }
}