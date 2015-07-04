namespace Sitecore.FakeDb.AutoFixture
{
  using Ploeh.AutoFixture.Kernel;
  using Sitecore.Data;

  public class DatabaseSpecimenBuilder : ISpecimenBuilder
  {
    public object Create(object request, ISpecimenContext context)
    {
      if (!typeof(Database).Equals(request))
      {
        return new NoSpecimen(request);
      }

      // TODO: Remove the hardcode.
      return Database.GetDatabase("master");
    }
  }
}
