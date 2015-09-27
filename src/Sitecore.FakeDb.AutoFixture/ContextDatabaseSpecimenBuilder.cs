namespace Sitecore.FakeDb.AutoFixture
{
  using Ploeh.AutoFixture.Kernel;
  using Sitecore.Data;

  public class ContextDatabaseSpecimenBuilder : ISpecimenBuilder
  {
    public object Create(object request, ISpecimenContext context)
    {
      if (!typeof(Database).Equals(request))
      {
        return new NoSpecimen(request);
      }

      var database = Context.Database;
      if (database == null)
      {
        return new NoSpecimen(request);
      }

      return new DatabaseSpecimenBuilder(database.Name).Create(request, context);
    }
  }
}