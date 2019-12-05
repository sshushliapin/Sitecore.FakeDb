namespace Sitecore.FakeDb.AutoFixture
{
    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using Sitecore.Diagnostics;

    public class AutoContentDbItemCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            Assert.ArgumentNotNull(fixture, "fixture");

            fixture.Customizations.Insert(
                0,
                new FilteringSpecimenBuilder(
                    new Postprocessor(
                        new MethodInvoker(new ListFavoringConstructorQuery()),
                        new AddContentDbItemCommand()),
                    new DbItemSpecification()));
        }
    }
}