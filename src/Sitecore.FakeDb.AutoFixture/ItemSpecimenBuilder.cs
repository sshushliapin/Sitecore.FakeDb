namespace Sitecore.FakeDb.AutoFixture
{
  using Ploeh.AutoFixture.Kernel;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Items;

  public class ItemSpecimenBuilder : ISpecimenBuilder
  {
    public object Create(object request, ISpecimenContext context)
    {
      if (!typeof(Item).Equals(request))
      {
        return new NoSpecimen(request);
      }

      return ItemHelper.CreateInstance();
    }
  }
}