namespace Sitecore.FakeDb.AutoFixture
{
  using System;
  using Ploeh.AutoFixture.Kernel;

  public class DbItemSpecification : IRequestSpecification
  {
    public bool IsSatisfiedBy(object request)
    {
      var type = request as Type;
      return typeof(DbItem).IsAssignableFrom(type);
    }
  }
}