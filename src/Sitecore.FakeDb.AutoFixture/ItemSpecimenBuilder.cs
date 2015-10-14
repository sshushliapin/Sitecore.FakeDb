namespace Sitecore.FakeDb.AutoFixture
{
  using Ploeh.AutoFixture.Kernel;
  using Sitecore.Data;
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

      var database = (Database)context.Resolve(typeof(Database));
      var name = (string)context.Resolve(typeof(string));
      var id = (ID)context.Resolve(typeof(ID));

      return ItemHelper.CreateInstance(database, name, id);
    }
  }
}