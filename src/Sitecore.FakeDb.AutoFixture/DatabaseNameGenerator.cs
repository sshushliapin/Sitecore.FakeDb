namespace Sitecore.FakeDb.AutoFixture
{
  using System;
  using Ploeh.AutoFixture.Kernel;

  public class DatabaseNameGenerator : ISpecimenBuilder
  {
    public object Create(object request, ISpecimenContext context)
    {
      if (request == null)
      {
        return new NoSpecimen();
      }

      var seededRequest = request as SeededRequest;
      if (seededRequest == null)
      {
        return new NoSpecimen();
      }

      if (typeof(string) != seededRequest.Request as Type)
      {
        return new NoSpecimen();
      }

      var seed = (string)seededRequest.Seed;
      if (seed != "databaseName" && seed != "DatabaseName")
      {
        return new NoSpecimen();
      }

      return "master";
    }
  }
}