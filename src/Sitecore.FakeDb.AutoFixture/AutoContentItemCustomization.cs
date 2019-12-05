namespace Sitecore.FakeDb.AutoFixture
{
    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using Sitecore.Diagnostics;

    public class AutoContentItemCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            Assert.ArgumentNotNull(fixture, "fixture");

            fixture.Customizations.Insert(
                0,
                new FilteringSpecimenBuilder(
                    new Postprocessor(
                        new ItemSpecimenBuilder(), new AddContentItemCommand()),
                    new ItemSpecification()));
        }
    }
}