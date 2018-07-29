namespace Sitecore.FakeDb.AutoFixture
{
    using global::AutoFixture.Kernel;
    using Sitecore.Data.Items;
    using Sitecore.FakeDb.Data.Items;

    public class TemplateItemSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(TemplateItem).Equals(request))
            {
                return new NoSpecimen();
            }

            return new TemplateItem(ItemHelper.CreateInstance());
        }
    }
}