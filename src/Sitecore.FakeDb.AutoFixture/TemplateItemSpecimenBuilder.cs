namespace Sitecore.FakeDb.AutoFixture
{
  using Ploeh.AutoFixture.Kernel;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Items;

  public class TemplateItemSpecimenBuilder : ISpecimenBuilder
  {
    public object Create(object request, ISpecimenContext context)
    {
      if (!typeof(TemplateItem).Equals(request))
      {
        return new NoSpecimen(request);
      }

      return new TemplateItem(ItemHelper.CreateInstance());
    }
  }
}